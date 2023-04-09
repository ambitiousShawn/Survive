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
            allScriptsPathWithUGUI = CollectionScriptsWithUGUI(targetFolder); //收集到所有UGUI相关的C#和lua脚本

            //foreach (string path in allScripts) Debug.Log("<color=orange>" + path + "</color>");
            //foreach (string path in allScriptsPathWithUGUI) Debug.Log("<color=orange>" + path + "</color>");
            EditorUtility.DisplayProgressBar("开始搜索文件", "正在搜索文件", 0.01f);
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
                    EditorUtility.DisplayProgressBar("正在扫描文件", "正在查找引用关系", (float)(allScripts.Count - j) * (allScriptsPathWithUGUI.Count - i) / (float) allScripts.Count * allScriptsPathWithUGUI.Count);
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
                            //Debug.Log("从" + allScriptName + "中排除了" + UGUIName);
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