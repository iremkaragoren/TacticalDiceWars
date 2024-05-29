using System;
using EventManager;
using General;
using ScriptableObjects;
using UnityEngine;

namespace DiceSelected
{
    public class RunButton:MonoBehaviour,IClickable
    {

        [SerializeField] private PlayerData_SO playerData;
        
        private int stackCount;
        private int previousStackCount;
       

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
            stackCount = playerData.StackableCount;
            
            previousStackCount = stackCount;
            
        }

        public void OnClick()
        {
            stackCount = playerData.StackableCount;

            
            if (stackCount != previousStackCount)
            {
                ExternalEvents.RunButtonClicked?.Invoke();
                
                previousStackCount = stackCount;
            }
            
        }
    }
}