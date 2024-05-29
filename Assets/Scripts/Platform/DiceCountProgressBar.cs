using System.Collections;
using System.Collections.Generic;
using EventManager;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Platform
{
    public class DiceCountProgressBar : MonoBehaviour
    {
        [SerializeField] private Image progressBarImage;
        [SerializeField] private GameObject progressBar;

        private float m_minValue = 0.0f;
        private int m_maxValue;
        private float currentValue;
        

        private void OnEnable()
        {
            InternalEvents.AttackerDiceCount += OnBossDiceBarCounter;
           
        }
        
        private void OnDisable()
        {
            InternalEvents.AttackerDiceCount -= OnBossDiceBarCounter;
           
        }
        

        private void OnBossDiceBarCounter(int progressBarCounter,int maxDiceSide)
        {
                progressBar.SetActive(true);
                m_maxValue = maxDiceSide;
                StartCoroutine(FillBar(progressBarCounter));
        }

        
        private IEnumerator FillBar(int targetValue)
        {
            float duration = 1f; 
            float startValue = currentValue; 
            float timer = 0.0f; 

            while (timer < duration)
            {
                timer += Time.deltaTime;
                currentValue = Mathf.Lerp(startValue, targetValue, timer / duration);
                progressBarImage.fillAmount = (currentValue - m_minValue) / (m_maxValue - m_minValue);
                yield return null;
            }

            currentValue = targetValue; 
            progressBarImage.fillAmount = (currentValue - m_minValue) / (m_maxValue - m_minValue);

            yield return new WaitForSeconds(1f);
            
            InternalEvents.ProgressbarFiiled?.Invoke();
            progressBar.SetActive(false);
            
            
        }

    }
}