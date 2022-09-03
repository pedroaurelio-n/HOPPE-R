using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Tortoise.HOPPER
{
    public class RestartLevel : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.rKey.wasPressedThisFrame)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
