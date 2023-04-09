using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class ScriptsCollection 
    {
        //查找脚本文件
        public List<string> unusedScriptsFiles = new List<string>();
        private List<string> excludeFiles = new List<string>();
        private Dictionary<string, string> classToFileMap = new Dictionary<string, string>();
        private Type[] ClassesFromAssembly;

        //初始化脚本文件映射
        private void InitMap()
        {
            ClassesFromAssembly = AssemblyManager.types;

            for (int i = unusedScriptsFiles.Count - 1; i >= 0; i--)
            {
                string filePath = unusedScriptsFiles[i];
                //string fileName = filePath.Substring(filePath.LastIndexOf('\\') + 1, filePath.LastIndexOf('.') - filePath.LastIndexOf('\\') - 1);
                string fileInfo = File.ReadAllText(filePath);

                foreach (Type type in ClassesFromAssembly)
                {
                    string className = type.Name;
                    if (!className.StartsWith("<"))
                    {
                        if (!classToFileMap.ContainsKey(className))
                            if (fileInfo.Contains("class " + className) || fileInfo.Contains("interface " + className))
                            {
                                //Debug.Log("<color=green>已将" + className + "_" + fileName + "</color>加入字典");
                                classToFileMap.Add(className, filePath);
                                continue;
                            }
                    }
                }
            }

            /*foreach (string key in classToFileMap.Keys)
            {
                Debug.Log(key + "_" + classToFileMap[key]);
            }*/
        }

        public void CollectionScripts(string[] targetFolder)
        {
            unusedScriptsFiles.Clear();
            excludeFiles.Clear();
            //EditorUtility.DisplayProgressBar("正在收集脚本资源", "正在收集脚本资源", 0.1f);

            unusedScriptsFiles = GetScriptFiles(targetFolder); //查找符合条件的所有文件路径
            InitMap();
            foreach (string path in unusedScriptsFiles) Debug.LogWarning(path);

            EditorUtility.DisplayProgressBar("正在排除脚本资源", "正在从场景中排除", 0.3f);
            ExcludeScriptsFromScene();
            EditorUtility.DisplayProgressBar("正在排除脚本资源", "正在排除非MonoBehaviour和编辑器脚本", 0.4f);
            ExcludeEditor();
            EditorUtility.DisplayProgressBar("正在排除脚本资源", "正在从预制体中排除", 0.5f);
            ExcludeScriptsFromPrefab();
            EditorUtility.DisplayProgressBar("正在排除脚本资源", "正在中排除基类脚本", 0.6f);
            ExcludeBaseScripts();


            foreach (string path in unusedScriptsFiles) Debug.LogError(path);
            EditorUtility.ClearProgressBar();
        }

        private void ExcludeBaseScripts()
        {

            for (int i = excludeFiles.Count - 1; i >= 0; i--)
            {
                string targetPath = excludeFiles[i];
                string targetName = targetPath.Substring(targetPath.LastIndexOf('\\') + 1, targetPath.LastIndexOf('.') - targetPath.LastIndexOf('\\') - 1);
                string targetText = File.ReadAllText(targetPath);
                foreach (Type type in ClassesFromAssembly)
                {
                    string typeName = type.Name;
                    if (!typeName.StartsWith("<") && targetText.Contains(typeName)
                        && classToFileMap.ContainsKey(typeName))
                    {
                        string filePath = classToFileMap[typeName];
                        if (!excludeFiles.Contains(filePath))
                        {
                            Debug.Log("<color=yellow>从" + targetPath + "中排除基类脚本" + filePath + "</color>");
                            ExcludeScripts(filePath);
                            classToFileMap.Remove(typeName);
                        }
                    }
                }
            }
        }

        private void ExcludeEditor()
        {
            for (int i = unusedScriptsFiles.Count - 1; i >= 0; i--)
            {
                string scriptInfo = File.ReadAllText(unusedScriptsFiles[i]);
                if (scriptInfo.Contains("using UnityEditor"))
                {
                    Debug.Log("编辑器排除" + unusedScriptsFiles[i]);
                    ExcludeScripts(unusedScriptsFiles[i]);
                }
            }
        }

        private void ExcludeScriptsFromPrefab()
        {
            string[] folderArr = new string[] { "Assets/Resources", "Assets/Prefab" };
            string[] prefabGuidList = AssetDatabase.FindAssets("t:prefab", folderArr); //找到所有预制体

            foreach (string guid in prefabGuidList)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(guid);
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                string info = File.ReadAllText(filePath);
                for (int i = unusedScriptsFiles.Count - 1; i > 0; i--)
                {
                    if (info.Contains(AssetDatabase.AssetPathToGUID(unusedScriptsFiles[i])))
                    {
                        Debug.Log("从预制体中排除" + unusedScriptsFiles[i]);
                        ExcludeScripts(unusedScriptsFiles[i]);
                    }
                }

            }
        }

        private List<string> GetScriptFiles(string[] targetFolder)
        {
            IEnumerable<string> files = targetFolder.SelectMany(
                c => Directory.GetFiles(c, "*.cs", SearchOption.AllDirectories)
                .Distinct());

            return files.ToList();
        }

        private void ExcludeScriptsFromScene()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(item => item.enabled == true)
                .Select(item => item.path)
                .ToArray();

            foreach (string path in AssetDatabase.GetDependencies(scenes, true))
            {
                string tempPath = path.Replace('/', '\\'); //大坑woc
                if (tempPath.EndsWith(".cs"))
                    Debug.Log("从场景中排除" + tempPath);
                ExcludeScripts(tempPath);

            }
        }

        private void ExcludeScripts(string path)
        {
            if (!unusedScriptsFiles.Contains(path)) return;
            unusedScriptsFiles.Remove(path);
            if (!excludeFiles.Contains(path))
                excludeFiles.Add(path);
        }
    }

}