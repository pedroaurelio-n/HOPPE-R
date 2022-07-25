using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerRollingState : PlayerLandingState
    {
        private PlayerRollData _rollData;

        public PlayerRollingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _rollData = MovementData.RollData;
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = _rollData.SpeedModifier;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.RollParameterHash);

            StateMachine.ReusableData.ShouldSprint = false;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.RollParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (StateMachine.ReusableData.MovementInput != Vector2.zero)
                return;
            
            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.MediumStoppingState);
                return;
            }

            OnMove();
        }
        #endregion

        #region InputMethods
        protected override void OnJumpStarted(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}
