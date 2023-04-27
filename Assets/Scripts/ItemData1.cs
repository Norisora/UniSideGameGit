using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ItemParam", menuName = "ScriptableObjects/CreateItemParam")]
public class ItemParam : ScriptableObject
{
    public enum Type
    {
        Heal,
        Weapon
    }
    [SerializeField]
    string name;
    [SerializeField]
    Type types;

    public string Name => name;

    public Type Types => types;

}