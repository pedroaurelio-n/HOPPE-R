using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class BasicItemCollection : MonoBehaviour
    {
        public delegate void ItemCountUpdated(int count);
        public static event ItemCountUpdated onItemCountUpdated;
        public static int ItemCount;


        private void Start()
        {
            ItemCount = 0;
            onItemCountUpdated?.Invoke(ItemCount);
        }

        public static void AddItem(int count = 1)
        {
            ItemCount += count;
            onItemCountUpdated?.Invoke(ItemCount);
        }

        public static void RemoveItem(int count = 1)
        {
            ItemCount -= count;
            onItemCountUpdated?.Invoke(ItemCount);
        }
    }
}
