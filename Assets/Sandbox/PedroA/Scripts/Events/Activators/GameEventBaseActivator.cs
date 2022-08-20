using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class GameEventBaseActivator : MonoBehaviour
    {
        [SerializeField] protected GameEvent gameEvent;

        public void Raise()
        {
            gameEvent.RaiseEvent();
        }
    }
}
