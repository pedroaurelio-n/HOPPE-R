using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyAnimationHelper : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        [SerializeField] private BasicEnemy basicEnemy;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        
        private void OnAnimatorMove()
        {
            basicEnemy.transform.position += Animator.deltaPosition;
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

            basicEnemy.OnAnimationEnterEvent();
        }

        public void TriggerAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }
                
            basicEnemy.OnAnimationExitEvent();
        }

        public void TriggerAnimationTransitionEvent()
        {
            basicEnemy.OnAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return Animator.IsInTransition(layerIndex);
        }
    }
}
