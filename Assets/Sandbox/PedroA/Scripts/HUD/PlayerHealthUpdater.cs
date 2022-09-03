using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tortoise.HOPPER
{
    public class PlayerHealthUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNumber;

        public void UpdateNumber(int health)
        {
            itemNumber.text = health.ToString();
        }
    }
}
