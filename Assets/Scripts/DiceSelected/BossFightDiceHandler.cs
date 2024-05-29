using System.Collections;
using System.Collections.Generic;
using EventManager;
using ScriptableObjects;
using TMPro;
using UnityEngine;


namespace DiceSelected
{
    public class BossFightDiceHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI attackerDiceCounter;
        [SerializeField] private PlayerData_SO playerDataSo;

        private int attackerDiceUpperCounter = 0;
        private int totalDiceCount = 0;
        private int diceCounter = 0;

        private void Awake()
        {
            InternalEvents.BossFightDiceOnGround += OnBossDiceRolled;
            InternalEvents.ProgressbarFiiled += OnDestroyAllChild;
        }

        private void OnDisable()
        {
            InternalEvents.BossFightDiceOnGround -= OnBossDiceRolled;
            InternalEvents.ProgressbarFiiled -= OnDestroyAllChild;
        }

        private void OnDestroyAllChild()
        {
            Destroy(attackerDiceCounter);
        }

        private void OnBossDiceRolled(List<Transform> diceList)
        {
            totalDiceCount = diceList.Count * 6;
            foreach (var diceTransform in diceList)
            {
                DetectUpperSide detectUpperSide = diceTransform.GetComponent<DetectUpperSide>();

                if (detectUpperSide != null)
                {
                    detectUpperSide.UpdateUpperNumber();
                    attackerDiceUpperCounter += detectUpperSide.UpperSideNumber;
                }
            }
            StartCoroutine(DestroyCounterTextPanel());
        }

        IEnumerator DestroyCounterTextPanel()
        {
            attackerDiceCounter.text = attackerDiceUpperCounter.ToString();
            Debug.Log(attackerDiceUpperCounter);
            InternalEvents.AttackerDiceCount?.Invoke(attackerDiceUpperCounter, totalDiceCount);

            yield return new WaitForSeconds(1f);
        }
    }
}