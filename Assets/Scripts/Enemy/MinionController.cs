using System.Collections;
using DG.Tweening;
using EventManager;
using ScriptableObjects;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class MinionController : MonoBehaviour,IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitTriggeredClip;
        [SerializeField] private EnemyData_SO minion_SO;
        [SerializeField] private LayerMask warriorLayer;
        [SerializeField] private float detectionRadius;
        [SerializeField] private float attackRadius;
        [SerializeField] private float moveSpeed;
        
        private Animator animator;
        private Transform target;
        private Rigidbody minionRb;
        private Coroutine corountine;
        
        private float currentHealth;
        private string currentState;
        
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;
        
        private const string MINION_IDLE = "Idle";
        private const string MINION_RUN= "Run";
        private const string MINION_ATTACK= "Attack";
        private const string MINION_DEATH= "Death";
        
        private void Awake()
        {
            
            animator = GetComponent<Animator>();
            minionRb = GetComponent<Rigidbody>(); 
            currentHealth = minion_SO.MaxHealth;
            ChangeAnimationState(MINION_IDLE);
            
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
           Destroy(this);
        }

        private void OnFightTextAssigned()
        {
            minionRb.isKinematic = false;
        }

        private void FixedUpdate()
        {
            if (minionRb.isKinematic ==false)
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

                RotateTowards(target.position);

                if (distance <= attackRadius)
                {
                    
                    minionRb.velocity = Vector3.zero;
                    Attack();
                }
                else
                {
                    
                    ChangeAnimationState(MINION_RUN);
                    Vector3 direction = (target.position - transform.position).normalized;
                    minionRb.MovePosition(minionRb.position + direction * moveSpeed * Time.fixedDeltaTime);
                    minionRb.isKinematic = false; 
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
            ChangeAnimationState(MINION_ATTACK);
            PlayDiceTriggeredSound();
        }
        
        private void PlayDiceTriggeredSound()
        {
            if (audioSource != null && hitTriggeredClip != null)
            {
                audioSource.PlayOneShot(hitTriggeredClip);
            }
        }


        public void TakeDamage(float amount)
        {
            currentHealth -= amount;

            if (currentHealth <= 0)
            {
                ChangeAnimationState(MINION_DEATH);
                InternalEvents.EnemyDeath?.Invoke(Enums.EnemyType.Minion);
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