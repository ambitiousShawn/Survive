using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string name;
    public int size;
    public string description;

    public Item(string name, int size, string description)
    {
        this.name = name;
        this.size = size;
        this.description = description;
    }
}
