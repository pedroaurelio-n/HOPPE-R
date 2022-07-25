using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.HardLandParameterHash);

            StateMachine.Player.Input.PlayerActions.Move.Disable();

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.HardLandParameterHash);
            
            StateMachine.Player.Input.PlayerActions.Move.Enable();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
                return;

            ResetVelocity();
        }

        public override void OnAnimationExitEvent()
        {
            StateMachine.Player.Input.PlayerActions.Move.Enable();
        }

        public override void OnAnimationTransitionEvent()
        {
            StateMachine.ChangeState(StateMachine.IdlingState);
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Move.started += OnmovementStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Move.started -= OnmovementStarted;
        }

        protected override void OnMove()
        {
            if (StateMachine.ReusableData.ShouldWalk)
                return;
            
            StateMachine.ChangeState(StateMachine.RunningState);
        }
        #endregion

        #region InputMethods
        protected override void OnJumpStarted(InputAction.CallbackContext ctx)
        {
        }

        private void OnmovementStarted(InputAction.CallbackContext ctx)
        {
            OnMove();
        }
        #endregion
    }
}
