using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using ScriptableObjects;


namespace InteractableObjects
{
    public class Stackable :InteractableBase
    {
        [SerializeField] private PlayerData_SO m_playerData;
        
        
        [SerializeField] private int m_maxHorizontalStack = 3; 
        [SerializeField] private float m_verticalSpacing = 1.0f; 
        [SerializeField] private float m_horizontalSpacing = 2.0f;
        private Vector3 m_stackOffset = new Vector3(-0.2f, 0.5f, -1);

        private List<GameObject> m_stackingObjects = new List<GameObject>();
        private int m_currentColumn = 0;

        [SerializeField] private float m_duration;

        

        protected override void SendInteractableAmountAndType()
        {
            base.SendInteractableAmountAndType();
            int totalObject = m_stackingObjects.Count;
            int rowCount = totalObject / m_maxHorizontalStack;
            int columnCount = totalObject % m_maxHorizontalStack;


            transform.DOMove(GetPositionToNewObject(rowCount, columnCount), m_duration)
                .OnComplete(() =>
                {
                    transform.DOKill();
                });
        }


        private Vector3 GetPositionToNewObject(int rowIndex, int colomnIndex)
        {
            Vector3 stackPosition = m_playerData.PlayerPosition - m_stackOffset;
        
            float xPosition = stackPosition.x + (rowIndex* m_horizontalSpacing);
            float yPosition = stackPosition.y + (colomnIndex * m_verticalSpacing);

            return new Vector3(xPosition, yPosition, stackPosition.z);
        }
        
        
        
        
        
        
        
        
    }
}
