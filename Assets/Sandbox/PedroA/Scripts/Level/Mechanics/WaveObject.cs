using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class WaveObject : MonoBehaviour
    {
        public delegate void ObjectDisabled(WaveObject waveObject);
        public static event ObjectDisabled onObjectDisabled;

        public void DisableObject()
        {
            onObjectDisabled?.Invoke(this);
        }

        private void OnDisable()
        {
            DisableObject();
        }
    }
}
