using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Tortoise.HOPPER
{
    public class ShieldHealth : MonoBehaviour
    {
        public Damageable Damageable { get; private set; }

        public float RechargeMeter { get => _rechargeMeter; }

        [SerializeField] private float normalRechargeRate;
        [SerializeField] private float brokenRechargeRate;

        private float _rechargeMeter;
        private float _rechargeRate;

        private void Awake()
        {
            Damageable = GetComponent<Damageable>();

            _rechargeRate = normalRechargeRate;
        }

        private void Update()
        {
            if (Damageable.Health >= Damageable.MaxHealth)
            {
                _rechargeMeter = 0f;
                return;
            }

            if (_rechargeMeter >= 1f)
            {
                _rechargeRate = normalRechargeRate;
                _rechargeMeter = 0f;
                Damageable.IncreaseHealth(1);
                return;
            }

            _rechargeMeter += _rechargeRate * Time.deltaTime;
        }

        // UnityEvent Reference
        public void BrokenRecharge()
        {
            _rechargeMeter = 0f;
            _rechargeRate = brokenRechargeRate;
        }
    }
}