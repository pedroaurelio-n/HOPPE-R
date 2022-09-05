using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class WaveObject : MonoBehaviour
    {
        public delegate void ObjectDisabled(WaveObject waveObject);
        public static event ObjectDisabled onObjectDisabled;

        public void RemoveObjectFromWave()
        {
            onObjectDisabled?.Invoke(this);
        }

        private void OnDisable()
        {
            RemoveObjectFromWave();
        }
    }
}
