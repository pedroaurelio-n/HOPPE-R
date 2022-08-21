using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class HideLockCursor : MonoBehaviour
    {
        [SerializeField] private bool hideCursor;
        [SerializeField] private bool lockCursor;

        private void Start()
        {
            Cursor.visible = !hideCursor;

            if (lockCursor)
                Cursor.lockState = CursorLockMode.Confined;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
