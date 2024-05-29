using System;
using EventManager;
using UnityEngine;

namespace UI
{
    public class ChosingWarriorArea:MonoBehaviour
    {
      
        private void OnEnable()
        {
            ExternalEvents.WarriorsSpawned += OnFightButtonClicked;
        }

        private void OnFightButtonClicked()
        {
            Destroy(this.gameObject);
        }

        private void OnDisable()
        {
            ExternalEvents.WarriorsSpawned -= OnFightButtonClicked;
        }
    }

   
}