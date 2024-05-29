using System.Collections;
using System.Collections.Generic;
using EventManager;
using TMPro;
using UnityEngine;

namespace General
{
    public class WarriorSpawner : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private WarriorData_SO swordsmenData;
        [SerializeField] private WarriorData_SO archerData;
        [SerializeField] private WarriorData_SO shieldData;

        public TextMeshProUGUI swordText;
        public TextMeshProUGUI archerText;
        public TextMeshProUGUI shieldText;
        
        private int totalSpawnedWarriors = 0;

        private Dictionary<Enums.WarriorType, WarriorInfo> warriorInfoDictionary = new Dictionary<Enums.WarriorType, WarriorInfo>();
        
        private float m_panelWidth;

        private Coroutine coroutine;
        private void Awake()
        {
            InitializeWarriorInfo();
            m_panelWidth = panelRectTransform.rect.width;
        }

        private void OnEnable()
        {
            InternalEvents.DiceRollEnd += OnDiceRolled;
            InternalEvents.WarriorDeath += OnWarriorDeath;
            ExternalEvents.FightTextAssigned += OnFightTextAssigned;
            ExternalEvents.LevelGO += OnLevelLoaded; 
        }
        
        

        private void OnDisable()
        {
            InternalEvents.DiceRollEnd -= OnDiceRolled;
            InternalEvents.WarriorDeath -= OnWarriorDeath;
            ExternalEvents.FightTextAssigned -= OnFightTextAssigned;
            ExternalEvents.LevelGO -= OnLevelLoaded; 
           
        }

        private void OnLevelLoaded(GameObject levelGO)
        {
            ResetWarriorSpawner();
        }
        
        public void ResetWarriorSpawner()
        {
            totalSpawnedWarriors = 0;
            foreach (var key in warriorInfoDictionary.Keys)
            {
                warriorInfoDictionary[key].TotalNumber = 0;
                warriorInfoDictionary[key].Spawned = false;
                warriorInfoDictionary[key].Text.text = "0";
            }
        }

        private void OnFightTextAssigned()
        {
            float zOffset = transform.position.z;
    
            foreach (var warriorInfo in warriorInfoDictionary)
            {
                if (!warriorInfo.Value.Spawned)
                {
                    SpawnWarriors(warriorInfo.Value.Data.WarriorPrefab, warriorInfo.Value.TotalNumber,
                        warriorInfo.Value.Data, zOffset);
            
                    warriorInfo.Value.Spawned = true; 
            
                    zOffset -= 1f;
            
                    ExternalEvents.WarriorsSpawned?.Invoke();
                }
            }
        }


        private void OnWarriorDeath()
        {
            totalSpawnedWarriors--;

            if (totalSpawnedWarriors <= 0)
            {
                InternalEvents.RetryAction?.Invoke();
            }
        }
        

        private void InitializeWarriorInfo()
        {
            warriorInfoDictionary.Add(Enums.WarriorType.Shield, new WarriorInfo(shieldData, shieldText));
            warriorInfoDictionary.Add(Enums.WarriorType.Swordsmen, new WarriorInfo(swordsmenData, swordText));
            warriorInfoDictionary.Add(Enums.WarriorType.Archer, new WarriorInfo(archerData, archerText));
        }

        private void OnDiceRolled(Enums.WarriorType diceType, int diceNumber)
        {
            if (warriorInfoDictionary.TryGetValue(diceType, out WarriorInfo info))
            {
                info.TotalNumber += diceNumber;
                info.Text.text = info.TotalNumber.ToString();
                
            }
            ExternalEvents.DiceTotalCountDone?.Invoke();
            
        }
        
        

        private void SpawnWarriors(GameObject warriorPrefab, int totalNumber, WarriorData_SO data, float initialZOffset)
        {
           
            const int maxPerRow = 4; 
            float zOffset = initialZOffset;
            
            int totalRows = Mathf.CeilToInt((float)totalNumber / maxPerRow);

           
            for (int row = 0; row < totalRows; row++)
            {
                int numberInThisRow = Mathf.Min(maxPerRow, totalNumber - (row * maxPerRow));
                float totalWidthThisRow = numberInThisRow * data.Width + (numberInThisRow - 1) * data.WarriorOffset;
                float startPositionX = (m_panelWidth - totalWidthThisRow) / 2 - m_panelWidth / 2;

                for (int col = 0; col < numberInThisRow; col++)
                {
                    totalSpawnedWarriors++;
                    float xPosition = startPositionX + col * (data.Width + data.WarriorOffset);
                    Vector3 spawnPosition = new Vector3(xPosition, 0.3f, zOffset);
                    Instantiate(warriorPrefab, spawnPosition, Quaternion.identity);
                }
                
                zOffset -= 1f; 
            }
            
        }

        
        private class WarriorInfo
        {
            public WarriorData_SO Data;
            public TextMeshProUGUI Text;
            public int TotalNumber;
            public bool Spawned = false; 

            public WarriorInfo(WarriorData_SO data, TextMeshProUGUI text)
            {
                Data = data;
                Text = text;
                TotalNumber = 0;
            }
        }
    }
}
