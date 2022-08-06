using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class StateMachine
    {
        public IState CurrentState;

        public void ChangeState(IState newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }

        public void HandleInput()
        {
            CurrentState?.HandleInput();
        }

        public void LogicUpdate()
        {
            CurrentState?.LogicUpdate();
        }

        public void PhysicsUpdate()
        {
            CurrentState?.PhysicsUpdate();
        }

        public void EnterTrigger(Collider collider)
        {
            CurrentState?.EnterTrigger(collider);
        }

        public void ExitTrigger(Collider collider)
        {
            CurrentState?.ExitTrigger(collider);
        }
    }
}
