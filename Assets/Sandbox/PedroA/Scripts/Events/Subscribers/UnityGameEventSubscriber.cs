using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    public class UnityGameEventSubscriber : BaseGameEventSubscriber
    {
        [SerializeField] private UnityEvent response;

        protected override void InvokeEvent()
        {
            response?.Invoke();

            base.InvokeEvent();
        }
    }
}
