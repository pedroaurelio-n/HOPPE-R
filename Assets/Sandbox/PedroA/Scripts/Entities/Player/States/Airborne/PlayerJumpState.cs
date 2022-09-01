using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerJumpState : PlayerAirborneState
    {
        private bool _canStartFalling;

        public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.JumpingParamHash, true);
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.FallingParamHash, false);

            Jump();
        }

        public override void Exit()
        {
            _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.JumpingParamHash, false);

            base.Exit();

            _canStartFalling = false;

            _Player.Input.PlayerActions.Move.Enable();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!_canStartFalling && IsMovingUp(0f))
            {
                _canStartFalling = true;
                _Player.AnimationHelper.SetAnimationBool(_Player.AnimationData.JumpingParamHash, false);
            }

            if (!IsMovingUp(1f) && _canStartFalling)
            {
                _Player.Input.PlayerActions.Move.Enable();
            }
            
            if (!_canStartFalling || GetVerticalVelocity().y > 0f)
                return;

            if (_Player.Input.PlayerActions.Jump.IsPressed() && _StateMachine.AdditionalJumps <= 0)
            {
                _StateMachine.ChangeState(_StateMachine.GlideState);
                return;
            }

            _StateMachine.ChangeState(_StateMachine.FallState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            ApplyLowJumpMultiplier();

            if (_StateMachine.MovementInput == Vector2.zero && IsMovingHorizontally(Mathf.Epsilon))
                DecelerateXZ(_Player.Data.AirNegAccel);
        }

        public override void EnterTrigger(Collider collider)
        {
            if (_canStartFalling && IsThereGroundUnderneath())
            {
                base.EnterTrigger(collider);
            }
        }
        #endregion

        #region MainMethods
        private void Jump()
        {
            var jumpDirection = Vector3.up;

            var capsuleCenterWorldSpace = _Player.FloatingCapsule.Collider.bounds.center;
            var downwardsRayFromCenter = new Ray(capsuleCenterWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCenter, out RaycastHit hit, _Player.Data.JumpAngleRayDistance, _Player.Data.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                var groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCenter.direction);

                var slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle, false);


                if (slopeSpeedModifier <= _Player.Data.UpJumpMaxSlopeValue)
                {
                    jumpDirection = (Vector3.up + hit.normal).normalized;

                    if (IsMovingUp(0f))
                    {
                        _Player.Input.PlayerActions.Move.Disable();
                        _StateMachine.MovementInput = Vector2.zero;
                        ResetVelocityY();
                    }
                }

                if (IsMovingDown())
                {
                    ResetVelocityY();
                }

                if (_StateMachine.IsOnStairs && IsMovingUp())
                {
                    ResetVelocityY();
                    jumpDirection *= 1.3f;
                }
            }

            _Player.Rigidbody.AddForce(jumpDirection * _StateMachine.JumpForce, ForceMode.VelocityChange);
        }

        private void ApplyLowJumpMultiplier()
        {
            var newAccel = Vector3.up * Physics.gravity.y * (_Player.Data.LowJumpMultiplier - 1);

            if (GetVerticalVelocity().y > 0 && !_Player.Input.PlayerActions.Jump.IsPressed())
                DecelerateY(newAccel);
        }
        #endregion
    }
}
