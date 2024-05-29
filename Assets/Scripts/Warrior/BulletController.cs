using System;
using System.Collections;
using Enemy;
using EventManager;
using UnityEngine;

namespace Warrior
{
    public class BulletController:MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitTriggeredClip;
        [SerializeField] private GameObject damagePartical;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float attackRange;
        [SerializeField] private float speed;
        
        private Rigidbody bulletRb;

        private void Awake()
        {
            bulletRb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            InternalEvents.BulletSpawned += OnCatapultSpawned;
        }

        private void OnDisable()
        {
            InternalEvents.BulletSpawned -= OnCatapultSpawned;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BossHealthHandler _))
            {
                Instantiate(damagePartical, transform.position, transform.rotation);
                Destroy(this.gameObject);
            }
        }
        
        private void OnCatapultSpawned()
        {
            CalculateLaunchVelocity();
        }

        public void CalculateLaunchVelocity()
        {
            RaycastHit hit;
            
            Debug.DrawRay(transform.position, Vector3.forward*attackRange, Color.red, 10f);
            
            if (Physics.Raycast(transform.position, Vector3.forward, out hit, attackRange, targetLayer))
            {
                if (hit.collider != null)
                {
                    Vector3 toTarget = (hit.point - transform.position).normalized;
                    
                    float angle = 35f; 
                    float radians = angle * Mathf.Deg2Rad;
                    
                    Vector3 velocity = new Vector3(
                        toTarget.x * Mathf.Cos(radians),
                        Mathf.Sin(radians),
                        toTarget.z * Mathf.Cos(radians)
                    ) * speed;

                    bulletRb.velocity = velocity;
                }
            }
        }
    }
}