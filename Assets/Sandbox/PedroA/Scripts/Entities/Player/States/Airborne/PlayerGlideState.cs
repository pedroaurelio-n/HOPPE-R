using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerGlideState : PlayerAirborneState
    {
        public PlayerGlideState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();
            
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.GlidingParamHash, true);
        }

        public override void Exit()
        {
            base.Exit();

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.GlidingParamHash, false);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (_StateMachine.MovementInput == Vector2.zero && IsMovingHorizontally(Mathf.Epsilon))
                DecelerateXZ(_Player.Data.AirNegAccel);

            LimitFallVelocity();
        }
        #endregion

        #region MainMethods
        private void LimitFallVelocity()
        {
            var verticalVelocity = GetVerticalVelocity();

            if (verticalVelocity.y > -_Player.Data.GlideMaxVelocity)
            {
                return;
            }

            var limitVelocity = new Vector3(0f, -_Player.Data.GlideMaxVelocity - verticalVelocity.y, 0f);

            _Player.Rigidbody.AddForce(limitVelocity, ForceMode.VelocityChange);
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputCallbacks()
        {
            base.AddInputCallbacks();

            _Player.Input.PlayerActions.Jump.canceled += OnGlideCanceled;
        }

        protected override void RemoveInputCallbacks()
        {
            base.RemoveInputCallbacks();

            _Player.Input.PlayerActions.Jump.canceled -= OnGlideCanceled;
        }
        #endregion

        #region InputMethods
        private void OnGlideCanceled(InputAction.CallbackContext ctx)
        {
            _StateMachine.ChangeState(_StateMachine.FallState);
        }
        #endregion
    }
}
