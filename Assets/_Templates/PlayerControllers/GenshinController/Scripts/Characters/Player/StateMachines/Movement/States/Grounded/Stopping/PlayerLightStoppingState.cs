using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StateMachine.ReusableData.MovementDecelerationForce = MovementData.StopData.LightDecelerationForce;

            StateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }
        #endregion
    }
}
