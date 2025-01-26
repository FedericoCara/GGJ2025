using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace BubbleNS
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStats playerstats;

        public Image barraDeoxigeno;    //TO DO: Remove this and all of its references

        public float movingSpeed;
        public float jumpForce;
        public int dashForce;
        public float dashingStopSpeed = 0.2f;
        public int dashCost = 10;

        public int fireCost = 5;
        public int damageForce = 5;
        public float receiveDamageDuration = 0.5f;
        private float moveInput;

        private Inputs inputs;

        private bool facingRight = false;

        [HideInInspector]
        public bool deathState = false;


        private bool isGrounded;
        public Transform groundCheck;
        public float fallingGravity = 0.5f;
        public float dashingDrag = 5;
        public List<float> firingDelayPerIntensity = new(){0.5f,0.25f};
        public Vector2 verticalSpeedLimits = new Vector2(-10, 10);
        public List<FireBubble> fireBubbles;
        public Transform bubbleSpawnPosition;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private InputAction _move;
        private InputAction _jump;
        private InputAction _dash;
        private InputAction _fire;
        private bool _dashing;
        private bool _firing;
        private float _fireWaitingTime;
        private float _receivingDamageDurationLeft;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            inputs = new Inputs();
            _move = inputs.Player.Move;
            _move.Enable();
            _jump = inputs.Player.Jump;
            _jump.Enable();
            _dash = inputs.Player.Dash;
            _dash.Enable();
            _fire = inputs.Player.Fire;
            _fire.Enable();
        }

        private void OnDestroy()
        {
            inputs.Disable();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (_receivingDamageDurationLeft>0)
            {
                _receivingDamageDurationLeft -= Time.deltaTime;
                return;
            }
            
            if (IsDashingButtonDown() && CanDash())
            {

                StartDash();
            }

            if (CheckDashingFinished())
            {
                _dashing = false;
                rigidbody.drag = 0;
            }
            CheckFiringDelay();

            if (!_dashing && CanFire())
            {
                FireBubbles();
            } else if (_move.IsPressed())
            {
                moveInput = _move.ReadValue<Vector2>().x;;
                if (_dashing && MovingOppositeDirection(moveInput))
                {
                    CancelDash();
                }else if (!_dashing || IsDashingFinishing())
                {
                    var direction = transform.right * moveInput;
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                    animator.SetInteger("playerState", 1); // Turn on run animation
                }
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }
            if(_jump.triggered && isGrounded )
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation

            if(!facingRight && moveInput > 0 || facingRight && moveInput<0)
            {
                Flip();
            }
            
            rigidbody.gravityScale = rigidbody.velocity.y < 0 ? fallingGravity : 1;
            LimitVerticalSpeed();
        }

        private bool CanDash() => !_dashing && playerstats.oxigin >= dashCost;

        private bool CanFire()
        {
            return _fire.IsPressed() && !_firing && _fireWaitingTime <= 0;
        }

        private void CheckFiringDelay()
        {
            if (_firing)
            {
                _fireWaitingTime -= Time.deltaTime;
                if (_fireWaitingTime <= 0)
                {
                    _fireWaitingTime = 0;
                    _firing = false;
                }
            }
        }

        private void FireBubbles()
        {

            _firing = true;
            _fireWaitingTime = firingDelayPerIntensity[CalcIntensity()];
            SpawnBubbles();

            playerstats.ModifyOxygen(-fireCost);
            
            if (barraDeoxigeno != null)
                barraDeoxigeno.fillAmount -= fireCost / 100f;
        }

        private int _bubbleIndex;
        private void SpawnBubbles()
        {
            var bubbleSpawned = Instantiate(GetNextBubble(), bubbleSpawnPosition.position, Quaternion.identity);
            bubbleSpawned.Initialize(facingRight, CalcIntensity());
        }

        private FireBubble GetNextBubble()
        {
            var index = _bubbleIndex;
            _bubbleIndex = (_bubbleIndex + 1) % fireBubbles.Count;
            return fireBubbles[index];
        }

        private int CalcIntensity() => playerstats.oxigin/100f>0.3f?1:0;

        private bool IsDashingFinishing()
        {
            return _dashing && Mathf.Abs(rigidbody.velocity.x) <= dashingStopSpeed;
        }

        private void StartDash()
        {
            rigidbody.AddForce(new Vector2((facingRight ? 1 : -1) * dashForce, 0), ForceMode2D.Impulse);
            rigidbody.drag = dashingDrag;
            playerstats.ModifyOxygen(-dashCost);
            if(barraDeoxigeno!=null)
                barraDeoxigeno.fillAmount -= dashCost/100f;

            _dashing = true;
        }

        private void CancelDash()
        {
            _dashing = false;
            rigidbody.velocity = StopXMovement();
            rigidbody.drag = 0;
        }

        private Vector2 StopXMovement() => new(0, rigidbody.velocity.y);

        private bool MovingOppositeDirection(float direction)
        {
            return rigidbody.velocity.x > 0 && direction < 0 ||
                   rigidbody.velocity.x < 0 && direction > 0;
        }

        private bool CheckDashingFinished()
        {
            return !_dashing || IsDashingFinishing();
        }

        private bool IsDashingButtonDown()
        {
            return _dash.triggered;
        }

        private void LimitVerticalSpeed()
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x,
                Mathf.Clamp(rigidbody.velocity.y, verticalSpeedLimits.x,verticalSpeedLimits.y));
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                var enemy = other.gameObject.GetComponent<EnemyStats>();
                if(enemy.IsBubbled)
                    return;
                playerstats.TakeDamage(enemy.damage);
                deathState = playerstats.IsDead;
                if (!deathState)
                {
                    _receivingDamageDurationLeft = receiveDamageDuration;
                    var directionFromEnemy = (transform.position - other.transform.position);
                    directionFromEnemy = new Vector3(directionFromEnemy.x, 3);
                    directionFromEnemy.Normalize();
                    rigidbody.AddForce(directionFromEnemy*damageForce, ForceMode2D.Impulse);
                }
                else
                {
                    Destroy(gameObject);
                    GameManager.Instance.RestartLevel();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Bubble"))
            {
                var lootBubble = other.GetComponentInParent<LootBubble>();
                playerstats.ModifyOxygen(lootBubble.oxigenProvided);
                barraDeoxigeno.fillAmount += lootBubble.oxigenProvided / 100f;
                Destroy(other.gameObject);
            }
        }
    }
}
