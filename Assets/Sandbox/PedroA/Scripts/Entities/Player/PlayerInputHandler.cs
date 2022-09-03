using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public delegate void InteractPerformed();
        public static event InteractPerformed onInteractPerformed;

        public delegate void ShieldPerformed();
        public static event ShieldPerformed onShieldPerformed;
        public delegate void ShieldCanceled();
        public static event ShieldCanceled onShieldCanceled;

        public PlayerControls Controls;
        public PlayerControls.PlayerActions PlayerActions;

        private void OnEnable()
        {
            if (Controls == null)
            {
                Controls = new PlayerControls();
                PlayerActions = Controls.Player;

                PlayerActions.Interact.performed += InteractInputPerformed;

                PlayerActions.Shield.performed += ShieldInputPerformed;
                PlayerActions.Shield.canceled += ShieldInputCanceled;

                Controls.Enable();
            }
        }

        private void OnDisable()
        {
            PlayerActions.Interact.performed -= InteractInputPerformed;

            PlayerActions.Shield.performed -= ShieldInputPerformed;
            PlayerActions.Shield.canceled -= ShieldInputCanceled;

            Controls.Disable();
        }

        private void InteractInputPerformed(InputAction.CallbackContext ctx)
        {
            onInteractPerformed?.Invoke();
        }

        private void ShieldInputPerformed(InputAction.CallbackContext ctx)
        {
            onShieldPerformed?.Invoke();
        }

        private void ShieldInputCanceled(InputAction.CallbackContext ctx)
        {
            onShieldCanceled?.Invoke();
        }
    }
}
