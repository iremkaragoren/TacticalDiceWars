using System;
using System.Collections;
using EventManager;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationHandler : MonoBehaviour
    {

        private Animator animator;
        private string currentState;
        private bool isBossDeath;
        
        private const string STICKMAN_IDLE = "Idle";
        private const string STICKMAN_RUN = "Run";
        private const string STICKMAN_STUMBLE = "Stumble";
        private const string STICKMAN_STACKING= "Stacking";
        
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }
        private void OnEnable()
        {
            ExternalEvents.LevelStart += OnLevelStart;
            ExternalEvents.DiceCollection += OnDiceCollection;
            ExternalEvents.DiceDecreaser += OnDiceDecreaser;
            InternalEvents.BattleArmyArea += OnBattleChoiserTrigger;
            InternalEvents.BossFightTriggered += OnBossFightDetector;
            InternalEvents.AllEnemyDeath += OnLevelContinue;
        }

        private void OnDisable()
        {
            ExternalEvents.LevelStart -= OnLevelStart;
            ExternalEvents.DiceCollection -= OnDiceCollection;
            ExternalEvents.DiceDecreaser -= OnDiceDecreaser;
            InternalEvents.BattleArmyArea -= OnBattleChoiserTrigger;
            InternalEvents.BossFightTriggered -= OnBossFightDetector;
            InternalEvents.AllEnemyDeath -= OnLevelContinue;
            
        }
        
        private void Update()
        {
            if (isBossDeath)
            {
                ChangeAnimationState(STICKMAN_IDLE);
            }
        }


        private void OnLevelContinue()
        {
            ChangeAnimationState(STICKMAN_RUN);
        }

        private void OnBossFightDetector()
        {
            isBossDeath = true;
        }
        
        

        private void OnLevelStart()
        {
            ChangeAnimationState(STICKMAN_RUN);
        }

        private void OnBattleChoiserTrigger(bool isBattleArea)
        {
            if (isBattleArea)
            {
                ChangeAnimationState(STICKMAN_IDLE);
            }
            
        }
        
        IEnumerator WaitForDeathAnimation()
        {
            ChangeAnimationState(STICKMAN_STUMBLE);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(STICKMAN_STUMBLE)
                                             && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            
            ChangeAnimationState(STICKMAN_RUN);
           
        }

        private void OnDiceDecreaser()
        {
            StartCoroutine(WaitForDeathAnimation());

        }
        
        private void OnDiceCollection()
        {
            ChangeAnimationState(STICKMAN_STACKING);
        }
        
       
        private void ChangeAnimationState(string newState)
        {
            if (currentState == newState) return;
            
            animator.Play(newState);
            currentState = newState;
        }
        
    }
}
