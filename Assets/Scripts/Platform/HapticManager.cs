using System;
using EventManager;
using UnityEngine;

public class HapticManager : MonoBehaviour
{
    private bool canHaptic;
    private void OnEnable()
    {
        ExternalEvents.RunButtonClicked += OnRunButtonClicked;
    }

    private void OnDisable()
    {
        ExternalEvents.RunButtonClicked -= OnRunButtonClicked;
      
    }
    
    private void OnRunButtonClicked()
    {
        TriggerHapticFeedback();
    }


    private  void TriggerHapticFeedback()
    {
        
            if (Application.platform == RuntimePlatform.Android)
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    var vibratorService = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");

                    int sdkInt = new AndroidJavaClass("android.os.Build$VERSION").GetStatic<int>("SDK_INT");
                    if (sdkInt >= 26) 
                    {
                        using (var vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect"))
                        {
                        
                            var createOneShot = vibrationEffectClass.CallStatic<AndroidJavaObject>("createOneShot", 1000, -1); // DEFAULT_AMPLITUDE için -1 kullanılır
                            vibratorService.Call("vibrate", createOneShot);
                        }
                    }
                    else
                    {
                    
                        vibratorService.Call("vibrate", 500);
                    }
                }
            }
        
        
    }
}