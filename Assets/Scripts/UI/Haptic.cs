using UnityEngine;

namespace UI
{
    public class Haptic : MonoBehaviour
    { 
        public void Vibrate()
        {
            Vibrator.Vibrate(1500);
        }
    }
}
