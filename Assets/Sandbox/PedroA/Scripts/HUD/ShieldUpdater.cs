using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 
namespace Tortoise.HOPPER
{
    public class ShieldUpdater : MonoBehaviour
    {
        [SerializeField] private ShieldHealth shield;
        [SerializeField] private Image healthFillImage;
        [SerializeField] private Image rechargeFillImage;

        private void Update()
        {
            var currentHealth = (float)shield.Damageable.Health;
            var maxHealth = (float)shield.Damageable.MaxHealth;

            healthFillImage.fillAmount = currentHealth / maxHealth;
            rechargeFillImage.fillAmount = shield.RechargeMeter;
        }
    }
}