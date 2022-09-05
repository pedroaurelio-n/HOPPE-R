using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tortoise.HOPPER
{
    public class DamageArea : MonoBehaviour
    {
        [SerializeField] private GameObject damager;
        [SerializeField] private int damageAmount;
        [SerializeField] private Vector3 damageDirection;
        [SerializeField] private float knockbackForce;
        [SerializeField] private float damageCooldown;
        [SerializeField] private bool isOneShot;
        [SerializeField] private bool onlyDamagePlayer;
        [SerializeField] private bool disableAfterUsage;

        private GameObject _damager;
        private Vector3 _direction;
        private bool _isDisabled = false;
        private Coroutine _damageCoroutine;

        private IEnumerator DamageCoroutine(Damageable damageable, DamageData data)
        {
            if (isOneShot)
            {
                damageable.ApplyDamage(data);
                yield return null;
            }
            else
            {
                while (true)
                {
                    damageable.ApplyDamage(data);
                    
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
            if (!other.TryGetComponent<Damageable>(out Damageable damageable))
                return;

            if (onlyDamagePlayer && !damageable.TryGetComponent<Player>(out Player player))
                return;

            if (!_isDisabled)
            {
                _damager = damager == null ? gameObject : damager;
                _direction = damageDirection == Vector3.zero ? damageable.gameObject.transform.position - _damager.transform.position : damageDirection;

                var damageData = new DamageData(damageAmount, _damager, _damager.transform.position, _direction, knockbackForce);

                _damageCoroutine = StartCoroutine(DamageCoroutine(damageable, damageData));
            }

            if (disableAfterUsage)
            {
                _isDisabled = true;
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<Damageable>(out Damageable damageable))
                return;

            if (onlyDamagePlayer && !damageable.TryGetComponent<Player>(out Player player))
                return;

            if (_damageCoroutine != null)
                StopCoroutine(_damageCoroutine);
        }
    }
}
