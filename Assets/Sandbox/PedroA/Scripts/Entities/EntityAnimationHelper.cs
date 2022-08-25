using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class EntityAnimationHelper : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        [SerializeField] private Entity entity;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        
        private void OnAnimatorMove()
        {
            entity.transform.position += Animator.deltaPosition;
        }

        public void SetRootMotion(bool value)
        {
            Animator.applyRootMotion = value;
        }

        public void SetAnimationBool(int paramHash, bool value)
        {
            Animator.SetBool(paramHash, value);
        }

        public void SetAnimationFloat(int paramHash, float value)
        {
            Animator.SetFloat(paramHash, value);
        }

        public void SetAnimationInt(int paramHash, int value)
        {
            Animator.SetInteger(paramHash, value);
        }

        public void TriggerAnimationEnterEvent()
        {
            if (IsInAnimationTransition())
                return;

            entity.OnAnimationEnterEvent();
        }

        public void TriggerAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }
                
            entity.OnAnimationExitEvent();
        }

        public void TriggerAnimationTransitionEvent()
        {
            entity.OnAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return Animator.IsInTransition(layerIndex);
        }
    }
}
