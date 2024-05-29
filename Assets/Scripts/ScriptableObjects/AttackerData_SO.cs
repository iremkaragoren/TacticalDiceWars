using UnityEngine;

namespace ScriptableObjects{
    
    [CreateAssetMenu(fileName = "AttackerData_SO", menuName = "ThisGame/Variable/AttackerData", order = 3)]
    public class AttackerData_SO : ScriptableObject
    {
        [SerializeField] SpritePrefabPair[] items;

        public SpritePrefabPair[] Items => items;
    }
    
    [System.Serializable]
    public class SpritePrefabPair
    {
        public Sprite sprite;
        public GameObject prefab;
        public int count;
        public GameObject bullet;
        public float width;
        
    }
}
