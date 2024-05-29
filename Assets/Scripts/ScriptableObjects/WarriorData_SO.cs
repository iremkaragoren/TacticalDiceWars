using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "WarriorData_SO", menuName = "ThisGame/Variable/WarriorData", order = 2)]
   
public class WarriorData_SO : ScriptableObject
{
    [SerializeField] private float maxHealth;
    [SerializeField] private float width;
    [SerializeField] private float warriorOffset;
    [SerializeField] private GameObject warriorPrefab;

    public GameObject WarriorPrefab => warriorPrefab;

    public float Width => width;
    public float WarriorOffset => warriorOffset;

    public float MaxHealth
    {
        get => maxHealth;
        set => maxHealth = value;
    }
}



