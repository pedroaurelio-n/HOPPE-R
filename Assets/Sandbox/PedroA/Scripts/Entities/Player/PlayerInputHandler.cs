using UnityEngine;
using UnityEngine.InputSystem;

namespace Tortoise.HOPPER
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerControls Controls;
        public PlayerControls.PlayerActions PlayerActions;

        private void OnEnable()
        {
            if (Controls == null)
            {
                Controls = new PlayerControls();
                PlayerActions = Controls.Player;

                Controls.Enable();
            }
        }

        private void OnDisable()
        {
            Controls.Disable();            
        }
    }
}
