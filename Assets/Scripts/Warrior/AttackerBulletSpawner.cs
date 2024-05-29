using System.Collections;
using EventManager;
using ScriptableObjects;
using UnityEngine;

namespace Warrior
{
    public class AttackerBulletSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private Transform shootPoint;
        
        private bool isThrowable = true;
        private int bulletCount = 4;
        
        private Animator animator;
        
        private const string ATTACKER_ATTACK = "Attack";
        private string currentState;
        
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            InternalEvents.AttackerSpawned += OnCatapultSpawned;
            InternalEvents.AttackerDiceCount += OnBossHealthCount;
        }

        private void OnDisable()
        {
            InternalEvents.AttackerSpawned -= OnCatapultSpawned;
            InternalEvents.AttackerDiceCount -= OnBossHealthCount;
        }

        private void OnBossHealthCount(int upperSide, int healthCount)
        {
            // Debug.Log("1");
            // bulletCount = healthCount;
            // Debug.Log(bulletCount);
        }

        private void OnCatapultSpawned(GameObject attacker, int attackerCount)
        {
            if (isThrowable)
            {
                StartCoroutine(LaunchRocksRoutine());
                isThrowable = false;
            }
        }

        private IEnumerator LaunchRocksRoutine()
        {
            for (int i = 0; i < bulletCount; i++)
            {
                Instantiate(bullet, shootPoint.position, Quaternion.Euler(-90, 0, 0));
                ChangeAnimationState(ATTACKER_ATTACK);
                InternalEvents.BulletSpawned?.Invoke();
                yield return new WaitForSeconds(1.2f);
            }

            this.enabled = false;
        }
        
        
        private void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;
            
            animator.Play(newState);
        }
    }

}