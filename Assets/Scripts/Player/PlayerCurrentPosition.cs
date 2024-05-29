using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;

public class PlayerCurrentPosition : MonoBehaviour
{
    [SerializeField] private PlayerData_SO playerData;
    
    private void Update()
    {
        playerData.PlayerPosition = transform.position;
    }
}
