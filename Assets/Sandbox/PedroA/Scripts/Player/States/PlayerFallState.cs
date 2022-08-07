using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerFallState : PlayerAirborneState
    {
        public PlayerFallState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.SetAnimationBool(_Player.AnimationData.FallingParamHash, true);
        }

        public override void Exit()
        {
            base.Exit();
            
            _Player.SetAnimationBool(_Player.AnimationData.FallingParamHash, false);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (_StateMachine.MovementInput == Vector2.zero && IsMovingHorizontally(Mathf.Epsilon))
                DecelerateXZ(_Player.Data.AirNegAccel);

            IncreaseFallAccel();

            LimitFallVelocity();
        }

        public override void EnterTrigger(Collider collider)
        {
            base.EnterTrigger(collider);

            if (_Player.Input.PlayerActions.Sprint.IsPressed())
            {
                _StateMachine.ChangeState(_StateMachine.SprintState);
                return;
            }
            _StateMachine.ChangeState(_StateMachine.LocomotionState);
        }
        #endregion

        #region MainMethods
        private void IncreaseFallAccel()
        {
            var newAccel = Vector3.up * Physics.gravity.y * (_Player.Data.FallMultiplier - 1);
            _Player.Rigidbody.AddForce(newAccel, ForceMode.Acceleration);
        }

        private void LimitFallVelocity()
        {
            var verticalVelocity = GetVerticalVelocity();

            if (verticalVelocity.y > -_Player.Data.FallMaxVelocity)
            {
                return;
            }

            var limitVelocity = new Vector3(0f, -_Player.Data.FallMaxVelocity - verticalVelocity.y, 0f);

            _Player.Rigidbody.AddForce(limitVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
