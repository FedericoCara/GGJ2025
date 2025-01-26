using System;
using UnityEngine;

namespace BubbleNS
{
    public class FireBubble : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;
        public float basicForce = 1;
        public float mediumForce = 5;
        public int damage = 1;
        
        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(bool right, int intensity = 0)
        {
            _rigidBody.AddForce((right? Vector2.right : Vector2.left)
                                * - GetIntensityForce(intensity)
                                +(intensity==0? Vector2.down : Vector2.zero),
                ForceMode2D.Impulse);
            if (intensity > 0)
            {
                _rigidBody.gravityScale = 0;
            }
        }

        private float GetIntensityForce(int intensity)
        {
            if (intensity <= 0)
            {
                return basicForce;
            }
            
            return mediumForce;
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            EnemyStats enemy;
            if((enemy = col.collider.GetComponent<EnemyStats>())!=null){
                enemy.TakeDamage(damage);
            }
            if(col.collider.tag=="Player")
                return;
            Destroy(gameObject);
            
        }
    }
}