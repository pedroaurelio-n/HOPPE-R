using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : Entity
    {
        public delegate void InteractInput();
        public static event InteractInput onInteractInput;

        [field: SerializeField] public PlayerSO Data { get; private set; }
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        [field: SerializeField] public Transform InputSpace { get; private set; }
        
        public Rigidbody Rigidbody { get; private set; }
        public PlayerInputHandler Input { get; private set; }
        public FloatingCapsule FloatingCapsule { get; private set; }

        private PlayerStateMachine _playerStateMachine;

        protected override void Awake()
        {
            base.Awake();
            
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<PlayerInputHandler>();
            FloatingCapsule = GetComponent<FloatingCapsule>();

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

            if (Input.PlayerActions.Interact.WasPressedThisFrame())
                onInteractInput?.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Data.GroundCastOffset, Data.GroundCastRadius);
        }
    }
}
