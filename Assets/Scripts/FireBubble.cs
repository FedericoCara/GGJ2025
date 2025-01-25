using System;
using UnityEngine;

namespace BubbleNS
{
    public class FireBubble : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;
        public float basicForce = 1;
        public float mediumForce = 5;
        public float maxForce = 50;

        private void Awake()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(bool right, int intensity = 0)
        {
            _rigidBody.AddForce((right? Vector2.right : Vector2.left)
                                * - GetIntensityForce(0),
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
            if (intensity == 1)
            {
                return mediumForce;
            }

            return maxForce;
        }
    }
}