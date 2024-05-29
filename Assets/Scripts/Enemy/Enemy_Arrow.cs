using System;
using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;

namespace Warrior
{
    public class Enemy_Arrow : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip arrowTriggeredClip;
        [SerializeField] private AudioClip particalTriggeredClip;
        [SerializeField] private GameObject dustPartical;
        [SerializeField] private string PlatformLayer = "Platform";
        [SerializeField]  private string WarriorLayer = "Warrior";
        [SerializeField] private float speed;
        
        [Range(20.0f, 75.0f)] public float launchAngle = 45.0f;
        
        private Quaternion initialRotation;
        private Rigidbody arrowRigidbody;
        
        private bool arcShot;

        void Awake()
        {
            arrowRigidbody = GetComponent<Rigidbody>();
            initialRotation = transform.rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            int platformLayerIndex = LayerMask.NameToLayer(PlatformLayer);
            int warriorLayer=LayerMask.NameToLayer(WarriorLayer);
        
            if (other.gameObject.layer == platformLayerIndex)
            {
                PlayParticalTriggeredSound();
                GameObject particleInstance =Instantiate(dustPartical, transform.position, transform.rotation);
                Destroy(particleInstance, 2f);
            }
            
            if (other.gameObject.layer == warriorLayer)
            {
                Destroy(this.gameObject);
            }
            
        }
        
        private void PlayParticalTriggeredSound()
        {
            if (audioSource != null && particalTriggeredClip != null)
            {
                audioSource.PlayOneShot(particalTriggeredClip);
            }
        }
        

        public void Launch(Transform target, bool arcShotParam)
        {
            Vector3 targetPosition = target.position;
            Vector3 launchDirection = (targetPosition - transform.position).normalized;
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (arcShotParam)
            {
                float deltaY = targetPosition.y - transform.position.y;
                Vector3 horizontalDirection = new Vector3(launchDirection.x, 0f, launchDirection.z).normalized;
                float horizontalDistance = Mathf.Sqrt(distanceToTarget * distanceToTarget - deltaY * deltaY);

          
                float gravity = Physics.gravity.magnitude;
                float launchSpeed = CalculateLaunchSpeed(horizontalDistance, deltaY, gravity, launchAngle);
            
                float verticalVelocity = launchSpeed * Mathf.Sin(launchAngle * Mathf.Deg2Rad);
                float horizontalVelocity = launchSpeed * Mathf.Cos(launchAngle * Mathf.Deg2Rad);

                Vector3 initialVelocity = horizontalDirection * horizontalVelocity + Vector3.up * verticalVelocity;
                arrowRigidbody.velocity = initialVelocity;

            
                
            }
            else
            {
                Vector3 flatTargetPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z); 
                Vector3 flatLaunchDirection = (flatTargetPosition - transform.position).normalized; 
                
                arrowRigidbody.velocity = flatLaunchDirection * speed; 
            }
            
            StartCoroutine(UpdateArrowRotation(arrowRigidbody));
            

        }
        private float CalculateLaunchSpeed(float horizontalDistance, float deltaY, float gravity, float launchAngle)
        {
            float launchSpeed = Mathf.Sqrt((gravity * horizontalDistance * horizontalDistance) / (2 * Mathf.Cos(launchAngle * Mathf.Deg2Rad) * Mathf.Cos(launchAngle * Mathf.Deg2Rad) * (horizontalDistance * Mathf.Tan(launchAngle * Mathf.Deg2Rad) - deltaY)));
            return launchSpeed;
        }
        
        
        private IEnumerator UpdateArrowRotation(Rigidbody arrowRigidbody)
        {
            Vector3 originalPosition = transform.position;
            yield return new WaitForFixedUpdate(); 

            while (arrowRigidbody != null && arrowRigidbody.velocity.sqrMagnitude > 0.1f)
            {
                transform.rotation = Quaternion.LookRotation(arrowRigidbody.velocity)*initialRotation;
                yield return new WaitForFixedUpdate();
            }

            transform.position = originalPosition;
            
        }
        
        
    }
}

