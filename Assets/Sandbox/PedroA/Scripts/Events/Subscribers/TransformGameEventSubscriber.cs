using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Tortoise.HOPPER
{
    public class TransformGameEventSubscriber : BaseGameEventSubscriber
    {
        [SerializeField] private Transform transformToAffect;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        [SerializeField] private Vector3 newPosition;
        [SerializeField] private bool isPositionRelative;
        [SerializeField] private Vector3 newRotation;
        [SerializeField] private bool isRotationRelative;
        [SerializeField] private Vector3 newScale;
        [SerializeField] private bool isScaleRelative;

        private Vector3 _newPosition;
        private Vector3 _newRotation;
        private Vector3 _newScale;

        protected override void InvokeEvent()
        {
            DefineTransformParams();

            if (_newPosition != Vector3.zero)
                transformToAffect.DOMove(_newPosition, duration).SetEase(ease);

            if (_newRotation != Vector3.zero)
                transformToAffect.DORotate(_newRotation, duration).SetEase(ease);

            if (_newScale != Vector3.zero)
                transformToAffect.DOScale(_newScale, duration).SetEase(ease);
                
            base.InvokeEvent();
        }

        private void DefineTransformParams()
        {
            if (isPositionRelative)
                _newPosition = transformToAffect.position + newPosition;
            else
                _newPosition = newPosition;

            if (isRotationRelative)
                _newRotation = transformToAffect.rotation.eulerAngles + newRotation;
            else
                _newRotation = newRotation;

            if (isScaleRelative)
                _newScale = transformToAffect.localScale + newScale;
            else
                _newScale = newScale;
        }
    }
}
