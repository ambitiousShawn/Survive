using System.IO;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class SearchResroucesWindow : EditorWindow
    {
        [MenuItem("Universal Tools/资源/03.通过GUID查找资源 #/", false, 0)]
        static void FindAssetsByGUID()
        {
            EditorWindow window = EditorWindow.GetWindowWithRect(typeof(SearchResroucesWindow), new Rect(100, 100, 600, 170));
            window.maxSize = new Vector2(650, 900);
        }

        private string targetGUID;
        private bool searchResult;
        private string targetPath;

        private void OnEnable()
        {
            targetGUID = "Please Click this area input GUID";
            searchResult = false;
            targetPath = string.Empty;
        }

        private void OnGUI()
        {
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUI.skin.label.fontSize = 18;
            GUILayout.Label("FindAssetByGUID");
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Please Input GUID witch you want to search.");
                EditorGUILayout.BeginHorizontal();
                {
                    targetGUID = EditorGUILayout.TextField("GUID:", targetGUID);
                    if (GUILayout.Button("Search"))
                    {
                        SearchAssetByGUID(targetGUID);
                    }
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("Search Result:");
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                if (!searchResult)
                {
                    EditorGUILayout.HelpBox("No matching file resource was found", MessageType.Info);
                }
                else
                {
                    if (targetPath != string.Empty)
                    {
                        Object target = AssetDatabase.LoadAssetAtPath<Object>(targetPath);
                        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
                        {
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.LabelField(target.name, EditorStyles.boldLabel);
                                EditorGUILayout.LabelField(targetPath);
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.BeginVertical();
                            {
                                EditorGUILayout.Space();
                                target = EditorGUILayout.ObjectField(target, typeof(Object), false);
                            }
                            EditorGUILayout.EndVertical();
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void SearchAssetByGUID(string targetGUID)
        {
            if (!Directory.Exists(Application.dataPath)) return;

            string[] filesPath = Directory.GetFiles(Application.dataPath, "*.*", SearchOption.AllDirectories);
            for (int i = 0;i < filesPath.Length;i++)
            {
                bool cancel = EditorUtility.DisplayCancelableProgressBar("查找资源中：" + (i).ToString() + "/" + filesPath.Length, filesPath[i], (float)i / filesPath.Length);
                if (cancel)
                {
                    EditorUtility.ClearProgressBar();
                    return;
                }

                if (filesPath[i].EndsWith(".meta")) continue;
                string filePath = filesPath[i];
                filePath = filePath.Substring(filePath.IndexOf("Assets")).Replace('\\', '/');

                Object obj = AssetDatabase.LoadAssetAtPath<Object>(filePath);

                if (obj == null) continue;

                if (AssetDatabase.AssetPathToGUID(filePath) == targetGUID)
                {
                    searchResult = true;
                    targetPath = filePath;
                    if (Selection.activeGameObject == null)
                        Selection.activeGameObject = obj as GameObject ;
                    continue;
                }
            }
            Debug.Log("查询已完成！");
            Resources.UnloadUnusedAssets();
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
            EditorUtility.ClearProgressBar();
        }
    }

}