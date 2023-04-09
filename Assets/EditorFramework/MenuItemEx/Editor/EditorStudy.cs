using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Shawn.EditorFramework
{

    public class Commons : EditorWindow
    {
        [MenuItem("�༭���ʼ�/�ʼǴ��� #/")]
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

        string textFieldInfo = "���������";
        string textAreaInfo = "���������";
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
               GUILayout.Width��GUILayout.Height�ֱ���������ߣ���������ΪGUILayoutOption
            */
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Label("����һ���ı�");
                if (GUILayout.Button("��ť")) 
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<Object>("Assets/EditorFramework/MenuItemEx/Editor/EditorStudy.cs"));
                }
            }
            GUILayout.EndHorizontal();

            textFieldInfo = GUILayout.TextField(textFieldInfo, 5); //�ڶ�����������󳤶�
            textAreaInfo = GUILayout.TextArea(textAreaInfo, 100);
            toggleVal = GUILayout.Toggle(toggleVal, "����");

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("�����϶���");
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
                    GUILayout.Label("�����϶���");
                    sliderValForVertical = GUILayout.VerticalSlider(sliderValForVertical, sliderMinVal, sliderMaxVal, GUILayout.Height(40));
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label("����������:");
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
            _count = EditorGUILayout.Popup("������ѡ��", _count, new string[] { "Option1", "Option2", "Option3", "Option4" });
            _bounds = EditorGUILayout.BoundsField("�߽������", _bounds);
            _color = EditorGUILayout.ColorField("��ɫ�����", _color);
            _curve = EditorGUILayout.CurveField("���������", _curve);
            _tag = EditorGUILayout.TagField("��ǩ�����", _tag);
            _layer = EditorGUILayout.LayerField("ͼ�������", _layer);
            _counts = EditorGUILayout.MaskField("������ѡ��", _counts, new string[] { "Option1", "Option2", "Option3", "Option4" });
            //Debug.Log(_counts); // ��ǰ����ֱ�Ϊ 0,-1,1,2,4,8����ӡ������Ǻ͡�
            _go = EditorGUILayout.ObjectField(_go, typeof(GameObject), true);
            _foldoutList = EditorGUILayout.Foldout(_foldoutList, "�۵���");
            if (_foldoutList)
            {
                EditorGUI.indentLevel++;//��������
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "�۵�������1");
                EditorGUI.indentLevel++;
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "�۵�������2");
                EditorGUI.indentLevel--;
                EditorGUI.indentLevel--;
                _foldoutList = EditorGUILayout.Foldout(_foldoutList, "�۵�������3");
            }

            _toggleGroup = EditorGUILayout.BeginToggleGroup("������", _toggleGroup);
            _textField = EditorGUILayout.TextField(_textField);
            GUILayout.Button("��ť1");
            EditorGUILayout.EndToggleGroup();

            //��ʾ����
            EditorGUILayout.HelpBox("HelpBox Error", MessageType.Error, true);
            EditorGUILayout.HelpBox("HelpBox Warning", MessageType.Warning, true);
            EditorGUILayout.HelpBox("HelpBox None", MessageType.None);
            EditorGUILayout.HelpBox("HelpBox Info", MessageType.Info);

            sliderVal = EditorGUILayout.Slider(sliderVal, 0, 1);

            v2 = EditorGUILayout.Vector2Field("��ά����", v2);
            v3 = EditorGUILayout.Vector3Field("��ά����", v3);
            v4 = EditorGUILayout.Vector4Field("��ά����", v4);

            EditorGUILayout.SelectableLabel("SelectableLabel"); //���Ը���ճ��
            //fold = EditorGUILayout.InspectorTitlebar(fold, Selection.activeGameObject);
        }
    }

}