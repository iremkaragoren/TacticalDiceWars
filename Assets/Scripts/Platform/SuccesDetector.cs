using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuccesDetector : MonoBehaviour, Interactable
{
    public void Interact()
    {
        EventManager.ExternalEvents.LevelSucces?.Invoke();
    }
}
