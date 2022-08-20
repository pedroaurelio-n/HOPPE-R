using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class GameEventTriggerActivator : GameEventBaseActivator
    {
        [SerializeField] private LayerMask layerMask;

        private void OnTriggerEnter(Collider other)
        {
            if ((1 << other.gameObject.layer & layerMask) != 0)
                Raise();
        }
    }
}
