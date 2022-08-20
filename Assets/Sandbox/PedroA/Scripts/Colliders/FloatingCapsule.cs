using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class FloatingCapsule : MonoBehaviour
    {
        [SerializeField] private float height = 1.8f;
        [SerializeField] private float centerY = 0.9f;
        [SerializeField] private float radius = 0.25f;
        [SerializeField] [Range(0f, 1f)] private float stepHeightPercentage = 0.25f;
        [field: SerializeField] [field: Range(0f, 5f)] public float FloatRayDistance { get; private set; } = 2f;
        [field: SerializeField] [field: Range(0f, 50f)] public float StepReachForce { get; private set; } = 25f;

        public CapsuleCollider Collider { get; private set; }
        public Vector3 ColliderCenterLocal { get; private set; }

        private void Awake()
        {
            Initialize();
            CalculateCapsuleColliderDimensions();
        }

        private void OnValidate()
        {
            Initialize();
            CalculateCapsuleColliderDimensions();
        }

        public void Initialize()
        {
            if (Collider != null)
                return;
         
            Collider = GetComponent<CapsuleCollider>();

            ColliderCenterLocal = Collider.center;
        }
        
        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(radius);
            SetCapsuleColliderHeight(height * (1f - stepHeightPercentage));

            RecalculateCapsuleColliderCenter();

            var halfHeight = Collider.height * 0.5f;
            if (halfHeight < Collider.radius)
            {
                SetCapsuleColliderRadius(halfHeight);
            }
        }

        public void SetCapsuleColliderRadius(float newRadius)
        {
            Collider.radius = newRadius;
        }

        public void SetCapsuleColliderHeight(float newHeight)
        {
            Collider.height = newHeight;
        }

        public void RecalculateCapsuleColliderCenter()
        {
            var heightDifference = height - Collider.height;
            var newCenter = new Vector3(0f, centerY + (heightDifference * 0.5f), 0f);

            Collider.center = newCenter;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position + ColliderCenterLocal, Vector3.down * FloatRayDistance);
        }
    }
}
