using System;
using System.Collections;
using EventManager;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AttackerImageHandler : MonoBehaviour
    {
        [SerializeField] Image m_targetImage;
        [SerializeField] private AttackerData_SO m_attackerData;

        private int m_maxAttackDiceCount;
        private int m_attackDiceCount;
        private int m_range;
       

        private Coroutine coroutine;

        private void OnEnable()
        {
            InternalEvents.ProgressbarFiiled += OnProgressbarFiiled;
            InternalEvents.AttackerDiceCount += OnAttackDiceCount;
        }

        private void OnDisable()
        {
            InternalEvents.ProgressbarFiiled -= OnProgressbarFiiled;
            InternalEvents.AttackerDiceCount -= OnAttackDiceCount;
        }

        private void OnProgressbarFiiled()
        {
             m_targetImage.enabled = true;
        }
        

        private void OnAttackDiceCount(int attackDiceCount, int maxAttackDiceCount)
        {
            m_attackDiceCount = attackDiceCount;
            m_maxAttackDiceCount = maxAttackDiceCount;
            CalculateIntervalIndex();
        }
        
        private void CalculateIntervalIndex()
        {
            if (m_maxAttackDiceCount == 0) return;
            
            int rangeSize = m_maxAttackDiceCount / 3;
            m_range =( m_attackDiceCount - 1) / rangeSize + 1;
            
            Debug.Log("range" + m_range);
            
            TriggerActionBasedOnRange();
            
        }
        
        private void TriggerActionBasedOnRange()
        {
            Sprite selectedSprite = m_attackerData.Items[m_range-1].sprite;
            m_targetImage.sprite = selectedSprite;
            
            coroutine=StartCoroutine(Duration());
        }

        IEnumerator Duration()
        {
            yield return new WaitForSeconds(1f);
            InternalEvents.AttackerSprite?.Invoke(m_range-1);

            StopDuration();
        }

        private void StopDuration()
        {
            if(coroutine!=null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
        
        
    }
}
