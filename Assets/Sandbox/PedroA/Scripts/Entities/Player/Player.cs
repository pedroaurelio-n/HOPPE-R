using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : Entity
    {
        [field: SerializeField] public PlayerSO Data { get; private set; }
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        [field: SerializeField] public Transform InputSpace { get; private set; }
        
        public Rigidbody Rigidbody { get; private set; }
        public PlayerInputHandler Input { get; private set; }
        public FloatingCapsule FloatingCapsule { get; private set; }
        public Shield Shield { get; private set; }

        private PlayerStateMachine _playerStateMachine;

        protected override void Awake()
        {
            base.Awake();
            
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<PlayerInputHandler>();
            FloatingCapsule = GetComponent<FloatingCapsule>();
            Shield = GetComponent<Shield>();

            _playerStateMachine = new PlayerStateMachine(this);
            _stateMachine = _playerStateMachine;

            AnimationData.Initialize();
        }

        private void Start()
        {
            _playerStateMachine.ChangeState(_playerStateMachine.LocomotionState);
        }

        protected override void Update()
        {
            _playerStateMachine.HandleInput();
            _playerStateMachine.LogicUpdate();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Data.GroundCastOffset, Data.GroundCastRadius);
        }
    }
}
