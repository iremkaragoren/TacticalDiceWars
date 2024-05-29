using System;
using System.Net.Mime;
using DG.Tweening;
using EventManager;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
namespace UI
{
    public class TotalScoreAssigner : MonoBehaviour
    {
        [SerializeField] private Text totalScoreText;
        [SerializeField] private Image diceImage;
        [SerializeField] private PlayerData_SO playerData;

        private void Awake()
        {
            ExternalEvents.DiceCollection += OnDiceCollection;
            ExternalEvents.DiceDecreaser += OnDiceDecrease;
            ExternalEvents.IndexIncreaseAction += OnDiceDecreaseByButton;
            InternalEvents.BattleArmyArea += OnBattleArmyArea;
            InternalEvents.AllEnemyDeath += OnAllEnemyDeath;
            InternalEvents.AttackerDiceCount += OnAttackerDiceCount;
        }
        
        private void OnDisable()
        {
            ExternalEvents.DiceCollection -= OnDiceCollection;
            ExternalEvents.DiceDecreaser -= OnDiceDecrease;
            ExternalEvents.IndexIncreaseAction -= OnDiceDecreaseByButton;
            InternalEvents.BattleArmyArea -= OnBattleArmyArea;
            InternalEvents.AllEnemyDeath -= OnAllEnemyDeath;
            InternalEvents.AttackerDiceCount -= OnAttackerDiceCount;
        }

        private void OnAttackerDiceCount(int count1, int count2)
        {
            this.gameObject.SetActive(false);
        }

        private void OnAllEnemyDeath()
        {
            diceImage.enabled = true;
            totalScoreText.enabled = true;
        }

        private void OnBattleArmyArea(bool isBattleArea)
        {
            diceImage.enabled = !isBattleArea;
            totalScoreText.enabled = !isBattleArea;
        }

        private void OnDiceDecreaseByButton()
        {
            totalScoreText.text = playerData.StackableCount.ToString();
        }

        private void OnDiceDecrease()
        {
            totalScoreText.text = playerData.StackableCount.ToString();
        }

        private void OnDiceCollection()
        {
            totalScoreText.text = playerData.StackableCount.ToString();
        }
        
    }
}
