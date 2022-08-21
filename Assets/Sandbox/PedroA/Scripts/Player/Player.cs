using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        public delegate void InteractInput();
        public static event InteractInput onInteractInput;

        [field: SerializeField] public PlayerSO Data { get; private set; }
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        [field: SerializeField] public Transform InputSpace { get; private set; }
        
        public Rigidbody Rigidbody { get; private set; }
        public PlayerInputHandler Input { get; private set; }
        public FloatingCapsule FloatingCapsule { get; private set; }

        private Animator _animator;
        private PlayerStateMachine _stateMachine;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
            Input = GetComponent<PlayerInputHandler>();
            FloatingCapsule = GetComponent<FloatingCapsule>();

            _stateMachine = new PlayerStateMachine(this);

            AnimationData.Initialize();
        }

        private void Start()
        {
            _stateMachine.ChangeState(_stateMachine.LocomotionState);
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.LogicUpdate();

            if (Input.PlayerActions.Interact.WasPressedThisFrame())
                onInteractInput?.Invoke();
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Data.GroundOverlapOffset, Data.GroundOverlapRadius);
        }

        public void SetAnimationBool(int paramHash, bool value)
        {
            _animator.SetBool(paramHash, value);
        }

        public void SetAnimationFloat(int paramHash, float value)
        {
            _animator.SetFloat(paramHash, value);
        }
    }
}
