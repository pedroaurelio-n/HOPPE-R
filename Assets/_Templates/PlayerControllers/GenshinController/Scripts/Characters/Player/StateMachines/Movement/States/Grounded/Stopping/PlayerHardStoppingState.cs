using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }
        
        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.HardStopParameterHash);

            StateMachine.ReusableData.MovementDecelerationForce = MovementData.StopData.HardDecelerationForce;

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.HardStopParameterHash);
        }
        #endregion

        #region ReusableMethods
        protected override void OnMove()
        {
            if (StateMachine.ReusableData.ShouldWalk)
                return;
            
            StateMachine.ChangeState(StateMachine.RunningState);
        }
        #endregion
    }
}
