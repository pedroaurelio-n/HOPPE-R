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
        public PlayerAnimationHelper AnimationHelper { get; private set; }

        private PlayerStateMachine _stateMachine;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Input = GetComponent<PlayerInputHandler>();
            FloatingCapsule = GetComponent<FloatingCapsule>();
            AnimationHelper = GetComponentInChildren<PlayerAnimationHelper>();

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + Data.GroundOverlapOffset, Data.GroundOverlapRadius);
        }
    }
}
