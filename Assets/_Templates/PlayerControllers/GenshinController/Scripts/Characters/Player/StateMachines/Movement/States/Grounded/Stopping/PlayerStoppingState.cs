using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public PlayerStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            SetBaseCameraRecenteringData();

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.StoppingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.StoppingParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            RotateTowardsTargetRotation();

            if (!IsMovingHorizontally())
                return;

            DecelerateHorizontally();
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

            StateMachine.Player.Input.PlayerActions.Move.started += OnMovementStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Move.started -= OnMovementStarted;
        }
        #endregion

        #region InputMethods
        private void OnMovementStarted(InputAction.CallbackContext ctx)
        {
            OnMove();
        }
        #endregion
    }
}
