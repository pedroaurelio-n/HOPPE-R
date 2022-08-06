using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PlayerInputHandler))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public PlayerAnimationData AnimationData { get; private set; }
        [field: SerializeField] public Transform InputSpace { get; private set; }
        [field: SerializeField] [field: Range(0f, 15f)] public float BaseSpeed { get; private set; }
        [field: SerializeField] [field: Range(0f, 30f)] public float RotationSpeed { get; private set; }
        [field: SerializeField] [field: Range(1f, 3f)] public float SprintModifier { get; private set; }
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float GroundNegAccel { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float AirNegAccel { get; private set; }
        [field: SerializeField] [field: Range(0f, 5f)] public float AirCounterSprint { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float AirCounterY { get; private set; }
        [field: SerializeField] [field: Range(0f, 50f)] public float SlideOffForce { get; private set; }
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField] public Vector3 GroundOverlapOffset { get; private set; }
        [field: SerializeField] public float GroundOverlapRadius { get; private set; }
        [field: SerializeField] public float GroundToFallRayDistance { get; private set; }
        [field: SerializeField] [field: Range(0f, 10f)] public float JumpHeight { get; private set; }
        [field: SerializeField] [field: Range(0, 5)] public int AdditionalJumps { get; private set; }
        [field: SerializeField] [field: Range(1f, 15f)] public float FallMaxVelocity { get; private set; }
        [field: SerializeField] [field: Range(1f, 15f)] public float GlideMaxVelocity { get; private set; }
        
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
            Gizmos.DrawWireSphere(transform.position + GroundOverlapOffset, GroundOverlapRadius);
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
