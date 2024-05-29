using System;
using System.Collections;
using System.Collections.Generic;
using DiceSelected;
using EventManager;
using General;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class WarriorButton : MonoBehaviour, IClickable
    {
        [SerializeField] private Enums.WarriorType warriorType;
        [SerializeField] private TextMeshProUGUI diceCounter;
        [SerializeField] private DiceDetector diceDetector;
        [SerializeField] private bool isButtonMinus;
        [SerializeField] private PlayerData_SO playerData;

        private Collider collider;
        private int currentStack;
        private bool isInArea;
        private bool onClick;

        public Enums.WarriorType WarriorType => warriorType;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (isInArea)
            {
                currentStack = playerData.StackableCount;
            }
        }

        private void OnEnable()
        {
            InternalEvents.BattleArmyArea += OnBattleArmyArea;
        }

        private void OnDisable()
        {
            InternalEvents.BattleArmyArea -= OnBattleArmyArea;
        }

        private void OnBattleArmyArea(bool battleArea)
        {
            isInArea = battleArea;
        }

        public void OnClick()
        {
            if (onClick)
            {
                return;
            }

            Debug.Log("click");
            int currentIndex = int.Parse(diceCounter.text.ToString());

            if (isButtonMinus)
            {
                if (currentIndex > 0)
                {
                    Transform diceTransform = diceDetector.diceTransformHolderList[currentIndex - 1].GetChild(0);
                    InternalEvents.ObjectTriggeredAction?.Invoke(Enums.ObjectType.Dice, 1, diceTransform);
                    currentIndex--;
                    diceCounter.text = currentIndex.ToString();
                }
            }
            else
            {
                if (currentStack > 0)
                {
                    Debug.Log("");
                    Transform currentTransform = diceDetector.diceTransformHolderList[currentIndex];
                    ExternalEvents.WarriorsButtonAction?.Invoke(currentTransform, isButtonMinus, warriorType);

                    currentIndex++;
                    diceCounter.text = currentIndex.ToString();
                    ExternalEvents.DiceDecreaser?.Invoke();
                }
            }

            StartCoroutine(ClickDuration());
        }

        IEnumerator ClickDuration()
        {
            onClick = true;

            yield return new WaitForSeconds(0.1f);

            onClick = false;
        }
    }
}


