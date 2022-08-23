using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class WaveObject : MonoBehaviour
    {
        public delegate void ObjectDisabled(WaveObject waveObject);
        public static event ObjectDisabled onObjectDisabled;

        private void OnDisable()
        {
            onObjectDisabled?.Invoke(this);
        }
    }
}
