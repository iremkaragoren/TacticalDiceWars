using System;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using ScriptableObjects;
using UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemy
{
    public class BossHealthHandler : MonoBehaviour, IDamageble
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip spawnTriggeredClip;
        [SerializeField] private AudioClip deathTriggeredClip;
        [SerializeField] private AttackerData_SO attackerData;
        [SerializeField] private string PlatformLayer = "Platform";
        [SerializeField] private Healthbar healthbar;
        
        private float maxHealth = 1;
        private float totalAttackerCount;
        private float currentHealth = 1;
        private string currentState;
        private int currentAnimationIndex;
        private bool isSpawned;

        private Animator animator;
        private Coroutine coroutine;
        
        private const string BOSS_DIZZY = "Dizzy";
        private const string BOSS_GO = "Go";
        private const string BOSS_JUMP = "Jump";
        private const string BOSS_DEATH = "Death";
        private const string BOSS_DAMAGE = "Damage";
        
        private string[] initialAnimations = { BOSS_JUMP, BOSS_DIZZY, BOSS_GO };
        
        [SerializeField] private SkinnedMeshRenderer[] childSkinMeshRenderer;

        [Button]
        public void GetAllChildrensMesh()
        {
            childSkinMeshRenderer = GetComponentsInChildren<SkinnedMeshRenderer>();
        }
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
            healthbar.UpdateHealthBar(maxHealth, currentHealth);
        }

        private void OnEnable()
        {
            InternalEvents.BossSpawned += OnBossSpawned;
            InternalEvents.AttackerSprite += OnAttackerSprite;
        }

        private void OnDisable()
        {
            InternalEvents.BossSpawned -= OnBossSpawned;
            InternalEvents.AttackerSprite -= OnAttackerSprite;
        }

        private void OnAttackerSprite(int rangeIndex)
        {
            totalAttackerCount = attackerData.Items[rangeIndex].count * 4;
            Debug.Log(totalAttackerCount + " health");
        }

        private void OnBossSpawned()
        {
            PlayDiceTriggeredSound();
            isSpawned = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            int platformLayerIndex = LayerMask.NameToLayer(PlatformLayer);

            if (other.gameObject.layer == platformLayerIndex)
            {
                PlayDeathTriggeredSound();
            }
        }

        private void PlayDiceTriggeredSound()
        {
            if (audioSource != null && spawnTriggeredClip != null)
            {
                audioSource.PlayOneShot(spawnTriggeredClip);
            }
        }

        private void PlayDeathTriggeredSound()
        {
            if (audioSource != null && deathTriggeredClip != null)
            {
                audioSource.PlayOneShot(deathTriggeredClip);
            }
        }

        private void Update()
        {
            if (isSpawned)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !animator.IsInTransition(0))
                {
                    if (currentAnimationIndex < initialAnimations.Length)
                    {
                        ChangeAnimationState(initialAnimations[currentAnimationIndex]);
                        currentAnimationIndex++;
                    }
                }
            }
        }

        public void TakeDamage(float amount)
        {
            currentHealth -= (1 / totalAttackerCount);

            ChangeAnimationState(BOSS_DAMAGE);
            healthbar.UpdateHealthBar(maxHealth, currentHealth);

            // Renk değişimini her hasar alındığında tetiklemek için
            ChangeMeshColor(Color.red);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                ChangeAnimationState(BOSS_DEATH);
                StartCoroutine(WaitForDeathAnimation());
            }
        }

        IEnumerator WaitForDeathAnimation()
        {
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(BOSS_DEATH)
                                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 &&
                                             !animator.IsInTransition(0));
            InternalEvents.BossDeath?.Invoke();
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
            currentState = newState;
        }
    }
}
