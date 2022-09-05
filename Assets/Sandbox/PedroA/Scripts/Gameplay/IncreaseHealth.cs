using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class IncreaseHealth : MonoBehaviour
    {
        [SerializeField, Min(0f)] private int healthChange;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<Player>(out Player player))
                return;

            var damageable = player.GetComponent<Damageable>();

            damageable.IncreaseHealth(healthChange);
            gameObject.SetActive(false);
        }
    }
}
