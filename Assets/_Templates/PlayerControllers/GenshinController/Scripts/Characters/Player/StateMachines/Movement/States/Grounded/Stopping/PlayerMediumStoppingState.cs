using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.MediumStopParameterHash);

            StateMachine.ReusableData.MovementDecelerationForce = MovementData.StopData.MediumDecelerationForce;

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.MediumStopParameterHash);
        }
        #endregion
    }
}
