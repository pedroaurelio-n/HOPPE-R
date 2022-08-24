using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyStateMachine : StateMachine
    {
        public BasicEnemy BasicEnemy { get; }

        public bool StopFollow { get; set; }

        public BasicEnemyIdleState IdleState { get; }
        public BasicEnemyWanderState WanderState { get; }
        public BasicEnemyFollowState FollowState { get; }

        public BasicEnemyStateMachine(BasicEnemy basicEnemy)
        {
            BasicEnemy = basicEnemy;

            IdleState = new BasicEnemyIdleState(this);
            WanderState = new BasicEnemyWanderState(this);
            FollowState = new BasicEnemyFollowState(this);
        }
    }
}
