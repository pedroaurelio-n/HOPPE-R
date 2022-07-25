using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        private PlayerIdleData _idleData;

        public PlayerIdlingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _idleData = MovementData.IdleData;
        }

        #region IState Methods
        public override void Enter()
        {
            StateMachine.ReusableData.MovementSpeedModifier = 0f;

            StateMachine.ReusableData.BackwardsCameraRecenteringData = _idleData.BackwardsCameraRecenteringData;

            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.IdleParameterHash);

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.IdleParameterHash);
        }

        public override void Update()
        {
            base.Update();

            if (StateMachine.ReusableData.MovementInput == Vector2.zero)
                return;
            
            OnMove();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
                return;

            ResetVelocity();
        }
        #endregion
    }
}
