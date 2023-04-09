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
    /// �����������ComponentEx�ű�
    /// </summary>
    public class UniversalTools
    {
        private const string rootPath = "Universal Tools/";

        #region Init Project
        private static string dataPath = Application.dataPath + "/";

        [MenuItem(rootPath + "�ļ���/01.��������Ŀ¼")]
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
            Debug.Log("����Ŀ¼�Ѵ�����");
        }

        [MenuItem(rootPath + "�ļ���/02.�Ƴ�Ŀ¼���ļ���")]
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
                Debug.Log("��ɾ���ļ����Լ���meta�ļ���" + rootPath.Substring(Application.dataPath.Length + 1));
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

        [MenuItem(rootPath + "��ҳ/01.API�ĵ�")]
        private static void OpenU3DDocs()
        {
            Application.OpenURL("https://docs.unity3d.com/cn/2020.2/ScriptReference/index.html");
        }

        [MenuItem(rootPath + "��ҳ/02.�ٶ�")]
        static void OpenBaidu()
        {
            Application.OpenURL("https://baidu.com");
        }

        [MenuItem(rootPath + "��ҳ/03.Bվ")]
        static void OpenBilibili()
        {
            Application.OpenURL("https://bilibili.com");
        }

        #endregion

        #region Batch Operation

        [MenuItem(rootPath + "��������/01.�������BoxCollider", priority = 0)]
        static void AddBoxCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("��ѡ��������Ӳ����ĸ�����");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                tran.GetOrAddComponent<BoxCollider>();
                //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                Debug.Log("����" + tran.gameObject.name + "�����BoxCollider�����");
            }
        }

        [MenuItem(rootPath + "��������/02.�����Ƴ�BoxCollider", priority = 0)]
        static void RemoveBoxCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("��ѡ�������Ƴ������ĸ�����");
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
                    Debug.Log("����" + tran.gameObject.name + "���Ƴ�BoxCollider�����");
                }

            }
        }

        [MenuItem(rootPath + "��������/03.�������MeshCollider", priority = 0)]
        static void AddMeshCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("��ѡ��������Ӳ����ĸ�����");
                return;
            }
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            foreach (Transform tran in trans)
            {
                if (tran == go.transform) continue;
                tran.GetOrAddComponent<MeshCollider>();
                //tran.gameObject.layer = LayerMask.NameToLayer("haha");
                Debug.Log("����" + tran.gameObject.name + "�����MeshCollider�����");
            }
        }

        [MenuItem(rootPath + "��������/04.�����Ƴ�MeshCollider", priority = 0)]
        static void RemoveMeshCollider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("��ѡ�������Ƴ������ĸ�����");
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
                    Debug.Log("����" + tran.gameObject.name + "���Ƴ�MeshCollider�����");
                }

            }
        }
        #endregion

        #region Open Unity Window
        [MenuItem(rootPath + "Unity����/01.Light")]
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

        [MenuItem(rootPath + "����/01.��ӵ�ǰ����", priority = 2)]
        static void AddSceneSettings()
        {
            Dictionary<string,EditorBuildSettingsScene> scenes = new Dictionary<string, EditorBuildSettingsScene>();

            foreach (EditorBuildSettingsScene v in EditorBuildSettings.scenes) scenes.Add(v.path, v);

            string currScenePath = SceneManager.GetActiveScene().path;
            if (!scenes.ContainsKey(currScenePath)) 
            {
                scenes.Add(currScenePath, new EditorBuildSettingsScene(currScenePath, true));
                EditorBuildSettings.scenes = scenes.Values.ToArray();
                Debug.Log("������ӳɹ�");
            }
            else
                Debug.Log("�����Ѵ���");
        }

        [MenuItem(rootPath + "����/02.���Ŀ¼���г���")]
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


            [MenuItem(rootPath + "����/03.�Ƴ�Ŀ¼���г���")] 
            public static void DeleteScenesFormBuildSettings() 
            {
                List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(); 
                EditorBuildSettings.scenes = scenes.ToArray(); 
            }

        #endregion
    }

}