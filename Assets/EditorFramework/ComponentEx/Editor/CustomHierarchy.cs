using Shawn.ProjectFramework;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    /// <summary>
    /// ��Hierachy����չ���ߡ�
    /// </summary>
    public class CustomHierarchy
    {
        [MenuItem("CONTEXT/DialogueData_SO/���л�λ�ñ��")]
        static void SerializePos()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            DialogueData_SO select = (DialogueData_SO)tempObj;
            for (int pos = 0; pos < select.Nodes.Count; pos++)
            {
                select.Nodes[pos].Pos = pos;
            }
        }

        [MenuItem("CONTEXT/BuffData_SO/���л�λ�ñ��")]
        static void SerializeID()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            BuffData_SO select = (BuffData_SO)tempObj;
            for (int id = 0; id < select.Nodes.Count; id++)
            {
                select.Nodes[id].ID = id;
            }
        }

        [MenuItem("CONTEXT/InventoryData_SO/���л�λ�ñ��")]
        static void SerializeID2()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            InventoryData_SO select = (InventoryData_SO)tempObj;
            for (int id = 0; id < select.Nodes.Count; id++)
            {
                select.Nodes[id].ID = id;
            }
        }

        [MenuItem("CONTEXT/TipData_SO/���л�λ�ñ��")]
        static void SerializeID3()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            TipData_SO select = (TipData_SO)tempObj;
            for (int id = 0; id < select.Nodes.Count; id++)
            {
                select.Nodes[id].ID = id;
            }
        }

        [MenuItem("CONTEXT/CardData_SO/���л�λ�ñ��")]
        static void SerializeID4()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            CardData_SO select = (CardData_SO)tempObj;
            for (int id = 0; id < select.Nodes.Count; id++)
            {
                select.Nodes[id].ID = id;
            }
        }

        [MenuItem("CONTEXT/SkillData_SO/���л�λ�ñ��")]
        static void SerializeID5()
        {
            Object tempObj = EditorUtility.InstanceIDToObject(Selection.activeInstanceID);
            SkillData_SO select = (SkillData_SO)tempObj;
            for (int id = 0; id < select.Nodes.Count; id++)
            {
                select.Nodes[id].ID = id;
            }
        }
    }
}