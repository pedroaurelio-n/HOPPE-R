using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerGroundedState : PlayerMovementState
    {
        private SlopeData _slopeData;

        public PlayerGroundedState(PlayerMovementStateMachine stateMachine) : base(stateMachine)
        {
            _slopeData = stateMachine.Player.ColliderUtility.SlopeData;
        }

        #region IStateMethods
        public override void Enter()
        {
            base.Enter();

            StartAnimation(StateMachine.Player.AnimationData.GroundedParameterHash);

            UpdateShouldSprintState();

            UpdateCameraRecenteringState(StateMachine.ReusableData.MovementInput);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(StateMachine.Player.AnimationData.GroundedParameterHash);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            FloatCapsule();
        }
        #endregion

        #region MainMethods
        private void FloatCapsule()
        {
            var capsuleColliderCenterInWorldSpace = StateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            var downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, _slopeData.FloatRayDistance, 
                                StateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                var groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                var slopeSpeedMofifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedMofifier == 0f)
                    return;

                var distanceToFloatingPoint = StateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderCenterInLocalSpace.y 
                                                * StateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                    return;

                var amountToLift = distanceToFloatingPoint * _slopeData.StepReachForce - GetVerticalVelocity().y;
                var liftForce = new Vector3 (0f, amountToLift, 0f);

                StateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            var slopeSpeedModifier = MovementData.SlopeSpeedAngles.Evaluate(angle);

            if (StateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                StateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;

                UpdateCameraRecenteringState(StateMachine.ReusableData.MovementInput);
            }

            return slopeSpeedModifier;
        }

        private void UpdateShouldSprintState()
        {
            if (!StateMachine.ReusableData.ShouldSprint)
                return;
            
            if (StateMachine.ReusableData.MovementInput != Vector2.zero)
                return;

            StateMachine.ReusableData.ShouldSprint = false;
        }

        private bool IsThereGroundUnderneath()
        {
            var groundCheckCollider = StateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckCollider;
            var groundColliderCenterInWorldSpace = groundCheckCollider.bounds.center;

            var overlappedGroundColliders = Physics.OverlapBox
            (
                groundColliderCenterInWorldSpace, 
                StateMachine.Player.ColliderUtility.TriggerColliderData.GroundCheckColliderExtents, 
                groundCheckCollider.transform.rotation, 
                StateMachine.Player.LayerData.GroundLayer, 
                QueryTriggerInteraction.Ignore
            );
            
            return overlappedGroundColliders.Length > 0;
        }
        #endregion

        #region ReusableMethods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;
            StateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            StateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;
            StateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }
        
        protected virtual void OnMove()
        {
            if (StateMachine.ReusableData.ShouldSprint)
            {
                StateMachine.ChangeState(StateMachine.SprintingState);
                return;
            }
            if (StateMachine.ReusableData.ShouldWalk)
            {
                StateMachine.ChangeState(StateMachine.WalkingState);
                return;
            }

            StateMachine.ChangeState(StateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            base.OnContactWithGroundExited(collider);

            if (IsThereGroundUnderneath())
                return;

            var capsuleColliderCenterInWorldSpace = StateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;
            var downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace - StateMachine.Player.ColliderUtility.CapsuleColliderData.ColliderVerticalExtents, 
                                                        Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleCenter, out _, MovementData.GroundToFallRayDistance, 
                                StateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        protected virtual void OnFall()
        {
            StateMachine.ChangeState(StateMachine.FallingState);
        }
        #endregion

        #region InputMethods

        protected virtual void OnDashStarted(InputAction.CallbackContext ctx)
        {
            StateMachine.ChangeState(StateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext ctx)
        {
            StateMachine.ChangeState(StateMachine.JumpingState);
        }
        #endregion
    }
}
