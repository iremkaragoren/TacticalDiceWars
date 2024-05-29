using EventManager;
using UnityEngine;

namespace UI
{
   public class RetryButton : MonoBehaviour
   {
      
      public void OnClick()
      {
         ExternalEvents.RetryButtonClicked?.Invoke();
      }



      
   }
}
