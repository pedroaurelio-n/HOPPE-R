using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [Serializable]
    public class PlayerAnimationData
    {
        public int SpeedParamHash { get; private set; }
        public int GroundedParamHash { get; private set; }
        public int AirborneParamHash { get; private set; }
        public int SprintingParamHash { get; private set; }
        public int JumpingParamHash { get; private set; }
        public int FallingParamHash { get; private set; }
        public int GlidingParamHash { get; private set; }

        [Header("Animation Parameters")]
        [field: SerializeField] private string SpeedParam = "Speed";
        [field: SerializeField] private string GroundedParam = "IsGrounded";
        [field: SerializeField] private string AirborneParam = "IsAirborne";
        [field: SerializeField] private string SprintingParam = "IsSprinting";
        [field: SerializeField] private string JumpingParam = "IsJumping";
        [field: SerializeField] private string FallingParam = "IsFalling";
        [field: SerializeField] private string GlidingParam = "IsGliding";

        public void Initialize()
        {
            SpeedParamHash = Animator.StringToHash(SpeedParam);
            GroundedParamHash = Animator.StringToHash(GroundedParam);
            AirborneParamHash = Animator.StringToHash(AirborneParam);
            SprintingParamHash = Animator.StringToHash(SprintingParam);
            JumpingParamHash = Animator.StringToHash(JumpingParam);
            FallingParamHash = Animator.StringToHash(FallingParam);
            GlidingParamHash = Animator.StringToHash(GlidingParam);
        }
    }
}
