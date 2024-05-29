using System;
using EventManager;
using ScriptableObjects;
using UnityEngine;

namespace Platform
{
    public class BossFightDetector : MonoBehaviour
    {
        
        [SerializeField] private RectTransform panelRectTransform;
        private Vector2 panelSize;
        private Vector3 panelWorldPosition;
        
        private void OnEnable()
        {
            ExternalEvents.BossFightHandle += OnBossFightHandle;
        }
        
        private void OnBossFightHandle()
        {
            panelSize = new Vector2(panelRectTransform.rect.width, panelRectTransform.rect.height); 
            panelWorldPosition=panelRectTransform.position;
            InternalEvents.DiceThrowAreaSize?.Invoke(panelSize, panelWorldPosition);
            gameObject.SetActive(false);
        }
        
        private void OnDisable()
        {
            ExternalEvents.BossFightHandle -= OnBossFightHandle;
        }
    }
}
