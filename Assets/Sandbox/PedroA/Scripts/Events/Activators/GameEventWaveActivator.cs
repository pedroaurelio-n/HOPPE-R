using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class GameEventWaveActivator : GameEventBaseActivator
    {
        [SerializeField] private List<WaveObject> objectsInWave;

        private void UpdateList(WaveObject waveObject)
        {
            if (objectsInWave.Contains(waveObject))
                objectsInWave.Remove(waveObject);

            if (objectsInWave.Count <= 0)
                Raise();
        }

        private void OnEnable()
        {
            WaveObject.onObjectDisabled += UpdateList;
        }

        private void OnDisable()
        {
            WaveObject.onObjectDisabled -= UpdateList;            
        }
    }
}
