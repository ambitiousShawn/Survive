using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class UnusedUIPrefabCollection 
    {
        public List<string> allScripts;
        public List<string> allScriptsPathWithUGUI;
        private List<string> excludeFiles = new List<string>();
        public List<string> prefabsPath = new List<string>();

        public void CollectionUIPrefab(string[] targetFolder)
        {
            allScripts = CollectionAllScripts(targetFolder);
            allScriptsPathWithUGUI = CollectionScriptsWithUGUI(targetFolder); //�ռ�������UGUI��ص�C#��lua�ű�

            //foreach (string path in allScripts) Debug.Log("<color=orange>" + path + "</color>");
            //foreach (string path in allScriptsPathWithUGUI) Debug.Log("<color=orange>" + path + "</color>");
            EditorUtility.DisplayProgressBar("��ʼ�����ļ�", "���������ļ�", 0.01f);
            ExcludeUnusedUGUIScriptsFromAll();

            MappingScriptToPrefab(targetFolder);
            EditorUtility.ClearProgressBar();
            //foreach (string path in allScriptsPathWithUGUI) Debug.LogError(path);
        }

        private void MappingScriptToPrefab(string[] targetFolder)
        {
            foreach (string path in allScriptsPathWithUGUI)
            {
                string fileName = Path.GetFileName(path);
                fileName = fileName.Substring(0, fileName.IndexOf('.'));

                List<string> prefabs = targetFolder.SelectMany(
                    c => Directory.GetFiles(c, "*.prefab", SearchOption.AllDirectories))
                    .Where(item => Path.GetFileName(item).Contains(fileName)).ToList();

                foreach (string prefabPath in prefabs)
                {
                    prefabsPath.Add(prefabPath);
                }
            }
        }

        private void ExcludeUnusedUGUIScriptsFromAll()
        {
            for (int i = allScriptsPathWithUGUI.Count - 1;i >= 0; i--)
            {
                string UGUIPath = allScriptsPathWithUGUI[i];
                string UGUIName = Path.GetFileName(UGUIPath);
                UGUIName = UGUIName.Substring(0, UGUIName.IndexOf('.'));

                for (int j = allScripts.Count - 1;j >= 0; j--)
                {
                    EditorUtility.DisplayProgressBar("����ɨ���ļ�", "���ڲ������ù�ϵ", (float)(allScripts.Count - j) * (allScriptsPathWithUGUI.Count - i) / (float) allScripts.Count * allScriptsPathWithUGUI.Count);
                    string allScriptsPath = allScripts[j];
                    string allScriptsName = Path.GetFileName(allScriptsPath);
                    allScriptsName = allScriptsName.Substring(0, allScriptsName.IndexOf('.'));
                    string allScriptInfo = File.ReadAllText(allScriptsPath);

                    if (UGUIName == allScriptsName) continue;

                    if (excludeFiles.Contains(UGUIPath))
                    {
                        break;
                    }
                    else
                    {
                        if (allScriptInfo.Contains(UGUIName))
                        {
                            //Debug.Log("��" + allScriptName + "���ų���" + UGUIName);
                            excludeFiles.Add(UGUIPath);
                            allScriptsPathWithUGUI.Remove(UGUIPath);
                        }
                    }
                    
                }
            }
        }

        private List<string> CollectionAllScripts(string[] targetFolder)
        {
            IEnumerable<string> files = targetFolder.SelectMany(
                c => Directory.GetFiles(c, "*.*", SearchOption.AllDirectories))
                .Distinct()
                .Where(item => Path.GetExtension(item) == ".cs" || Path.GetExtension(item) == ".lua");
            return files.ToList();
        }

        private List<string> CollectionScriptsWithUGUI(string[] targetFolder)
        {
            IEnumerable<string> files = targetFolder.SelectMany(
                c => Directory.GetFiles(c, "*.*", SearchOption.AllDirectories))
                .Distinct()
                .Where(item => Path.GetExtension(item) == ".cs" || Path.GetExtension(item) == ".lua")
                .Where(item => Path.GetFileName(item).StartsWith("UGUI_"));

            return files.ToList();
        }
    }

}