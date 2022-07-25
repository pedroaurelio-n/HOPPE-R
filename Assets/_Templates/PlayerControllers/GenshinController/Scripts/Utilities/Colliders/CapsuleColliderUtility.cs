using System;
using UnityEngine;

namespace GenshinController
{
    [Serializable]
    public class CapsuleColliderUtility
    {
        public CapsuleColliderData CapsuleColliderData { get; private set; }
        [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData SlopeData { get; private set; }

        public void Initialize(GameObject gameObject)
        {
            if (CapsuleColliderData != null)
                return;
                
            CapsuleColliderData = new CapsuleColliderData();

            CapsuleColliderData.Initialize(gameObject);

            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
        }

        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentage));

            RecalculateCapsuleColliderCenter();

            var halfColliderHeight = CapsuleColliderData.Collider.height * 0.5f;

            if (halfColliderHeight < CapsuleColliderData.Collider.radius)
                SetCapsuleColliderRadius(halfColliderHeight);

            CapsuleColliderData.UpdateColliderData();
        }

        private void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.Collider.radius = radius;
        }

        private void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.Collider.height = height;
        }

        private void RecalculateCapsuleColliderCenter()
        {
            var colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.Collider.height;

            var newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference * 0.5f), 0f);

            CapsuleColliderData.Collider.center = newColliderCenter;
        }
    }
}
