using System;
using System.Collections;
using EventManager;
using ScriptableObjects;
using UnityEngine;

namespace Warrior
{
    public class AttackerSpawner : MonoBehaviour
    {
        [SerializeField] private RectTransform panelRectTransform;
        [SerializeField] private AttackerData_SO attackerData;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private PlayerData_SO playerDataSO;

        private GameObject spawnObject;
        
        private int attackerCount;
        private float width;
        private float panelWidth;
        private int totalSpawnedAttacker;
        private bool canAttackerSpawn = false;

        private void Awake()
        {
            panelWidth = panelRectTransform.rect.width;
        }

        private void OnEnable()
        {
            InternalEvents.AttackerSprite += OnAttackerSprite;
            InternalEvents.ProgressbarFiiled += OnProgressbarFiiled;
        }

        private void OnDisable()
        {
            InternalEvents.AttackerSprite -= OnAttackerSprite;
            InternalEvents.ProgressbarFiiled -= OnProgressbarFiiled;
        }

        private void OnProgressbarFiiled()
        {
            float zOffset = transform.position.z;
            SpawnWarriors(spawnObject, attackerCount, width, zOffset);
            InternalEvents.AttackerSpawned?.Invoke(spawnObject, attackerCount);
        }

        private void OnAttackerSprite(int rangeIndex)
        {
            spawnObject = attackerData.Items[rangeIndex].prefab;
            attackerCount = attackerData.Items[rangeIndex].count;
            width = attackerData.Items[rangeIndex].width;
        }

        private void SpawnWarriors(GameObject attackerPrefab, int totalNumber, float attackerWidth,
            float initialZOffset)
        {
            const int maxPerRow = 3;
            float zOffset = initialZOffset;

            int totalRows = Mathf.CeilToInt((float)totalNumber / maxPerRow);

            for (int row = 0; row < totalRows; row++)
            {
                int numberInThisRow = Mathf.Min(maxPerRow, totalNumber - (row * maxPerRow));
                float totalWidthThisRow = numberInThisRow * attackerWidth + (numberInThisRow - 1) * attackerWidth;
                float startPositionX = (panelWidth - totalWidthThisRow) / 2 - 2;

                for (int col = 0; col < numberInThisRow; col++)
                {
                    float xPosition = startPositionX + col * (attackerWidth + attackerWidth);
                    Vector3 spawnPosition = new Vector3(xPosition, 0.3f, zOffset);
                    Instantiate(attackerPrefab, spawnPosition, Quaternion.identity);
                }

                zOffset -= 1f;
            }
        }
    }
}