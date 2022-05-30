using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Unnamed Item", menuName = "Inventory Item")]
public class item : ScriptableObject
{
    [SerializeField]
    string itemName;
    [SerializeField]
    string description;

    public string getName()
    {
        return itemName;
    }
}
