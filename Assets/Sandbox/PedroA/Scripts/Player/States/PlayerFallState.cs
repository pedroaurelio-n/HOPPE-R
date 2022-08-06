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
                DecelerateXZ(_Player.AirNegAccel);

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
        private void LimitFallVelocity()
        {
            var verticalVelocity = GetVerticalVelocity();

            if (verticalVelocity.y > -_Player.FallMaxVelocity)
            {
                return;
            }

            var limitVelocity = new Vector3(0f, -_Player.FallMaxVelocity - verticalVelocity.y, 0f);

            _Player.Rigidbody.AddForce(limitVelocity, ForceMode.VelocityChange);
        }
        #endregion
    }
}
