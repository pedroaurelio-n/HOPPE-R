using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerGroundedState : PlayerBaseState
    {
        public PlayerGroundedState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.SetAnimationBool(_Player.AnimationData.AirborneParamHash, false);
            _Player.SetAnimationBool(_Player.AnimationData.GroundedParamHash, true);

            _StateMachine.AdditionalJumps = _Player.Data.AdditionalJumps;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsThereGroundUnderneath())
                return;

            if (!CheckForGroundContactExited())
                _StateMachine.ChangeState(_StateMachine.FallState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            
            Float();

            if (_StateMachine.MovementInput == Vector2.zero && IsMovingHorizontally(Mathf.Epsilon))
                DecelerateXZ(_Player.Data.GroundNegAccel);
        }
        #endregion

        #region MainMethods
        private void Float()
        {
            var capsuleCenterWorldSpace = _Player.FloatingCapsule.Collider.bounds.center;
            var downwardsRayFromCenter = new Ray(capsuleCenterWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCenter, out RaycastHit hit, _Player.FloatingCapsule.FloatRayDistance, _Player.Data.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                var groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCenter.direction);

                var slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    _StateMachine.MovementInput = Vector2.zero;
                    var pushForce = hit.normal + Vector3.down;
                    _Player.Rigidbody.AddForce(pushForce * _Player.Data.SlideOffForce, ForceMode.Acceleration);
                    return;
                }
                
                var distanceFromFloatingPoint = _Player.FloatingCapsule.ColliderCenterLocal.y - hit.distance;

                if (distanceFromFloatingPoint == 0f)
                    return;

                var amountToLift = distanceFromFloatingPoint * _Player.FloatingCapsule.StepReachForce - GetVerticalVelocity().y;
                var liftForce = new Vector3(0f, amountToLift, 0f);

                _Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private bool CheckForGroundContactExited()
        {
            var capsuleCenterWorldSpace = _Player.FloatingCapsule.Collider.bounds.center;
            var downwardsRayFromCapsuleBottom = new Ray(capsuleCenterWorldSpace - _Player.FloatingCapsule.Collider.bounds.extents, Vector3.down);

            return Physics.Raycast(downwardsRayFromCapsuleBottom, out _, _Player.Data.GroundToFallRayDistance, _Player.Data.GroundLayer, QueryTriggerInteraction.Ignore);
        }
        #endregion

        #region InputMethods
        protected override void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
            if (_StateMachine.SlopeSpeedModifier == 0f)
                return;

            _StateMachine.ChangeState(_StateMachine.JumpState);
        }
        #endregion
    }
}
