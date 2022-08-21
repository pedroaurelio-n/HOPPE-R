using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Tortoise.HOPPER
{
    public class BasicItemUpdater : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNumber;

        private void UpdateNumber(int count)
        {
            itemNumber.text = count.ToString();
        }

        private void OnEnable()
        {
            BasicItemCollection.onItemCountUpdated += UpdateNumber;
        }

        private void OnDisable()
        {
            BasicItemCollection.onItemCountUpdated -= UpdateNumber;            
        }
    }
}
