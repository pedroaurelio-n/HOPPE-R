using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public interface IState
    {
        public void Enter();
        public void Exit();
        public void HandleInput();
        public void LogicUpdate();
        public void PhysicsUpdate();
        public void EnterTrigger(Collider collider);
        public void ExitTrigger(Collider collider);
        public void AnimationEnterEvent();
        public void AnimationExitEvent();
        public void AnimationTransitionEvent();
    }
}
