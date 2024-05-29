
using System.Collections;
using DG.Tweening;
using EventManager;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Enemy
{
    public class GiantController : MonoBehaviour,IDamageble
    {
        [SerializeField] private EnemyData_SO giant_SO;
        [SerializeField] private LayerMask warriorLayer;
        [SerializeField] private float detectionRadius;
        [SerializeField] private float attackRadius;
        [SerializeField] private float moveSpeed;

        private Animator animator;
        private Transform target;
        private Rigidbody giantRb;
        private Coroutine corountine;
        
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;

        private string currentState;
        private float currentHealth;
        
        private const string GIANT_IDLE = "Idle";
        private const string GIANT_RUN= "Run";
        private const string GIANT_ATTACK= "Attack";
        private const string GIANT_DEATH= "Death";
        
        [Button]
        public void GetAllChildrensMesh()
        {
            childRenderers = GetComponentsInChildren<MeshRenderer>();  
            childSkinMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        private void Awake()
        {
            animator = GetComponent<Animator>();
            giantRb = GetComponent<Rigidbody>(); 
            currentHealth = giant_SO.MaxHealth;
            ChangeAnimationState(GIANT_IDLE);
            
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
            // Destroy(this.gameObject);
        }

        private void OnFightTextAssigned()
        {
            giantRb.isKinematic = false;
        }

        private void FixedUpdate()
        {
            if (giantRb.isKinematic ==false)
            {
                DetectWarriors();
                MoveAndAttack();
            }
        }

        private void DetectWarriors()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, warriorLayer);
            float closestDistance = Mathf.Infinity;
            Collider closestEnemy = null;

            foreach (Collider hitCollider in hitColliders)
            {
                float distance = Vector3.Distance(transform.position, hitCollider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = hitCollider;
                }
            }

            target = closestEnemy != null ? closestEnemy.transform : null;
        }

        private void MoveAndAttack()
        {
            if (target != null)
            {
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance > attackRadius)
                {
                    ChangeAnimationState(GIANT_RUN);
                    Vector3 direction = (target.position - transform.position).normalized;
                    giantRb.MovePosition(giantRb.position + direction * moveSpeed * Time.fixedDeltaTime);
                    RotateTowards(target.position);
                    giantRb.isKinematic = false;
                }
                else
                {
                    giantRb.velocity = Vector3.zero;
                    Attack();
                }
            }
        }
        
        void RotateTowards(Vector3 targetPosition)
        {
            Vector3 direction = targetPosition - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.DORotateQuaternion(lookRotation, 0.2f).SetEase(Ease.OutQuad);
        }

        private void Attack()
        {
            ChangeAnimationState(GIANT_ATTACK);
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                ChangeAnimationState(GIANT_DEATH);
                InternalEvents.EnemyDeath?.Invoke(Enums.EnemyType.Giant);
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
            if (currentState == newState) return;
            
            animator.Play(newState);
        }
    }
}