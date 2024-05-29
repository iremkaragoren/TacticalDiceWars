using System;
using System.Collections;
using DG.Tweening;
using Enemy;
using EventManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
namespace Warrior
{
    public class ArchersController : MonoBehaviour, IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip arrowTriggeredClip;
        [SerializeField] private WarriorData_SO archerSO;
        [SerializeField] private GameObject archerHolder;
        [SerializeField] private Transform arrowPrefab;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask friendlyLayer;
        [SerializeField] private float detectionRadius = 15f;
        [SerializeField] private float attackCooldown = 1f;
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;
        
        private Animator animator;
        private Coroutine coroutine;
        
        private float lastAttackTime = -1f;
        private float currentHealth;
        private int bulletCount = 4;
        private int spawnedBulletCount;
        private bool resetOnCatapultSpawned = false;
        private string currentState;
        private bool canBulletSpawned = false;
        
        private const string ARCHER_IDLE = "Idle";
        private const string ARCHER_ATTACK = "Attack";
        private const string ARCHER_DEATH = "Death";
        private const string ARCHER_DAMAGE = "Damage";

        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentHealth = archerSO.MaxHealth;
            ChangeAnimationState(ARCHER_IDLE);
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
            InternalEvents.AttackerSpawned += OnCatapultSpawned;
            InternalEvents.AttackerSpawned += OnCatapultSpawned;
        }
        private void OnDisable()
        {
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
            InternalEvents.AttackerSpawned -= OnCatapultSpawned;
            InternalEvents.AttackerSpawned -= OnCatapultSpawned;
        }
        private void OnAllEnemyDeath()
        {
            Destroy(this.gameObject);
        }
        
        private void OnCatapultSpawned(GameObject attacker, int attackCount)
        {
            spawnedBulletCount = 0;
            resetOnCatapultSpawned = true;
        }

        void FixedUpdate()
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
                if (enemies.Length > 0)
                {
                    Transform closestEnemy = FindClosestEnemy(enemies);
                    if (closestEnemy != null)
                    {
                        RotateTowards(closestEnemy.position);
                        ChangeAnimationState(ARCHER_ATTACK);
                        ShootArrow(closestEnemy);
                        lastAttackTime = Time.time;
                    }
                }
            }
        }

        Transform FindClosestEnemy(Collider[] enemies)
        {
            Transform closestEnemy = null;
            float closestDistance = detectionRadius;
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

        void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 0.2f).SetEase(Ease.OutQuad);
        }

        void ShootArrow(Transform target)
        {
            spawnedBulletCount++;

            if (resetOnCatapultSpawned && spawnedBulletCount >= bulletCount)
            {
                this.enabled = false;
                animator.enabled = false;
                resetOnCatapultSpawned = false;
            }

            Quaternion additionalRotation = Quaternion.Euler(-90, 0, 0);
            Vector3 directionToTarget = target.position - archerHolder.transform.position;
            Transform arrow = Instantiate(arrowPrefab, archerHolder.transform.position, Quaternion.LookRotation(directionToTarget) * additionalRotation);

            RaycastHit hitInfo;
            bool hasMeleeInFront = Physics.Raycast(transform.position, directionToTarget, out hitInfo, 50f, friendlyLayer);

            if (hasMeleeInFront)
            {
                Debug.DrawLine(transform.position, hitInfo.point, Color.red, 2f);
            }
            else
            {
                Debug.DrawLine(transform.position, transform.position + directionToTarget.normalized * 50f, Color.green, 2f);
            }

            arrow.GetComponent<Arrow>().Launch(target, hasMeleeInFront);
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;
            ChangeAnimationState(ARCHER_DAMAGE);
            if (currentHealth <= 0)
            {
                ChangeAnimationState(ARCHER_DEATH);
                InternalEvents.WarriorDeath?.Invoke();
                Destroy(this.gameObject);
            }
        }

        public void ChangeMeshColor(Color newcolor)
        {
            coroutine = StartCoroutine(SkinColorDuration(newcolor));
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
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        private void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;
            animator.Play(newState);
        }
    }
}

