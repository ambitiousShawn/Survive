using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shawn.EditorFramework
{
    /// <summary>
    /// 该类深度依赖ComponentEx脚本
    /// </summary>
    public class UniversalTools
    {
        private const string rootPath = "Universal Tools/";

        #region Init Project
        private static string dataPath = Application.dataPath + "/";

        [MenuItem(rootPath + "文件夹/01.创建常用目录")]
        static void QuickConstruct()
        {
            CreateDirIfNot("Art");
            CreateDirIfNot("Resources");
            CreateDirIfNot("Audio");
            CreateDirIfNot("Scripts");
            CreateDirIfNot("StreamingAssets");
            CreateDirIfNot("Audio/BGM");
            CreateDirIfNot("Audio/Sound");
            CreateDirIfNot("SO");
            CreateDirIfNot("Prefab");
            AssetDatabase.Refresh();
            Debug.Log("常用目录已创建。");
        }

        [MenuItem(rootPath + "文件夹/02.移除目录空文件夹")]
        static void QuickRemoveEmptyDir()
        {
            Dfs(dataPath);
        }

        private static void Dfs(string rootPath)
        {
            if (!Directory.Exists(rootPath)) return;
            
            string[] childDirs = Directory.GetDirectories(rootPath);
            string[] childDocs = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly);

            if (childDirs.Length == 0 && childDocs.Length == 0)
            {
                File.Delete(rootPath + ".meta");
                Directory.Delete(rootPath);
                Debug.Log("已删除文件夹以及其meta文件：" + rootPath.Substring(Application.dataPath.Length + 1));
                Dfs(Directory.GetParent(rootPath).FullName);
                AssetDatabase.Refresh();
            } 
            
            foreach (string dir in childDirs) Dfs(dir);

        }

        private static void CreateDirIfNot (string dirName)
        {
            if (!Directory.Exists(dataPath + dirName)) Directory.CreateDirectory(dataPath + dirName);
        }
        #endregion

        #region Websites Starter

        [MenuItem(rootPath + "网页/01.API文档")]
        private static void OpenU3DDocs()
        {
            Application.OpenURL("https://docs.unity3d.com/cn/2020.2/ScriptReference/index.html");
        }

        [MenuItem(rootPath + "网页/02.百度")]
        static void OpenBaidu()
        {
            Application.OpenURL("https://baidu.com");
        }

        [MenuItem(rootPath + "网页/03.B站")]
        static void OpenBilibili()
        {
            Application.OpenURL("https://bilibili.com");
        }

        #endregion

        #region Batch Operation

        [MenuItem(rootPath + "批量操作/01.批量添加BoxCollider", priority = 0)]
        static void AddBoxCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选择批量添加操作的父对象！");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                tran.GetOrAddComponent<BoxCollider>();
                //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                Debug.Log("已在" + tran.gameObject.name + "上添加BoxCollider组件。");
            }
        }

        [MenuItem(rootPath + "批量操作/02.批量移除BoxCollider", priority = 0)]
        static void RemoveBoxCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选择批量移除操作的父对象！");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                BoxCollider boxCollider = tran.GetOrAddComponent<BoxCollider>();
                if (boxCollider != null)
                {
                    GameObject.DestroyImmediate(boxCollider);
                    //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                    Debug.Log("已在" + tran.gameObject.name + "上移除BoxCollider组件。");
                }

            }
        }

        [MenuItem(rootPath + "批量操作/03.批量添加MeshCollider", priority = 0)]
        static void AddMeshCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选择批量添加操作的父对象！");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                tran.GetOrAddComponent<MeshCollider>();
                //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                Debug.Log("已在" + tran.gameObject.name + "上添加MeshCollider组件。");
            }
        }

        [MenuItem(rootPath + "批量操作/04.批量移除MeshCollider", priority = 0)]
        static void RemoveMeshCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("请选择批量移除操作的父对象！");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                MeshCollider meshCollider = tran.GetOrAddComponent<MeshCollider>();
                if (meshCollider != null)
                {
                    GameObject.DestroyImmediate(meshCollider);
                    //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                    Debug.Log("已在" + tran.gameObject.name + "上移除MeshCollider组件。");
                }

            }
        }
        #endregion

        #region Open Unity Window
        [MenuItem(rootPath + "Unity窗口/01.Light")]
        static void OpenLightingWindow()
        {
            FocusWindow("LightingWindow", "CreateLightingWindow");
        }

        private static Assembly editorAssembly = Assembly.GetAssembly(typeof(Editor));
        private static void FocusWindow(string windowClassName, string createMethodName)
        {
             var type = editorAssembly.GetTypes()
                .Where(t => t.Name == windowClassName)
                .FirstOrDefault();

            Object[] wins = Resources.FindObjectsOfTypeAll(type);
            EditorWindow win = wins.Length > 0 ? (EditorWindow)(wins[0]) : null;
            if (win != null)
            {
                win.Show();
                win.Focus();
            }
            var flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            var method = type.GetMethod(createMethodName, flags);
            if (method != null) method.Invoke(null, null);
        }
        #endregion

        #region Scene Manager

        public const string scenePath = "Scenes";

        [MenuItem(rootPath + "场景/01.添加当前场景", priority = 2)]
        static void AddSceneSettings()
        {
            Dictionary<string,EditorBuildSettingsScene> scenes = new Dictionary<string, EditorBuildSettingsScene>();

            foreach (EditorBuildSettingsScene v in EditorBuildSettings.scenes) scenes.Add(v.path, v);

            string currScenePath = SceneManager.GetActiveScene().path;
            if (!scenes.ContainsKey(currScenePath)) 
            {
                scenes.Add(currScenePath, new EditorBuildSettingsScene(currScenePath, true));
                EditorBuildSettings.scenes = scenes.Values.ToArray();
                Debug.Log("场景添加成功");
            }
            else
                Debug.Log("场景已存在");
        }

        [MenuItem(rootPath + "场景/02.添加目录所有场景")]
        public static void AddAllScenesToBuildSettings()
        {
            HashSet<string> sceneNames = new HashSet<string>();
            string[] paths = new string[] { "Assets" };
            string[] sceneArr = AssetDatabase.FindAssets("t:Scene", paths);  
            
            foreach(string scene in sceneArr)        
            {           
                sceneNames.Add(AssetDatabase.GUIDToAssetPath(scene));         
            }

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();         

            foreach(string scensName in sceneNames)        
            {                      
                scenes.Add(new EditorBuildSettingsScene(scensName, true));        
            }        

            EditorBuildSettings.scenes = scenes.ToArray();    
        }


            [MenuItem(rootPath + "场景/03.移除目录所有场景")] 
            public static void DeleteScenesFormBuildSettings() 
            {
                List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(); 
                EditorBuildSettings.scenes = scenes.ToArray(); 
            }

        #endregion
    }

}