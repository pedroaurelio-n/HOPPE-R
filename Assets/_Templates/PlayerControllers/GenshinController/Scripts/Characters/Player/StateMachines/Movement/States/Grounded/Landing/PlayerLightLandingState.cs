using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        public PlayerLightLandingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;

            ResetVelocity();
        }

        public override void Update()
        {
            base.Update();

            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
                return;
            
            OnMove();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
                return;

            ResetVelocity();
        }

        public override void OnAnimationTransitionEvent()
        {
            StateMachine.ChangeState(StateMachine.IdlingState);
        }
        #endregion
    }
}
