using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private int damageAmount;
        [SerializeField] private float damageCooldown;
        [SerializeField] private bool isOneShot;
        [SerializeField] private bool disableAfterUsage;

        private bool _isDisabled = false;
        private Coroutine _damageCoroutine;

        private IEnumerator DamageCoroutine(Damageable damageable)
        {
            if (isOneShot)
            {
                damageable.ApplyDamage(damageAmount);
                yield return null;
            }
            else
            {
                while (true)
                {
                    damageable.ApplyDamage(damageAmount);
                    
                    var elapsedTime = 0f;

                    while (elapsedTime <= damageCooldown)
                    {
                        elapsedTime += Time.deltaTime;
                        yield return null;
                    }

                    yield return null;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Damageable>(out Damageable damageable))
            {
                if (!_isDisabled)
                    _damageCoroutine = StartCoroutine(DamageCoroutine(damageable));
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<Damageable>(out Damageable damageable))
            {
                if (_damageCoroutine != null)
                    StopCoroutine(_damageCoroutine);

                if (disableAfterUsage)
                    _isDisabled = true;
            }
        }
    }
}
