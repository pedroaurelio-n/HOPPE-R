using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BasicEnemy : Entity
    {
        [field: SerializeField] public BasicEnemySO Data { get; private set; }
        [field: SerializeField] public BasicEnemyAnimationData AnimationData { get; private set; }
        [field: SerializeField] public BoxCollider MoveArea { get; private set; }
        [field: SerializeField] public Transform Target { get; private set; }

        public NavMeshAgent Agent { get; private set; }

        private BasicEnemyStateMachine _enemyStateMachine;

        protected override void Awake()
        {
            base.Awake();

            Agent = GetComponent<NavMeshAgent>();

            _enemyStateMachine = new BasicEnemyStateMachine(this);
            _stateMachine = _enemyStateMachine;

            AnimationData.Initialize();
        }

        private void Start()
        {
            Agent.speed = Data.MoveSpeed;
            Agent.angularSpeed = Data.RotationSpeed;
            Agent.acceleration = Data.Acceleration;
            Agent.stoppingDistance = Data.StoppingDistance;

            _enemyStateMachine.ChangeState(_enemyStateMachine.IdleState);
        }
    }
}
