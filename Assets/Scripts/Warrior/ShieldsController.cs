using System;
using System.Collections;
using EventManager;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class ShieldsController : MonoBehaviour,IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip hitTriggeredClip;
        [SerializeField] private WarriorData_SO shields_SO;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private float detectionRadius;
        
        [SerializeField] private MeshRenderer[] childRenderers;
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;
        
        private Coroutine corountine;
        private Animator animator;
        private Transform target;
        
        private string currentState;
        private float currentHealth;

        private const string SHIELD_IDLE = "Idle";
        private const string SHIELD_PUNCH = "Punch";
        private void Awake()
        {
            animator = GetComponent<Animator>();
            currentHealth = shields_SO.MaxHealth;
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

        private void OnAllEnemyDeath()
        {
            Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
        }

        private void FixedUpdate()
        {
            DetectEnemyAndAttack();
        }

        private void DetectEnemyAndAttack()
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

            if (closestEnemy != null && closestDistance <= detectionRadius)
            {
                target = closestEnemy.transform;
                PlayDiceTriggeredSound();
                ChangeAnimationState(SHIELD_PUNCH);
            }
            else
            {
                ChangeAnimationState(SHIELD_IDLE);
                target = null;
            }
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
        }
    }
}
