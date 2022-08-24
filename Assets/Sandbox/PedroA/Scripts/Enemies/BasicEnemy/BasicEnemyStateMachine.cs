using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyStateMachine : StateMachine
    {
        public BasicEnemy BasicEnemy { get; }

        public BasicEnemyIdleState IdleState { get; }
        public BasicEnemyWanderState WanderState { get; }

        public BasicEnemyStateMachine(BasicEnemy basicEnemy)
        {
            BasicEnemy = basicEnemy;

            IdleState = new BasicEnemyIdleState(this);
            WanderState = new BasicEnemyWanderState(this);
        }
    }
}
