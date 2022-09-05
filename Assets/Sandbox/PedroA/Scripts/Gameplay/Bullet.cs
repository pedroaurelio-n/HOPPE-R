using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Tortoise.HOPPER
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(DamageArea))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private bool isDeflectable;

        private Transform _source;
        private Rigidbody _rigidbody;
        private DamageArea _damageArea;

        private float _initialSpeed;
        private bool _wasDeflected;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _damageArea = GetComponent<DamageArea>();
        }

        public void Initialize(Transform source, Vector3 direction, float shootSpeed)
        {
            _source = source;
            _initialSpeed = shootSpeed;
            _rigidbody.velocity = direction * _initialSpeed;
        }

        public void DeflectBullet(Transform shield)
        {
            if (!isDeflectable)
                return;

            _wasDeflected = true;

            var newDirection = _source.position - shield.position;
            _rigidbody.velocity = newDirection.normalized * _initialSpeed;

            _damageArea.SwitchDamageInfo(shield.root.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<DamageArea>(out DamageArea damageArea))
                return;
            
            if (_wasDeflected && other.TryGetComponent<Shield>(out Shield shield))
                return;

            if (_wasDeflected)
                Destroy(gameObject);
            else
                StartCoroutine(DestroyBullet());
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForEndOfFrame();

            if (_wasDeflected)
                yield break;
            else
            {
                Destroy(gameObject);
                yield break;
            }
        }
    }
}