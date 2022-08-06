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

            DecelerateY(_Player.Data.AirCounterY);

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
            var jumpForce = new Vector3(0f, _StateMachine.JumpForce, 0f);

            _Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion
    }
}
