using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerMovementState : IState
    {
        protected PlayerMovementStateMachine StateMachine;

        protected PlayerGroundedData MovementData;
        protected PlayerAirborneData AirborneData;

        public PlayerMovementState(PlayerMovementStateMachine stateMachine)
        {
            StateMachine = stateMachine;

            MovementData = StateMachine.Player.Data.GroundedData;
            AirborneData = StateMachine.Player.Data.AirborneData;

            SetBaseCameraRecenteringData();

            InitializeData();
        }

        private void InitializeData()
        {
            SetBaseRotationData();
        }

        #region IStateMethods
        public virtual void Enter()
        {
            Debug.Log($"State: {GetType().Name}");

            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (StateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);
                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (StateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);
                return;
            }
        }
        #endregion

        #region MainMethods
        private void ReadMovementInput()
        {
            StateMachine.ReusableData.MovementInput = StateMachine.Player.Input.PlayerActions.Move.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (StateMachine.ReusableData.MovementInput == Vector2.zero || StateMachine.ReusableData.MovementSpeedModifier == 0f)
                return;
            
            var movementDirection = GetMovementDirection();
            var targetRotationYAngle = Rotate(movementDirection);

            var targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            var movementSpeed = GetMovementSpeed();

            var currentHorizontalVelocity = GetHorizontalVelocity();
            StateMachine.Player.Rigidbody.AddForce(movementSpeed * targetRotationDirection - currentHorizontalVelocity, ForceMode.VelocityChange);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        private float GetDirectionAngle(Vector3 direction)
        {
            var directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
                directionAngle += 360f;

            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += StateMachine.Player.MainCameraTransform.eulerAngles.y;

            if (angle > 360f)
                angle -= 360f;

            return angle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            StateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            StateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }
        #endregion

        #region ReusableMethods
        protected void StartAnimation(int animationHash)
        {
            StateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            StateMachine.Player.Animator.SetBool(animationHash, false);
        }

        protected virtual void AddInputActionsCallbacks()
        {
            StateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            StateMachine.Player.Input.PlayerActions.Move.canceled += OnMovementCanceled;

            StateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;
            StateMachine.Player.Input.PlayerActions.Move.performed += OnMovementPerformed;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            StateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            StateMachine.Player.Input.PlayerActions.Move.canceled -= OnMovementCanceled;

            StateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;
            StateMachine.Player.Input.PlayerActions.Move.performed -= OnMovementPerformed;
        }

        protected Vector3 GetMovementDirection()
        {
            return new Vector3(StateMachine.ReusableData.MovementInput.x, 0, StateMachine.ReusableData.MovementInput.y);
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            var movementSpeed = MovementData.BaseSpeed * StateMachine.ReusableData.MovementSpeedModifier;

            if (shouldConsiderSlopes)
                movementSpeed *= StateMachine.ReusableData.MovementOnSlopesSpeedModifier;

            return movementSpeed;
        }

        protected Vector3 GetHorizontalVelocity()
        {
            var horizontalVelocity = StateMachine.Player.Rigidbody.velocity;
            horizontalVelocity.y = 0f;

            return horizontalVelocity;
        }

        protected Vector3 GetVerticalVelocity()
        {
            return new Vector3(0f, StateMachine.Player.Rigidbody.velocity.y, 0f);
        }

        protected void RotateTowardsTargetRotation()
        {
            var currentYAngle = StateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle == StateMachine.ReusableData.CurrentTargetRotation.y)
                return;

            var smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, StateMachine.ReusableData.CurrentTargetRotation.y, ref StateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, 
                                                        StateMachine.ReusableData.TimeToReachTargetRotation.y - StateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            StateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            var targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            StateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
                directionAngle = AddCameraRotationToAngle(directionAngle);

            if (directionAngle != StateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }
        
        protected Vector3 GetTargetRotationDirection(float targetAngle)
        {
            return Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        }

        protected void ResetVelocity()
        {
            StateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            var horizontalVelocity = GetHorizontalVelocity();

            StateMachine.Player.Rigidbody.velocity = horizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            var playerHorizontalVelocity = GetHorizontalVelocity();

            StateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * StateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            var playerVerticalVelocity = GetVerticalVelocity();

            StateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * StateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            var playerHorizontalVelocity = GetHorizontalVelocity();
            var playerHorizontalMovement = new Vector2(playerHorizontalVelocity.x, playerHorizontalVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetVerticalVelocity().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetVerticalVelocity().y < -minimumVelocity;
        }

        protected void SetBaseRotationData()
        {
            StateMachine.ReusableData.RotationData = MovementData.BaseRotationData;

            StateMachine.ReusableData.TimeToReachTargetRotation = StateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }
        
        protected virtual void OnContactWithGround(Collider collider)
        {
        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {
        }

        protected void UpdateCameraRecenteringState(Vector2 movementInput)
        {
            if (movementInput == Vector2.zero)
                return;
            
            if (movementInput == Vector2.up)
            {
                DisableCameraRecentering();
                return;
            }

            var cameraVerticalAngle = StateMachine.Player.MainCameraTransform.eulerAngles.x;

            if (cameraVerticalAngle >= 270f)
                cameraVerticalAngle -= 360f;

            cameraVerticalAngle = Mathf.Abs(cameraVerticalAngle);

            if (movementInput == Vector2.down)
            {
                SetCameraRecenteringState(cameraVerticalAngle, StateMachine.ReusableData.BackwardsCameraRecenteringData);
                return;
            }

            SetCameraRecenteringState(cameraVerticalAngle, StateMachine.ReusableData.SidewaysCameraRecenteringData);
        }

        protected void EnableCameraRecentering(float waitTime = -1f, float recenteringTime = -1f)
        {
            var movementSpeed = GetMovementSpeed();

            if (movementSpeed == 0f)
            {
                movementSpeed = MovementData.BaseSpeed;
            }

            StateMachine.Player.CameraUtility.EnableRecentering(waitTime, recenteringTime, MovementData.BaseSpeed, movementSpeed);
        }

        protected void DisableCameraRecentering()
        {
            StateMachine.Player.CameraUtility.DisableRecentering();
        }

        protected void SetCameraRecenteringState(float angle, List<PlayerCameraRecenteringData> cameraRecenteringData)
        {
            for (int i = 0; i < cameraRecenteringData.Count; i++)
            {
                if (!cameraRecenteringData[i].IsWithinRange(angle))
                    continue;
                
                EnableCameraRecentering(cameraRecenteringData[i].WaitTime, cameraRecenteringData[i].RecenteringTime);
                return;
            }

            DisableCameraRecentering();
        }

        protected void SetBaseCameraRecenteringData()
        {
            StateMachine.ReusableData.BackwardsCameraRecenteringData = MovementData.BackwardsCameraRecenteringData;
            StateMachine.ReusableData.SidewaysCameraRecenteringData = MovementData.SidewaysCameraRecenteringData;
        }
        #endregion

        #region InputMethods
        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext ctx)
        {
            StateMachine.ReusableData.ShouldWalk = !StateMachine.ReusableData.ShouldWalk;
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext ctx)
        {
            DisableCameraRecentering();
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext ctx)
        {
            UpdateCameraRecenteringState(StateMachine.ReusableData.MovementInput);
        }

        private void OnMovementPerformed(InputAction.CallbackContext ctx)
        {
            UpdateCameraRecenteringState(ctx.ReadValue<Vector2>());
        }
        #endregion
    }
}
