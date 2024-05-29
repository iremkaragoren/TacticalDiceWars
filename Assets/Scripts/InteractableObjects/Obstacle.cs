using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using DG;
using UnityEngine.Serialization;

namespace InteractableObjects
{
    public class Obstacle : InteractableBase
    {
        [SerializeField] private float m_duration;

        

        // private void Start()
        // {
        //      MovementType();
        // }
        //
        // private void MovementType()
        // {
        //     if (ObjectType == Enums.ObjectType.Saw)
        //     {
        //         RotationToAround();
        //     }
        //
        //     if (ObjectType == Enums.ObjectType.CircularSaw)
        //     {
        //         RotationToAround();
        //         
        //
        //             
        //             float currentY = transform.position.y;
        //             float currentZ = transform.position.z;
        //             float targetX = transform.position.x > 0 ? -1 : 1; 
        //
        //             
        //             Vector3 targetPosition = new Vector3(targetX, currentY, currentZ);
        //         
        //            // transform.DOMove(targetPosition, m_duration).SetLoops(-1, LoopType.Yoyo);
        //         
        //         
        //        
        //     }
        // }
        //
        // private void RotationToAround()
        // {
        //     transform.DORotate(new Vector3(0, 0, 360), m_duration)
        //         .SetEase(Ease.Linear)
        //         .SetLoops(-1, LoopType.Restart);
        // }
    }
}
