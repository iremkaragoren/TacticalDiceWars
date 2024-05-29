using System;
using System.Collections.Generic;
using UnityEngine;

namespace DiceSelected
{
    public class DetectUpperSide : MonoBehaviour
    {
        public Vector3Int DirectionValues;
        private Vector3Int OpposingDirectionValues;
        
        public int UpperSideNumber { get; private set; }

        readonly List<string> _faceRepresent = new List<string>() {"", "1", "2", "3", "4", "5", "6"};

        private void Start()
        {
            OpposingDirectionValues = 7 * Vector3Int.one - DirectionValues;
        }



        public void UpdateUpperNumber()
        {
            string upperSideText = "";

            if (Vector3.Cross(Vector3.up, transform.right).magnitude < 0.5f)
            {
                if (Vector3.Dot(Vector3.up, transform.right) > 0)
                {
                    upperSideText = _faceRepresent[DirectionValues.x];
                }
                else
                {
                    upperSideText = _faceRepresent[OpposingDirectionValues.x];
                }
            }
            else if (Vector3.Cross(Vector3.up, transform.up).magnitude < 0.5f)
            {
                if (Vector3.Dot(Vector3.up, transform.up) > 0)
                {
                    upperSideText = _faceRepresent[DirectionValues.y];
                }
                else
                {
                    upperSideText = _faceRepresent[OpposingDirectionValues.y];
                }
            }
            else if (Vector3.Cross(Vector3.up, transform.forward).magnitude < 0.5f)
            {
                if (Vector3.Dot(Vector3.up, transform.forward) > 0)
                {
                    upperSideText = _faceRepresent[DirectionValues.z];
                }
                else
                {
                    upperSideText = _faceRepresent[OpposingDirectionValues.z];
                }
            }

            if (!string.IsNullOrEmpty(upperSideText))
            {
                UpperSideNumber = int.Parse(upperSideText);
            }
        }
    }
}
