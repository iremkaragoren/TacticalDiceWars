using System.Collections;
using EventManager;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
    public class WarriorDiceText : MonoBehaviour
    {
        [SerializeField] private PlayerData_SO playerData;
        [SerializeField] private Image diceImage;
        [SerializeField] private Text totalScoreText;

        private void OnEnable()
        {
            ExternalEvents.DiceCollection += OnDiceCollection;
            ExternalEvents.IndexIncreaseAction += OnDiceDecreaseByButton;
            InternalEvents.BattleArmyArea += OnBattleArmyArea;
            ExternalEvents.RunButtonClicked += OnRunButtonClicked;
        }
        private void OnDisable()
        {
            ExternalEvents.DiceCollection -= OnDiceCollection;
            ExternalEvents.IndexIncreaseAction -= OnDiceDecreaseByButton;
            InternalEvents.BattleArmyArea -= OnBattleArmyArea;
            ExternalEvents.RunButtonClicked -= OnRunButtonClicked;
        }

        private void OnRunButtonClicked()
        {
            gameObject.SetActive(false);
        }

        private void OnBattleArmyArea(bool isBattleArea)
        {
            StartCoroutine(UIDuration());
        }

        IEnumerator UIDuration()
        {
            yield return new WaitForSeconds(1f);
            diceImage.enabled = true;
            totalScoreText.enabled = true;
            totalScoreText.text = playerData.StackableCount.ToString();
        }

        private void OnDiceDecreaseByButton()
        {
            totalScoreText.text = playerData.StackableCount.ToString();
        }

        private void OnDiceCollection()
        {
            totalScoreText.text = playerData.StackableCount.ToString();
        }
        
    }
}
