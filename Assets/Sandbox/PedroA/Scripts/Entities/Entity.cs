using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public abstract class Entity : MonoBehaviour
    {
        public EntityAnimationHelper AnimationHelper { get; private set; }

        protected StateMachine _stateMachine;

        protected virtual void Awake()
        {
            AnimationHelper = GetComponentInChildren<EntityAnimationHelper>();
        }

        protected virtual void Update()
        {
            _stateMachine.LogicUpdate();
        }

        protected virtual void FixedUpdate()
        {
            _stateMachine.PhysicsUpdate();
        }

        public void EnterTrigger(Collider collider)
        {
            _stateMachine.EnterTrigger(collider);
        }

        public void ExitTrigger(Collider collider)
        {
            _stateMachine.ExitTrigger(collider);
        }

        public void OnAnimationEnterEvent()
        {
            _stateMachine.AnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            _stateMachine.AnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            _stateMachine.AnimationTransitionEvent();
        }
    }
}
