using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    [CreateAssetMenu(fileName = "New Game Event", menuName = "Game Event")]
    public class GameEvent : ScriptableObject
    {
        private List<BaseGameEventSubscriber> subscribers = new List<BaseGameEventSubscriber>();

        public void RaiseEvent()
        {
            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                subscribers[i].OnEventRaised();
            }
        }

        public void RegisterListener(BaseGameEventSubscriber subscriber)
        {
            if (!subscribers.Contains(subscriber))
                subscribers.Add(subscriber);
        }

        public void UnregisterListener(BaseGameEventSubscriber subscriber)
        {
            if (subscribers.Contains(subscriber))
                subscribers.Remove(subscriber);
        }
    }
}
