using System;
using System.Collections;
using DG.Tweening;
using EventManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class SwordsmenController : MonoBehaviour,IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip swordTriggeredClip;
        [SerializeField] private WarriorData_SO swordsman_SO;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask friendlyLayer;
        [SerializeField] private float detectionRadius; 
        [SerializeField] private float attackRadius;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float friendlyDetectionDistance;
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;
        
        private float lastAttackTime = -1f; 
        private float attackCooldown = 1f;
        private float currentHealth;
        private string currentState;
        private bool canAttack;
        
        private Coroutine corountine;
        private Transform target;
        private Rigidbody swordsmenRb;
        private Animator animator;
        
        private const string SWORDSMAN_IDLE = "Idle";
        private const string SWORDSMAN_RUN= "Run";
        private const string SWORDSMAN_ATTACK= "Attack";
        private const string SWORDSMAN_DEATH= "Death";
        private const string SWORDSMAN_DAMAGE= "Damage";
       

        private void Awake()
        {
            swordsmenRb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            currentHealth = swordsman_SO.MaxHealth;
            ChangeAnimationState(SWORDSMAN_IDLE);
        }
        
        [Button]
        public void GetAllChildrensMesh()
        {
            childRenderers = GetComponentsInChildren<MeshRenderer>();  
            childSkinMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }

        private void OnEnable()
        {
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
        }
        private void OnDisable()
        {
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
        }
        
        private void OnAllEnemyDeath()
        {
           Destroy(this.gameObject);
        }
        
        private void FixedUpdate()
        {
            if (!IsFriendlyInFront())
            {
                DetectEnemy();
                MoveAndAttack();
            }

            else
            {
                swordsmenRb.velocity=Vector3.zero;
            }
        }
        
        void DetectEnemy()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
            float closestDistance = Mathf.Infinity;
            Collider closestEnemy = null;

            foreach (var hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider;
                }
            }

            if (closestEnemy != null)
            {
                target = closestEnemy.transform;
                Debug.DrawRay(transform.position, target.position - transform.position, Color.red, 1f);
            }
            else
            {
                target = null;
            }
        }
        
        bool IsFriendlyInFront()
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, transform.forward, out hit, friendlyDetectionDistance, friendlyLayer))
            {
                Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow); 
                return true; 
            }
            return false;
        }
        void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 0.2f).SetEase(Ease.OutQuad);
        }
        

        private void MoveAndAttack()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);
                RotateTowards(target.position);

               
                if (distance <= attackRadius && Time.time - lastAttackTime >= attackCooldown)
                {
                    swordsmenRb.velocity = Vector3.zero;
                    Attack();
                }
                else if (distance > attackRadius) 
                {
                    ChangeAnimationState(SWORDSMAN_RUN);
                    Vector3 direction = (target.position - transform.position).normalized;
                    swordsmenRb.MovePosition(swordsmenRb.position + direction * moveSpeed * Time.fixedDeltaTime);
                }
            }
        }

        private void Attack()
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                
                ChangeAnimationState(SWORDSMAN_ATTACK);
                PlayDiceTriggeredSound();
                lastAttackTime = Time.time; 
            }
        }

        private void PlayDiceTriggeredSound()
        {
            if (audioSource != null && swordTriggeredClip != null)
            {
                audioSource.PlayOneShot(swordTriggeredClip);
            }
        }
        public void TakeDamage(float amount)
        {
           currentHealth -= amount;
           ChangeAnimationState(SWORDSMAN_DAMAGE);

           if (currentHealth <= 0)
           {
               ChangeAnimationState(SWORDSMAN_DEATH);
               InternalEvents.WarriorDeath?.Invoke();
               Destroy(this.gameObject);
               
           }
        }
        public void ChangeMeshColor(Color newcolor)
        {
            corountine= StartCoroutine(SkinColorDuration(newcolor));
        }

        IEnumerator SkinColorDuration(Color newcolor)
        {
            SetMeshColor(newcolor);
            
            yield return new WaitForSeconds(0.2f);
            
            Color oldColor = Color.white;
            SetMeshColor(oldColor);
            
            StopCoroutine();
        }
        
        void SetMeshColor(Color color)
        {
            foreach (MeshRenderer childRenderer in childRenderers)
            {
                childRenderer.material.SetColor("_Color", color);
            }
            foreach (SkinnedMeshRenderer childSkinRenderer in childSkinMeshRenderer)
            {
                childSkinRenderer.material.SetColor("_Color", color);
            }
        }

        private void StopCoroutine()
        {
            if (corountine != null)
            {
                StopCoroutine(corountine);
                corountine = null;
            }
        }


        private void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;
            
            animator.Play(newState);
            currentState = newState;
        }
    }
}
