using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private PlayerFallData _fallData;

        private Vector3 _playerPositionOnEnter;

        public PlayerFallingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _fallData = AirborneData.FallData;
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.FallParameterHash);

            _playerPositionOnEnter = StateMachine.Player.transform.position;

            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            ResetVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.FallParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            LimitVerticalVelocity();
        }
        #endregion

        #region MainMethods
        private void LimitVerticalVelocity()
        {
            var playerVerticalVelocity = GetVerticalVelocity();
            if (playerVerticalVelocity.y >= -_fallData.FallSpeedLimit)
                return;
            
            var limitedVelocity = new Vector3(0f, -_fallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            StateMachine.Player.Rigidbody.AddForce(limitedVelocity, ForceMode.VelocityChange);
        }
        #endregion

        #region ReusableMethods
        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            var fallDistance = _playerPositionOnEnter.y - StateMachine.Player.transform.position.y;

            if (fallDistance < _fallData.MinimumDistanceToHardFall)
            {
                StateMachine.ChangeState(StateMachine.LightLandingState);
                return;
            }

            if (StateMachine.ReusableData.ShouldWalk && !StateMachine.ReusableData.ShouldSprint || StateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                StateMachine.ChangeState(StateMachine.HardLandingState);
                return;
            }

            StateMachine.ChangeState(StateMachine.RollingState);
        }
        #endregion
    }
}
