using System.Collections;
using EventManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
   public class FightImageAssigner : MonoBehaviour
   {
     [SerializeField] private Image m_fightImage;

      private void OnEnable()
      {
         ExternalEvents.DiceTotalCountDone += OnDiceTotalCounterDone;
      }

      private void OnDisable()
      {
         ExternalEvents.DiceTotalCountDone -= OnDiceTotalCounterDone;
      }

      private void OnDiceTotalCounterDone()
      {
         StartCoroutine(FightTextDuration());
      }
   
      IEnumerator FightTextDuration()
      {
         m_fightImage.enabled = true;
         yield return new WaitForSeconds(2.5f);

         ExternalEvents.FightTextAssigned?.Invoke();
         Destroy(this.gameObject);
        
   
      }
   }
}
