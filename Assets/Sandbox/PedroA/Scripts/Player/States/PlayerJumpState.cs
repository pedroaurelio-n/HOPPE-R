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
            //_StateMachine.SpeedModifier = 1f;

            base.Enter();

            _Player.SetAnimationBool(_Player.AnimationData.JumpingParamHash, true);

            Jump();
        }

        public override void Exit()
        {
            _Player.SetAnimationBool(_Player.AnimationData.JumpingParamHash, false);

            base.Exit();

            _canStartFalling = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // Debug.Log(_StateMachine.SpeedModifier);
            // _StateMachine.SpeedModifier = Mathf.MoveTowards(_StateMachine.SpeedModifier, 1f, _Player.AirNegAccel * Time.deltaTime);

            if (!_canStartFalling && IsMovingUp(0f))
            {
                _canStartFalling = true;
                _Player.SetAnimationBool(_Player.AnimationData.JumpingParamHash, false);
            }
            
            if (!_canStartFalling || GetVerticalVelocity().y > 0)
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
            // DecelerateY(_Player.Data.AirCounterY);

            if (_StateMachine.MovementInput == Vector2.zero && IsMovingHorizontally(Mathf.Epsilon))
                DecelerateXZ(_Player.Data.AirNegAccel);
        }

        public override void EnterTrigger(Collider collider)
        {
            base.EnterTrigger(collider);

            if (_canStartFalling && IsThereGroundUnderneath())
                _StateMachine.ChangeState(_StateMachine.LocomotionState);
        }
        #endregion

        #region MainMethods
        private void Jump()
        {
            var jumpDirection = Vector3.up;

            // var capsuleCenterWorldSpace = _Player.FloatingCapsule.Collider.bounds.center;
            // var downwardsRayFromCenter = new Ray(capsuleCenterWorldSpace, Vector3.down);

            // if (Physics.Raycast(downwardsRayFromCenter, out RaycastHit hit, _Player.FloatingCapsule.FloatRayDistance, _Player.Data.GroundLayer, QueryTriggerInteraction.Ignore))
            // {
            //     var groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCenter.direction);

            //     var slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle, false);


            //     if (slopeSpeedModifier <= _Player.Data.UpJumpMaxSlopeValue)
            //     {
            //         jumpDirection = (hit.normal + Vector3.up).normalized;
            //         // jumpForce += hit.normal;
            //     }
            // }

            Debug.DrawRay(_Player.transform.position, jumpDirection * _StateMachine.JumpForce, Color.cyan, 5f);
            _Player.Rigidbody.AddForce(jumpDirection * _StateMachine.JumpForce, ForceMode.VelocityChange);
        }

        private void ApplyLowJumpMultiplier()
        {
            var newAccel = Vector3.up * Physics.gravity.y * (_Player.Data.LowJumpMultiplier - 1);

            if (GetVerticalVelocity().y > 0 && !_Player.Input.PlayerActions.Jump.IsPressed())
                _Player.Rigidbody.AddForce(newAccel, ForceMode.Acceleration);
        }
        #endregion
    }
}
