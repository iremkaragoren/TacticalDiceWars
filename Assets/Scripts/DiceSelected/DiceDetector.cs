using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


public class DiceDetector : MonoBehaviour
{
    public List<Transform> diceTransformHolderList;

    [Button]
    public void GetAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            diceTransformHolderList.Add(transform.GetChild(i));
        }
    }
}