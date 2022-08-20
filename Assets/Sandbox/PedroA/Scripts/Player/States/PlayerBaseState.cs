using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerBaseState : IState
    {
        protected PlayerStateMachine _StateMachine;
        protected Player _Player;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            _StateMachine = stateMachine;
            _Player = _StateMachine.Player;
        }

        #region IStateMethods
        public virtual void Enter()
        {
            StateUpdateHud.UpdateStateList(GetType().Name);

            AddInputCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void PhysicsUpdate()
        {
            Move();
        }

        public virtual void EnterTrigger(Collider collider)
        {
        }

        public virtual void ExitTrigger(Collider collider)
        {
        }

        public virtual void AnimationEnterEvent()
        {
        }

        public virtual void AnimationExitEvent()
        {
        }

        public virtual void AnimationTransitionEvent()
        {
        }
        #endregion

        #region MainMethods
        private void ReadMovementInput()
        {
            if (_StateMachine.SlopeSpeedModifier == 0f)
                return;

            _StateMachine.MovementInput = _Player.Input.PlayerActions.Move.ReadValue<Vector2>();
            var clampedInput = Mathf.Clamp01(Mathf.Abs(_StateMachine.MovementInput.x) + Mathf.Abs(_StateMachine.MovementInput.y));

            _StateMachine.MoveAmount = Mathf.MoveTowards(_StateMachine.MoveAmount, clampedInput, _Player.Data.PosAccel * Time.deltaTime);
        }

        private void Move()
        {
            if (_StateMachine.MovementInput == Vector2.zero)
                return;
            
            var movementDirection = GetInputDirection();
            Rotate(movementDirection);

            var movementSpeed = GetMovementSpeed();
            var currentHorizontalVelocity = GetHorizontalVelocity();

            _Player.Rigidbody.AddForce(movementSpeed * movementDirection - currentHorizontalVelocity, ForceMode.VelocityChange);
        }

        private void Rotate(Vector3 direction)
        {
            var lookDirection = Quaternion.LookRotation(direction, Vector3.up);
            _Player.transform.rotation = Quaternion.RotateTowards(_Player.transform.rotation, lookDirection, _Player.Data.RotationSpeed);
        }
        #endregion

        #region ReusableMethods
        protected virtual void AddInputCallbacks()
        {
            _Player.Input.PlayerActions.Jump.performed += OnJumpPerformed;
        }

        protected virtual void RemoveInputCallbacks()
        {
            _Player.Input.PlayerActions.Jump.performed -= OnJumpPerformed;
        }
        
        protected Vector3 GetInputDirection()
        {
            var rawDirection = new Vector3(_StateMachine.MovementInput.x, 0f, _StateMachine.MovementInput.y);

            var direction = rawDirection;

            if (_Player.InputSpace != null)
            {
                var directionOnSpace = direction.x * _Player.InputSpace.right + direction.z * _Player.InputSpace.forward;

                direction = Vector3.ProjectOnPlane(directionOnSpace, Vector3.up);
                direction.Normalize();
            }

            return direction;
        }

        protected Vector3 GetHorizontalVelocity()
        {
            return new Vector3(_Player.Rigidbody.velocity.x, 0f, _Player.Rigidbody.velocity.z);
        }

        protected bool IsMovingHorizontally(float minimumSpeed = 0.1f)
        {
            return GetHorizontalVelocity().magnitude > minimumSpeed;
        }

        protected Vector3 GetVerticalVelocity()
        {
            return new Vector3(0f, _Player.Rigidbody.velocity.y, 0f);
        }

        protected bool IsMovingVertically(float minimumSpeed = 0.1f)
        {
            return GetVerticalVelocity().magnitude > minimumSpeed;
        }

        protected bool IsMovingUp(float minimumSpeed = 0.1f)
        {
            return GetVerticalVelocity().y > minimumSpeed;
        }

        protected bool IsMovingDown(float minimumSpeed = -0.1f)
        {
            return GetVerticalVelocity().y < minimumSpeed;
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            var speed = _Player.Data.BaseSpeed * _StateMachine.SpeedModifier;

            if (shouldConsiderSlopes)
                speed *= _StateMachine.SlopeSpeedModifier;

            return speed * _StateMachine.MoveAmount;
        }
        
        protected void DecelerateXZ(float deceleration)
        {
            var horizontalVelocity = GetHorizontalVelocity();

            _Player.Rigidbody.AddForce(deceleration * -horizontalVelocity, ForceMode.Acceleration);
        }

        protected void DecelerateY(Vector3 deceleration)
        {
            _Player.Rigidbody.AddForce(deceleration, ForceMode.Acceleration);
        }

        protected void ResetVelocity()
        {
            _Player.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVelocityXZ()
        {
            _Player.Rigidbody.velocity = GetVerticalVelocity();
        }

        protected void ResetVelocityY()
        {
            _Player.Rigidbody.velocity = GetHorizontalVelocity();
        }

        protected bool IsThereGroundUnderneath()
        {
            var colliders = Physics.OverlapSphere(_Player.transform.position + _Player.Data.GroundOverlapOffset, _Player.Data.GroundOverlapRadius,
                                                _Player.Data.GroundLayer, QueryTriggerInteraction.Ignore);
            
            if (colliders.Length > 0)
            {
                var layer = colliders[0].gameObject.layer;
                _StateMachine.IsOnStairs = (1 << layer & _Player.Data.StairLayer) != 0;
            }

            return colliders.Length > 0;
        }

        protected float SetSlopeSpeedModifierOnAngle(float angle, bool setModifier = true)
        {
            var slopeSpeedModifier = _Player.Data.SlopeSpeedAngles.Evaluate(angle);

            if (setModifier)
                _StateMachine.SlopeSpeedModifier = slopeSpeedModifier;

            return slopeSpeedModifier;
        }
        #endregion

        #region InputMethods
        protected virtual void OnJumpPerformed(InputAction.CallbackContext ctx)
        {
        }
        #endregion
    }
}
