using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GenshinController
{
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        [field: Header("References")]
        [field: SerializeField] public PlayerSO Data { get; private set; }

        [field: Header("Collisions")]
        [field: SerializeField] public PlayerCapsuleColliderUtility ColliderUtility { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }

        [field: Header("Camera")]
        [field:SerializeField] public PlayerCameraUtility CameraUtility { get; private set; }

        [field: Header("Animations")]
        [field:SerializeField] public PlayerAnimationData AnimationData { get; private set; }

        public PlayerInputHandler Input { get; private set; } 
        public Rigidbody Rigidbody { get; private set; } 
        public Animator Animator { get; private set; } 

        public Transform MainCameraTransform { get; private set; } 

        private PlayerMovementStateMachine _movementStateMachine;

        private void Awake()
        {
            Input = GetComponent<PlayerInputHandler>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();

            CameraUtility.Initialize();

            AnimationData.Initialize();

            MainCameraTransform = Camera.main.transform;

            _movementStateMachine = new PlayerMovementStateMachine(this);
        }

        private void OnValidate()
        {
            ColliderUtility.Initialize(gameObject);
            ColliderUtility.CalculateCapsuleColliderDimensions();
        }

        private void Start()
        {
            _movementStateMachine.ChangeState(_movementStateMachine.IdlingState);
        }

        private void OnTriggerEnter(Collider collider)
        {
            _movementStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            _movementStateMachine.OnTriggerExit(collider);
        }

        private void Update()
        {
            _movementStateMachine.HandleInput();

            _movementStateMachine.Update();
        }

        private void FixedUpdate()
        {
            _movementStateMachine.PhysicsUpdate();
        }

        public void OnMovementStateAnimationEnterEvent()
        {
            _movementStateMachine.OnAnimationEnterEvent();
        }

        public void OnMovementStateAnimationExitEvent()
        {
            _movementStateMachine.OnAnimationExitEvent();
        }

        public void OnMovementStateAnimationTransitionEvent()
        {
            _movementStateMachine.OnAnimationTransitionEvent();
        }
    }
}
