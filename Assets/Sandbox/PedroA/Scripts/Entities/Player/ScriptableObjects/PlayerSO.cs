using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [CreateAssetMenu(fileName = "New Player", menuName = "Characters/Player")]
    public class PlayerSO : ScriptableObject
    {
        [field: Header("Speeds Params")]
        [field: SerializeField] [field: Range(0f, 15f)] public float BaseSpeed { get; private set; }
        [field: SerializeField] [field: Range(0f, 30f)] public float RotationSpeed { get; private set; }
        [field: SerializeField] [field: Range(1f, 3f)] public float SprintSpeedModifier { get; private set; }
        [field: SerializeField] public AnimationCurve SlopeSpeedAngles { get; private set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float SprintMaxSlopeValue { get; private set; }

        [field: Header("Accel/Force Params")]
        [field: SerializeField] [field: Range(0f, 20f)] public float PosAccel { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float GroundNegAccel { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float AirNegAccel { get; private set; }
        [field: SerializeField] [field: Range(0f, 5f)] public float AirCounterSprint { get; private set; }
        [field: SerializeField] [field: Range(0f, 50f)] public float SlideOffForce { get; private set; }

        [field: Header("Ground Checks Params")]
        [field: SerializeField] public LayerMask GroundLayer { get; private set; }
        [field: SerializeField] public LayerMask StairLayer { get; private set; }
        [field: SerializeField] public Vector3 GroundOverlapOffset { get; private set; }
        [field: SerializeField] public float GroundOverlapRadius { get; private set; }
        [field: SerializeField] public float GroundToFallRayDistance { get; private set; }

        [field: Header("Airborne Params")]
        [field: SerializeField] [field: Range(0f, 10f)] public float JumpHeight { get; private set; }
        [field: SerializeField] [field: Range(0, 5)] public int AdditionalJumps { get; private set; }
        [field: SerializeField] public float JumpAngleRayDistance { get; private set; }
        [field: SerializeField] [field: Range(0f, 1f)] public float UpJumpMaxSlopeValue { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float FallMultiplier { get; private set; }
        [field: SerializeField] [field: Range(0f, 20f)] public float LowJumpMultiplier { get; private set; }
        [field: SerializeField] [field: Range(1f, 15f)] public float FallMaxVelocity { get; private set; }
        [field: SerializeField] [field: Range(1f, 15f)] public float GlideMaxVelocity { get; private set; }
    }
}
