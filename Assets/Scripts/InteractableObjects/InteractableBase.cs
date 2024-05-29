using System;
using EventManager;
using UnityEngine;

namespace InteractableObjects
{
    public class InteractableBase:MonoBehaviour,Interactable
    {
        [SerializeField] private Enums.ObjectType m_objectType;
        [SerializeField] private int m_amount;
        
        public Enums.ObjectType ObjectType => m_objectType;
        public int Amount => m_amount;

        private bool m_isTriggered;

        

        protected  virtual void SendInteractableAmountAndType()
        {
            InternalEvents.ObjectTriggeredAction.Invoke(ObjectType,Amount,transform);
            
            m_isTriggered = true;
        }

        public void Interact()
        {
            if (m_isTriggered)
                return;
            
            SendInteractableAmountAndType();
            
        }

        
    }
}