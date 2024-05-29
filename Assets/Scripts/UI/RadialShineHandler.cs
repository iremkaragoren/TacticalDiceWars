using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using EventManager;
using UnityEngine;
using UnityEngine.UI;

public class RadialShineHandler : MonoBehaviour
{
    [SerializeField] List<Transform> radialShineObject;

    private void OnEnable()
    {
        ExternalEvents.RetrySceneActivated += OnFightText;
    }

    private void OnDisable()
    {
        ExternalEvents.RetrySceneActivated -= OnFightText;

    }

    private void OnFightText()
    {
       
        StartCoroutine(RadialNumberDelay());
    }


    IEnumerator RadialNumberDelay()
    {

        foreach (Transform radialElement in radialShineObject)
        {
         
            Image image = radialElement.GetComponent<Image>();
            if (image != null) 
            {
               
                image.enabled = true;

               
                yield return new WaitForSeconds(.5f);

               
                image.enabled = false;
            };

        }
      

    }

    [Button]
    public void GetAllChildren()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            radialShineObject.Add(transform.GetChild(i));
          
        }

    }

}
