using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerAttack3State : PlayerAttackState
    {
        public PlayerAttack3State(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        #region IStateMethods
        public override void Enter()
        {
            _ComboStep = 3;

            base.Enter();
        }
        #endregion
    }
}
