using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class PlayerGroundCheck : MonoBehaviour
    {
        [SerializeField] private Player player;

        private void OnTriggerEnter(Collider other)
        {
            player.EnterTrigger(other);
        }

        private void OnTriggerExit(Collider other)
        {
            player.ExitTrigger(other);
        }
    }
}
