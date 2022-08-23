using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerAnimationHelper : MonoBehaviour
    {
        public Animator Animator { get; private set; }
        [SerializeField] private Player player;

        private void Awake()
        {
            Animator = GetComponent<Animator>();
        }
        
        private void OnAnimatorMove()
        {
            player.transform.position += Animator.deltaPosition;
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

            player.OnAnimationEnterEvent();
        }

        public void TriggerAnimationExitEvent()
        {
            if (IsInAnimationTransition())
            {
                return;
            }
                
            player.OnAnimationExitEvent();
        }

        public void TriggerAnimationTransitionEvent()
        {
            player.OnAnimationTransitionEvent();
        }

        private bool IsInAnimationTransition(int layerIndex = 0)
        {
            return Animator.IsInTransition(layerIndex);
        }
    }
}
