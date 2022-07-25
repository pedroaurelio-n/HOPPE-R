using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.AirborneParameterHash);

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.AirborneParameterHash);
        }
        #endregion

        #region ReusableMethods
        protected override void OnContactWithGround(Collider collider)
        {
            StateMachine.ChangeState(StateMachine.LightLandingState);
        }

        protected virtual void ResetSprintState()
        {
            StateMachine.ReusableData.ShouldSprint = false;
        }
        #endregion
    }
}
