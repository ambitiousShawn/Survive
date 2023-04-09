using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using static UnityEngine.GUILayout;

namespace Shawn.EditorFramework
{
    public class UnusedResourcesWindow : EditorWindow
    {
        AssetCollector collector = new AssetCollector();

        ScriptsCollection collector2 = new ScriptsCollection();

        UIResCollection collector3 = new UIResCollection();

        UnusedUIPrefabCollection collector4 = new UnusedUIPrefabCollection();

        private static int FindType = 0;

        Vector2 scroll = default;

        public const string Check_Folder_Path = "Assets"; //待检查目录

        [MenuItem("Universal Tools/资源/01.查询未引用资源 %/", priority = -1)]
        static void CheckUnusedResource()
        {
            FindType = 0;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            UnusedResourcesWindow window = CreateInstance<UnusedResourcesWindow>();
            window.collector.Collection(new string[] { Check_Folder_Path });
            window.CopyDeleteFileList(window.collector.deleteFileList);
            window.Show();
        }

        [MenuItem("Universal Tools/资源/02.查询未引用脚本 %.",priority = -1)]
        static void CheckUnusedScripts()
        {
            FindType = 1;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            UnusedResourcesWindow window = CreateInstance<UnusedResourcesWindow>();
            window.collector2.CollectionScripts(new string[] { Check_Folder_Path });
            window.CopyDeleteScriptsList(window.collector2.unusedScriptsFiles);
            window.Show();
        }

        [MenuItem("Universal Tools/资源/03.查询所有UI预制体 #.", priority = -1)]
        static void CheckAllUIRes()
        {
            FindType = 2;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            UnusedResourcesWindow window = CreateInstance<UnusedResourcesWindow>();
            window.collector3.CollectionUIRes(new string[] { Check_Folder_Path });
            window.CopyUIResList(window.collector3.UIResPathList);
            window.Show();
        }

        [MenuItem("Universal Tools/资源/04.查询未引用的UI预制体 %,", priority = -1)]
        static void CheckUnusedUIPrefab()
        {
            FindType = 3;
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            UnusedResourcesWindow window = CreateInstance<UnusedResourcesWindow>();
            window.collector4.CollectionUIPrefab(new string[] { Check_Folder_Path });
            window.CopyUIPrefabList(window.collector4.prefabsPath);
            window.Show();
        }



        void OnGUI()
        {
            using (var horizontal = new EditorGUILayout.HorizontalScope("box"))
            {
                EditorGUILayout.LabelField("以下资源未使用(仅静态场景中未使用,谨慎使用删除操作)");
            }

            using (ScrollViewScope scrollScope = new ScrollViewScope(scroll))
            {
                scroll = scrollScope.scrollPosition;
                List<DeleteAssetData> tempList = new List<DeleteAssetData>() ;
                
                if (FindType == 0)
                {
                    foreach (DeleteAssetData data in deleteAssetCollection)
                    {
                        tempList.Add(data);
                        if (string.IsNullOrEmpty(data.filePath)) continue;
                        {
                            using (var horizontal = new EditorGUILayout.HorizontalScope())
                            {
                                Texture icon = AssetDatabase.GetCachedIcon(data.filePath);
                                Label(icon, Width(20), Height(20));
                                Label(data.filePath);
                                if (Button("选中", Width(40), Height(20)))
                                {
                                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(data.filePath);
                                }
                                if (Button("移除", Width(40), Height(20)))
                                {
                                    if (EditorUtility.DisplayDialog("", "是否确定删除该文件", "确定", "取消"))
                                    {
                                        tempList.Remove(data);
                                        AssetDatabase.DeleteAsset(data.filePath);
                                    }
                                }
                            }
                        }
                        deleteAssetCollection = tempList;
                    }
                }
                else if (FindType == 1)
                {
                    foreach (DeleteAssetData data in deleteScriptsCollection)
                    {
                        tempList.Add(data);
                        if (string.IsNullOrEmpty(data.filePath)) continue;
                        using (var horizontal = new EditorGUILayout.HorizontalScope())
                            {
                                Texture icon = AssetDatabase.GetCachedIcon(data.filePath);
                                Label(icon, Width(20), Height(20));
                                Label(data.filePath);
                                if (Button("选中", Width(40), Height(20)))
                                {
                                    Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(data.filePath);
                                }
                                if (Button("移除", Width(40), Height(20)))
                                {
                                    if (EditorUtility.DisplayDialog("", "是否确定删除该文件", "确定", "取消"))
                                    {
                                        tempList.Remove(data);
                                        AssetDatabase.DeleteAsset(data.filePath);
                                    }
                                }
                            }
                        
                        deleteScriptsCollection = tempList;
                    }
                }
                else if (FindType == 2)
                {
                    foreach (DeleteAssetData data in uiResCollection)
                    {
                        tempList.Add(data);
                        if (string.IsNullOrEmpty(data.filePath)) continue;
                        using (var horizontal = new EditorGUILayout.HorizontalScope())
                        {
                            Texture icon = AssetDatabase.GetCachedIcon(data.filePath);
                            Label(icon, Width(20), Height(20));
                            Label(data.filePath);
                            if (Button("选中", Width(40), Height(20)))
                            {
                                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(data.filePath);
                            }
                            if (Button("移除", Width(40), Height(20)))
                            {
                                if (EditorUtility.DisplayDialog("", "是否确定删除该文件", "确定", "取消"))
                                {
                                    tempList.Remove(data);
                                    AssetDatabase.DeleteAsset(data.filePath);
                                }
                            }
                        }

                        uiResCollection = tempList;
                    }
                }else if (FindType == 3)
                {
                    foreach (DeleteAssetData data in deleteUICollection)
                    {
                        tempList.Add(data);
                        if (string.IsNullOrEmpty(data.filePath)) continue;
                        using (var horizontal = new EditorGUILayout.HorizontalScope())
                        {
                            Texture icon = AssetDatabase.GetCachedIcon(data.filePath);
                            Label(icon, Width(20), Height(20));
                            Label(data.filePath);
                            if (Button("选中", Width(40), Height(20)))
                            {
                                Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(data.filePath);
                            }
                            if (Button("移除", Width(40), Height(20)))
                            {
                                if (EditorUtility.DisplayDialog("", "是否确定删除该文件", "确定", "取消"))
                                {
                                    tempList.Remove(data);
                                    AssetDatabase.DeleteAsset(data.filePath);
                                }
                            }
                        }

                        deleteUICollection = tempList;
                    }
                }
                
            }
        }

        private void CopyDeleteFileList(IEnumerable<string> deleteFileList)
        {
            deleteAssetCollection.Clear();
            foreach (string asset in deleteFileList)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(asset);

                if (!string.IsNullOrEmpty(filePath))
                {
                    DeleteAssetData tempDelData = new DeleteAssetData();
                    tempDelData.filePath = filePath;
                    deleteAssetCollection.Add(tempDelData);
                }
            }
        }

        List<DeleteAssetData> deleteScriptsCollection = new List<DeleteAssetData>();
        List<DeleteAssetData> uiResCollection = new List<DeleteAssetData>();
        List<DeleteAssetData> deleteAssetCollection = new List<DeleteAssetData>();
        List<DeleteAssetData> deleteUICollection = new List<DeleteAssetData>();

        private void CopyDeleteScriptsList(IEnumerable<string> deleteScriptsList)
        {
            deleteScriptsCollection.Clear();
            foreach (string path in deleteScriptsList) Debug.Log("Path + : <color=orange>" + path + "</color>");
            foreach (string asset in deleteScriptsList)
            {
                string filePath = asset;

                if (!string.IsNullOrEmpty(filePath))
                {
                    DeleteAssetData tempDelData = new DeleteAssetData();
                    tempDelData.filePath = filePath;
                    deleteScriptsCollection.Add(tempDelData);
                }
            }
        }

        private void CopyUIResList(IEnumerable<string> uiResList)
        {
            deleteScriptsCollection.Clear();
            foreach (string asset in uiResList)
            {
                string filePath = asset;

                if (!string.IsNullOrEmpty(filePath))
                {
                    DeleteAssetData tempDelData = new DeleteAssetData();
                    tempDelData.filePath = filePath;
                    uiResCollection.Add(tempDelData);
                }
            }
        }

        private void CopyUIPrefabList(IEnumerable<string> uiResList)
        {
            deleteUICollection.Clear();
            foreach (string asset in uiResList)
            {
                string filePath = asset;

                if (!string.IsNullOrEmpty(filePath))
                {
                    DeleteAssetData tempDelData = new DeleteAssetData();
                    tempDelData.filePath = filePath;
                    deleteUICollection.Add(tempDelData);
                }
            }
        }
    }
}