using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Tortoise.HOPPER
{
    public class BasicEnemy : MonoBehaviour
    {
        [field: SerializeField] public BasicEnemySO Data { get; private set; }
        [field: SerializeField] public BoxCollider MoveArea { get; private set; }
        [field: SerializeField] public Transform Target { get; private set; }

        public NavMeshAgent Agent { get; private set; }

        private BasicEnemyStateMachine _stateMachine;

        private void Awake()
        {
            Agent = GetComponent<NavMeshAgent>();

            _stateMachine = new BasicEnemyStateMachine(this);
        }

        private void Start()
        {
            _stateMachine.ChangeState(_stateMachine.IdleState);
        }

        private void Update()
        {
            _stateMachine.LogicUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsUpdate();
        }

        public void EnterTrigger(Collider collider)
        {
            _stateMachine.EnterTrigger(collider);
        }

        public void ExitTrigger(Collider collider)
        {
            _stateMachine.ExitTrigger(collider);
        }

        public void OnAnimationEnterEvent()
        {
            _stateMachine.AnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            _stateMachine.AnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            _stateMachine.AnimationTransitionEvent();
        }

    }
}
