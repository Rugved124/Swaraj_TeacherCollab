using System;
using UnityEngine;

namespace Checks
{
    public class Ground : MonoBehaviour
    {
        private bool onGround;
        private float friction;

        void EvaluateCollision(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                Vector2 normal = collision.GetContact(i).normal;
                onGround |= normal.y >= 0.9f;
            }
        }

        void RetrieveFriction(Collision2D collision)
        {
            if (collision.rigidbody)
            {
                PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

                friction = 0;

                if (material != null)
                {
                    friction = material.friction;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            EvaluateCollision(col);
            RetrieveFriction(col);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
            RetrieveFriction(collision);
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            onGround = false;
            friction = 0;
        }

        public bool GetOnGround()
        {
            return onGround;
        }

        public float GetFriction()
        {
            return friction;
        }
    }
}
