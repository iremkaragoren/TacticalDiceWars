using System.Collections;
using EventManager;
using UnityEngine;

namespace Player
{
    public class PlayerBossBattle : MonoBehaviour
    {
        [SerializeField] private GameObject arrowPrefab;
        [SerializeField] private Transform shootPoint;
        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private float attackRange = 50f;
        [SerializeField] private float attackCooldown = 3f;

        private float lastAttackTime = -Mathf.Infinity;

        private void OnEnable()
        {
            InternalEvents.BossFightTriggered += OnBossFight;
        }

        private void OnDisable()
        {
            InternalEvents.BossFightTriggered -= OnBossFight;
        }

        private void OnBossFight()
        {
            
            StartCoroutine(FireArrowsAtIntervals(attackCooldown));
        }

        private IEnumerator FireArrowsAtIntervals(float interval)
        {
            while (true)
            {
                if (Time.time >= lastAttackTime + interval)
                {
                    FireArrow();
                    lastAttackTime = Time.time;
                }
                yield return new WaitForSeconds(interval);
            }
        }

        private void FireArrow()
        {
            RaycastHit hit;

            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, attackRange, targetLayer))
            {
                
                var arrowInstance = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(hit.point - shootPoint.position)) as GameObject;
                
               
                var arrowRigidbody = arrowInstance.GetComponent<Rigidbody>();
                if (arrowRigidbody != null)
                {
                    Vector3 direction = (hit.point - shootPoint.position).normalized;
                    arrowRigidbody.velocity = direction * 20f;
                }

                Debug.DrawRay(shootPoint.position, shootPoint.forward * attackRange, Color.red, 2f);
            }
        }
    }
}
