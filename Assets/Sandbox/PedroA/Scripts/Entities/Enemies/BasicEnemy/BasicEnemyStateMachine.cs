using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicEnemyStateMachine : StateMachine
    {
        public BasicEnemy BasicEnemy { get; }

        public bool StopFollow { get; set; }
        public float CurrentCooldown { get; set; }

        public BasicEnemyIdleState IdleState { get; }
        public BasicEnemyWanderState WanderState { get; }
        public BasicEnemyFollowState FollowState { get; }
        public BasicEnemyAttackState AttackState { get; }
        public BasicEnemyDamageState DamageState { get; }
        public BasicEnemyDisabledState DisabledState { get; }

        public BasicEnemyStateMachine(BasicEnemy basicEnemy)
        {
            BasicEnemy = basicEnemy;

            IdleState = new BasicEnemyIdleState(this);
            WanderState = new BasicEnemyWanderState(this);
            FollowState = new BasicEnemyFollowState(this);
            AttackState = new BasicEnemyAttackState(this);
            DamageState = new BasicEnemyDamageState(this);
            DisabledState = new BasicEnemyDisabledState(this);
        }
    }
}
