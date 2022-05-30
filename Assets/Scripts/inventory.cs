using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventory : MonoBehaviour
{
    [SerializeField]
    item[] contents;
    // Start is called before the first frame update
    void Start()
    {
        foreach(var item in contents)
        {
            Debug.Log($"Has item: {item.getName()}.");
        }
    }
}
