using EventManager;
using UnityEngine;

namespace UI
{
    public class NextLevelButton : MonoBehaviour
    {
        public void OnClick()
        {
            ExternalEvents. NextLevelButtonClicked?.Invoke();
        }

    }
}
