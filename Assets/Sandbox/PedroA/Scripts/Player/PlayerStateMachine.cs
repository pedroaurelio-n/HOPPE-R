using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerStateMachine : StateMachine
    {
        public float JumpForce { get; }
        public Vector2 MovementInput { get; set; }
        public float MoveAmount { get; set; }
        public float SpeedModifier { get; set; }
        public float SlopeSpeedModifier { get; set; }
        public int AdditionalJumps { get; set; }

        public Player Player { get; }

        public PlayerLocomotionState LocomotionState { get; }
        public PlayerJumpState JumpState { get; }
        public PlayerFallState FallState { get; }
        public PlayerGlideState GlideState { get; }
        public PlayerSprintState SprintState { get; }

        public PlayerStateMachine(Player player)
        {
            Player = player;

            LocomotionState = new PlayerLocomotionState(this);
            JumpState = new PlayerJumpState(this);
            FallState = new PlayerFallState(this);
            GlideState = new PlayerGlideState(this);
            SprintState = new PlayerSprintState(this);

            var height = Player.Data.JumpHeight - 1f;
            JumpForce = Mathf.Sqrt(-2f * height * Physics.gravity.y);
        }
    }
}
