// using System;
// using System.Collections;
// using General;
// using TMPro;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace UI
// {
//     public class FightButton : MonoBehaviour
//     {
//
//         private Button m_fightButton;
//         
//
//         private void Awake()
//         {
//             m_fightButton = GetComponent<Button>();
//         }
//
//         private void OnEnable()
//         {
//             ExternalEvents.RunButtonClicked += OnRunButtonClicked; 
//             
//         }
//
//         private void OnRunButtonClicked()
//         {
//             m_fightButton.interactable = true;
//           
//         }
//
//         private void OnDisable()
//         {
//             ExternalEvents.RunButtonClicked -= OnRunButtonClicked;
//         }
//
//         public void OnClick()
//         {
//            gameObject.SetActive(false);
//         }
//         
//
//        
//     }
// }
