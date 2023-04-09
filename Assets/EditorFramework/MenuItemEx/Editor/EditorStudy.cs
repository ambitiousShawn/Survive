using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class Commons : EditorWindow
    {
        [MenuItem("编辑器笔记/笔记窗口 #/")]
        static void CustomWindows()
        {
            Commons common = GetWindow<Commons>();
            common.Show();
        }



        private void OnGUI()
        {
            _GUILayout();
            GUILayout.Label("-----------------------------");
            _EditorGUILayout();

        }

        string textFieldInfo = "单行输入框";
        string textAreaInfo = "多行输入框";
        bool toggleVal = true;

        float sliderMinVal = 0;
        float sliderMaxVal = 1;
        float sliderValForHorizontal = 0.5f;
        float sliderValForVertical = 0.6f;

        Vector2 pos = Vector2.zero;
        string pwd = "";

        private void _GUILayout()
        {
            /*
               GUILayout.Width，GUILayout.Height分别控制组件宽高，返回类型为GUILayoutOption
            */
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Label("这是一个文本");
                if (GUILayout.Button("按钮")) 
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<Object>("Assets/EditorFramework/MenuItemEx/Editor/EditorStudy.cs"));
                }
            }
            GUILayout.EndHorizontal();

            textFieldInfo = GUILayout.TextField(textFieldInfo, 5); //第二个参数是最大长度
            textAreaInfo = GUILayout.TextArea(textAreaInfo, 100);
            toggleVal = GUILayout.Toggle(toggleVal, "开关");

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("横向拖动条");
                GUILayout.Label(sliderMinVal.ToString(), GUILayout.Width(14));
                sliderValForHorizontal = GUILayout.HorizontalSlider(sliderValForHorizontal, sliderMinVal, sliderMaxVal);
                GUILayout.Label(sliderMaxVal.ToString(), GUILayout.Width(14));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                pos = GUILayout.BeginScrollView(pos, true, false, GUILayout.Width(200), GUILayout.Height(40));
                {
                    GUILayout.Label("lsadjalskfnsalkjfaslkhfalsk");
                }
                GUILayout.EndScrollView();
                GUILayout.BeginVertical();
                {
                    GUILayout.Label("纵向拖动条");
                    sliderValForVertical = GUILayout.VerticalSlider(sliderValForVertical, sliderMinVal, sliderMaxVal, GUILayout.Height(40));
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("请输入密码:");
                pwd = GUILayout.PasswordField(pwd, '*');
            }
            GUILayout.EndHorizontal();
        }

        int _count = 0;
        Bounds _bounds = new Bounds(Vector3.zero, Vector3.one);
        Color _color = Color.yellow;
        AnimationCurve _curve = AnimationCurve.Linear(0, 0, 24, 100);
        string _tag = "Player";
        int _layer = 0;
        int _counts = 1;
        Object _go = default;
        bool _foldoutList = false;
        bool _toggleGroup = false;
        string _textField = "string";
        float sliderVal = 0.5f;
        Vector2 v2 = default;
        Vector3 v3 = default;
        Vector4 v4 = default;
        bool fold = default;

        private void _EditorGUILayout()
        {
            _count = EditorGUILayout.Popup("下拉单选框", _count, new string[] { "Option1", "Option2", "Option3", "Option4" });
            _bounds = EditorGUILayout.BoundsField("边界输入框", _bounds);
            _color = EditorGUILayout.ColorField("颜色输入框", _color);
            _curve = EditorGUILayout.CurveField("曲线输入框", _curve);
            _tag = EditorGUILayout.TagField("标签输入框", _tag);
            _layer = EditorGUILayout.LayerField("图层输入框", _layer);
            _counts = EditorGUILayout.MaskField("下拉多选框", _counts, new string[] { "Option1", "Option2", "Option3", "Option4" });
            //Debug.Log(_counts); // 从前到后分别为 0,-1,1,2,4,8，打印输出的是和。
            _go = EditorGUILayout.ObjectField(_go, typeof(GameObject), true);
            _foldoutList = EditorGUILayout.Foldout(_foldoutList, "折叠块");
            if (_foldoutList)
            {
                EditorGUI.indentLevel++;//缩进级别
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "折叠块内容1");
                EditorGUI.indentLevel++;
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "折叠块内容2");
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "折叠块内容3");
            }

            _toggleGroup = EditorGUILayout.BeginToggleGroup("开关组", _toggleGroup);
            _textField = EditorGUILayout.TextField(_textField);
            GUILayout.Button("按钮1");
            EditorGUILayout.EndToggleGroup();

            //提示窗口
            EditorGUILayout.HelpBox("HelpBox Error", MessageType.Error, true);
            EditorGUILayout.HelpBox("HelpBox Warning", MessageType.Warning, true);
            EditorGUILayout.HelpBox("HelpBox None", MessageType.None);
            EditorGUILayout.HelpBox("HelpBox Info", MessageType.Info);

            sliderVal = EditorGUILayout.Slider(sliderVal, 0, 1);

            v2 = EditorGUILayout.Vector2Field("二维坐标", v2);
            v3 = EditorGUILayout.Vector3Field("三维坐标", v3);
            v4 = EditorGUILayout.Vector4Field("四维坐标", v4);

            EditorGUILayout.SelectableLabel("SelectableLabel"); //可以复制粘贴
            //fold = EditorGUILayout.InspectorTitlebar(fold, Selection.activeGameObject);
        }
    }

}