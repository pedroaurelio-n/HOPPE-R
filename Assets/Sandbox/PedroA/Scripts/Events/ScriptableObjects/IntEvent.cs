using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Tortoise.HOPPER
{
    [System.Serializable]
    public class IntUnityEvent : UnityEvent<int>
    {
    }

    [CreateAssetMenu(fileName = "New Game Event", menuName = "Events/Int Event")]
    public class IntEvent : GameEvent
    {
        public void RaiseEvent(int value)
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                var subscriber = subscribers[i] as IntUnityGameEventSubscriber;
                subscriber.OnEventRaised(value);
            }
        }
    }
}
