using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Shawn.EditorFramework
{

    public class AssetCollector
    {
        public List<string> deleteFileList = new List<string>(); //待删除文件列表
        public List<CollectionData> referenceCollection = new List<CollectionData>(); //文件的引用相关数据集
        private List<string> ignoreExtension = new List<string>() { ".bytes", ".xml", ".json", ".txt", ".csv", "lua", ".proto", "montage", "asset" };

        public void Collection(string[] collectionFolders)
        {
            deleteFileList.Clear();
            referenceCollection.Clear();

            EditorUtility.DisplayProgressBar("正在收集资源", "正在收集资源", 0.4f);

            List<string> files = GetAllCheckFiles(collectionFolders); //查找符合条件的所有文件路径

            foreach (string path in files)
            {
                deleteFileList.Add(AssetDatabase.AssetPathToGUID(path));
            }


            EditorUtility.DisplayProgressBar("检查是否引用", "排除部分后缀名", 0.3f);
            UnregisterIgnoreExtension();

            EditorUtility.DisplayProgressBar("检查是否引用", "排除场景引用", 0.5f);
            UnregisterReferenceFromScene();

            EditorUtility.DisplayProgressBar("检查是否引用", "排除部分预制体引用", 0.8f);
            UnregisterPrefab();

            EditorUtility.ClearProgressBar();
        }
        public class ABCDEFG
        {

        }

        private List<string> GetAllCheckFiles(string[] collectionFolders)
        {
            IEnumerable<string> files = collectionFolders.SelectMany(
                c => Directory.GetFiles(c, "*.*", SearchOption.AllDirectories))
                .Distinct()
                .Where(item => Path.GetExtension(item) != ".meta")
                .Where(item => Path.GetExtension(item) != ".js")
                .Where(item => Path.GetExtension(item) != ".dll")
                .Where(item => Path.GetExtension(item) != ".bytes")
                .Where(item => Path.GetExtension(item) != ".json")
                .Where(item => Path.GetExtension(item) != ".csv")
                .Where(item => Path.GetExtension(item) != ".txt")
                .Where(item => Path.GetExtension(item) != ".xml")
                .Where(item => Path.GetExtension(item) != ".cs")
                .Where(item => Regex.IsMatch(item, "[\\/\\\\]Gizmos[\\/\\\\]") == false)
                .Where(item => Regex.IsMatch(item, "[\\/\\\\]Plugins[\\/\\\\]Android[\\/\\\\]") == false)
                .Where(item => Regex.IsMatch(item, "[\\/\\\\]Plugins[\\/\\\\]iOS[\\/\\\\]") == false)
                .Where(item => Regex.IsMatch(item, "[\\/\\\\]Resources[\\/\\\\]") == false);

            return files.ToList();
        }

        /// <summary>
        /// 忽略部分扩展名
        /// </summary>
        private void UnregisterIgnoreExtension()
        {
            List<string> tempDeleteList = new List<string>();
            for (int i = 0; i < deleteFileList.Count; i++)
            {
                if (deleteFileList[i] != null)
                {
                    //拿到对应文件的扩展名
                    string extension = Path.GetExtension(Path.GetFileName(AssetDatabase.GUIDToAssetPath(deleteFileList[i])));
                    if (extension != null && ignoreExtension.Contains(extension.ToLower()))
                    {
                        tempDeleteList.Add(deleteFileList[i]);
                    }
                }
            }

            foreach (string item in tempDeleteList)
            {
                UnregisterFromDeleteList(item);
            }
        }

        /// <summary>
        /// 忽略场景引用
        /// </summary>
        private void UnregisterReferenceFromScene()
        {
            string[] scenes = EditorBuildSettings.scenes
                .Where(item => item.enabled == true)
                .Select(item => item.path)
                .ToArray();

            foreach (string path in AssetDatabase.GetDependencies(scenes, true))
            {
                UnregisterFromDeleteList(AssetDatabase.AssetPathToGUID(path));
            }
        }
        private void UnregisterPrefab()
        {
            string[] folderArr = new string[] { "Assets/Resources", "Assets/Prefab" };
            string[] prefabGidList = AssetDatabase.FindAssets("t:prefab", folderArr); //找到所有预制体

            foreach (string gid in prefabGidList)
            {
                string filePath = AssetDatabase.GUIDToAssetPath(gid);
                GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(filePath);
                Image[] img = obj.GetComponentsInChildren<Image>();
                foreach (Image im in img)
                {
                    string path = AssetDatabase.GetAssetPath(im.sprite.GetInstanceID());
                    UnregisterFromDeleteList(AssetDatabase.AssetPathToGUID(path));
                }
            }
        }

        private void UnregisterFromDeleteList(string guid)
        {
            if (!deleteFileList.Contains(guid)) return;
            deleteFileList.Remove(guid);

            if (referenceCollection.Exists(c => c.fileGuid == guid))
            {
                CollectionData refInfo = referenceCollection.First(c => c.fileGuid == guid);
                foreach (string refGuid in refInfo.referenceGids)
                {
                    UnregisterFromDeleteList(refGuid);
                }
            }
        }
    }




}