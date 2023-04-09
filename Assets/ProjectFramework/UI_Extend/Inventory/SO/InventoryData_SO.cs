using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shawn.ProjectFramework
{

    [CreateAssetMenu(fileName = "Inventory_SO", menuName = "SO/Create Inventory")]
    public class InventoryData_SO : ScriptableObject
    {
        public List<ItemNode> Nodes;
    }

    public enum E_ItemType
    {
        Common
    }

    [Serializable]
    public class ItemNode
    {
        public int ID;
        public E_ItemType Type;
        public string Name;
        public string Describtion;
    }
}