using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using EventManager;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;
using Warrior;

namespace Enemy
{
    public class Enemy_ArcherController : MonoBehaviour,IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip arrowTriggeredClip;
        [SerializeField] private EnemyData_SO archerSO;
        [SerializeField] private Transform arrowPrefab;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask friendlyLayer;
        [SerializeField] private GameObject arrowHolder;
        [SerializeField] private float detectionRadius;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private float minDistanceToEnemy; 
        [SerializeField] private float friendlyDistance;
        [SerializeField] private float moveSpeed = 3f;
        
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;
        
        private string m_currentState;
        private float lastAttackTime = -1f;
        private float currentHealth;
        
        private Rigidbody archerRb; 
        private Animator animator;
        private Coroutine corountine;
        
        private const string E_ARCHER_IDLE = "Idle";
        private const string E_ARCHER_RUN= "Run";
        private const string E_ARCHER_ATTACK= "Attack";
        private const string E_ARCHER_DEATH= "Death";
       
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            ChangeAnimationState(E_ARCHER_IDLE);
            archerRb = GetComponent<Rigidbody>();
            currentHealth = archerSO.MaxHealth;
        }
        
        [Button]
        public void GetAllChildrensMesh()
        {
            childRenderers = GetComponentsInChildren<MeshRenderer>();  
            childSkinMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        private void OnEnable()
        {
            
            ExternalEvents.FightTextAssigned += OnFightTextAssigned;
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
        }

        private void OnDisable()
        {
            ExternalEvents.FightTextAssigned -= OnFightTextAssigned;
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
        }

        private void OnAllEnemyDeath()
        {
          
        }

        private void OnFightTextAssigned()
        {
            archerRb.isKinematic = false;
        }


        void FixedUpdate()
        {
            if (archerRb.isKinematic == false)
            {
                    Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
                    Transform closestEnemy = FindClosestEnemy(enemies);
                    
                    if (closestEnemy != null)
                    {
                        float distanceToClosestEnemy = Vector3.Distance(transform.position, closestEnemy.position);
                        if (distanceToClosestEnemy > minDistanceToEnemy)
                        {
                            ChangeAnimationState(E_ARCHER_RUN);
                            MoveTowards(closestEnemy.position);
                            archerRb.isKinematic = false;
                        }
                        else
                        {
                            archerRb.velocity = Vector3.zero;
                        }

                        RotateTowards(closestEnemy.position);
                        if (Time.time - lastAttackTime >= attackCooldown)
                        {
                            ChangeAnimationState(E_ARCHER_ATTACK);
                            ShootArrow(closestEnemy);
                            lastAttackTime = Time.time;
                        }
                    }
            }
            
        }
        

        void MoveTowards(Vector3 targetPosition)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            archerRb.MovePosition(archerRb.position + direction * moveSpeed * Time.fixedDeltaTime); 
        }

        void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 0.2f).SetEase(Ease.OutQuad);
        }

        Transform FindClosestEnemy(Collider[] enemies)
        {
            Transform closestEnemy = null;
            float closestDistance = Mathf.Infinity;
            foreach (Collider enemy in enemies)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }
            }
            return closestEnemy;
        }

        void ShootArrow(Transform target)
        {
            Quaternion additionalRotation = Quaternion.Euler(90, 0, 0);
            Vector3 directionToTarget = target.position -arrowHolder.transform.position;
            Transform arrow = Instantiate(arrowPrefab,arrowHolder.transform.position,Quaternion.LookRotation(directionToTarget)*additionalRotation);
            
            PlayDiceTriggeredSound();
            RaycastHit hitInfo;
            bool hasMeleeInFront = Physics.Raycast(transform.position, directionToTarget, out hitInfo, 50f, friendlyLayer);

            if (hasMeleeInFront)
            {
               
                Debug.DrawLine(transform.position, hitInfo.point, Color.red,2f);
            }
            else
            {
               
                Debug.DrawLine(transform.position, transform.position + directionToTarget.normalized * 50f, Color.green,2f);
            }
            
    
            arrow.GetComponent<Enemy_Arrow>().Launch(target, hasMeleeInFront);
        }
        
        private void PlayDiceTriggeredSound()
        {
            if (audioSource != null && arrowTriggeredClip != null)
            {
                audioSource.PlayOneShot(arrowTriggeredClip);
            }
        }


        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            
            if (currentHealth <= 0)
            {
                ChangeAnimationState(E_ARCHER_DEATH);
                InternalEvents.EnemyDeath?.Invoke(Enums.EnemyType.EnemyArcher);
                Debug.Log(gameObject.name);
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
            if (m_currentState == newState) return;
            
            animator.Play(newState);
            m_currentState = newState;
        }
    }
}
