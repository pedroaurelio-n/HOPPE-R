using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Tortoise.HOPPER
{
    public class ShootObjects : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Rigidbody objectToShoot;
        [SerializeField] private Transform rotatingObject;
        [SerializeField] private Transform shootPosition;
        [SerializeField] private float shootSpeed;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float shootInterval;
        [SerializeField] private float minRange;
        [SerializeField] private float maxRange;

        private float _elapsedTime;
        private bool _isTargetInRange;

        private void OnValidate()
        {
            if (maxRange < minRange)
                maxRange = minRange;

            if (minRange > maxRange)
                minRange = maxRange;
        }

        private void Awake()
        {
            OnValidate();

            _elapsedTime = 0f;
        }

        private void Update()
        {
            if (target == null)
                return;

            var targetDistance = Vector3.Distance(target.position, rotatingObject.position);
            _isTargetInRange = targetDistance >= minRange && targetDistance <= maxRange;

            if (!_isTargetInRange)
                return;

            if (_elapsedTime < shootInterval)
            {
                _elapsedTime += Time.deltaTime;
                return;
            }

            Shoot();
            _elapsedTime = 0f;
        }

        private void FixedUpdate()
        {
            if (!_isTargetInRange)
                return;

            var direction = target.position - transform.position;
            Rotate(direction.normalized);
        }

        public void Shoot()
        {
            var temp = Instantiate(objectToShoot, shootPosition.position, Quaternion.identity);
            temp.velocity = rotatingObject.forward * shootSpeed;
        }

        private void Rotate(Vector3 direction)
        {
            var lookDirection = Quaternion.LookRotation(direction, Vector3.up);
            rotatingObject.rotation = Quaternion.RotateTowards(rotatingObject.rotation, lookDirection, rotationSpeed);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(rotatingObject.position, minRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(rotatingObject.position, maxRange);
        }
    }
}