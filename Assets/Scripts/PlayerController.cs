    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Bubble
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        public int dashForce;
        public float dashingStopSpeed = 0.2f;
        private float moveInput;
        
        private Inputs inputs;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;
        public float fallingGravity = 0.5f;
        public float dashingDrag = 5;
        public Vector2 verticalSpeedLimits = new Vector2(-10, 10);

        private Rigidbody2D rigidbody;
        private Animator animator;
        private InputAction _move;
        private InputAction _jump;
        private InputAction _dash;
        private bool _dashing;

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
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (IsDashingButtonDown() && !_dashing)
            {
                StartDash();
            }

            if (CheckDashingFinished())
            {
                _dashing = false;
                rigidbody.drag = 0;
            }

            if (_move.IsPressed())
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

        private bool IsDashingFinishing()
        {
            return _dashing && Mathf.Abs(rigidbody.velocity.x) <= dashingStopSpeed;
        }

        private void StartDash()
        {
            rigidbody.AddForce(new Vector2((facingRight ? 1 : -1) * dashForce, 0), ForceMode2D.Impulse);
            rigidbody.drag = dashingDrag;
            _dashing = true;
        }

        private void CancelDash()
        {
            _dashing = false;
            rigidbody.velocity = StopXMovement();
            rigidbody.drag = 0;
        }

        private Vector2 StopXMovement() => new Vector2(0, rigidbody.velocity.y);

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
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                Destroy(other.gameObject);
            }
        }
    }
}
