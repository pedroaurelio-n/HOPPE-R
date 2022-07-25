using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinController
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public PlayerControls PlayerControls { get; private set; }
        public PlayerControls.PlayerActions PlayerActions { get; private set; }

        private void Awake()
        {
            PlayerControls = new PlayerControls();

            PlayerActions = PlayerControls.Player;
        }

        private void OnEnable()
        {
            PlayerControls.Enable();
        }

        private void OnDisable()
        {
            PlayerControls.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}
