using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EquipmentItemData), true)]
public class ItemDataEditor : Editor
{
    private SerializedProperty idProp;
    private SerializedProperty nameProp;
    private SerializedProperty descProp;
    private SerializedProperty iconProp;
    private SerializedProperty typeProp;
    private SerializedProperty valueProp;
    private SerializedProperty equipTypeProp;
    private SerializedProperty itemStatusProp;

    private static Texture2D _checkerTexture;

    private void OnEnable()
    {
        idProp = serializedObject.FindProperty("Id");
        nameProp = serializedObject.FindProperty("Name");
        descProp = serializedObject.FindProperty("Description");
        iconProp = serializedObject.FindProperty("IconImage");
        typeProp = serializedObject.FindProperty("Type");
        valueProp = serializedObject.FindProperty("Value");
        equipTypeProp = serializedObject.FindProperty("EquipmentType");
        itemStatusProp = serializedObject.FindProperty("ItemStatus");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // 상단 타이틀 라인
        EditorGUILayout.BeginVertical("box");
        GUILayout.Label("Item Info", EditorStyles.boldLabel);

        // 텍스트 영역
        EditorGUILayout.BeginHorizontal("box");
        if (iconProp.objectReferenceValue != null)
        {
            Texture iconTex = ((Sprite)iconProp.objectReferenceValue).texture;
            GUILayout.Label(iconTex, GUILayout.Width(64), GUILayout.Height(64));
        }
        else
        {
            GUILayout.Box("No Icon", GUILayout.Width(64), GUILayout.Height(64));
        }

        EditorGUILayout.BeginVertical();
        GUILayout.Space(4);

        EditorGUILayout.PropertyField(idProp, new GUIContent("Index"));
        EditorGUILayout.PropertyField(nameProp, new GUIContent("Item Name"));
        EditorGUILayout.PropertyField(descProp, new GUIContent("Description"));

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(5);

        // 다른 필드들 섹션
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Basic Info", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(iconProp, new GUIContent("Icon Image"));
        EditorGUILayout.PropertyField(typeProp, new GUIContent("Type"));
        EditorGUILayout.PropertyField(valueProp, new GUIContent("Value"));
        EditorGUILayout.PropertyField(equipTypeProp, new GUIContent("Equipment Type"));
        EditorGUILayout.PropertyField(itemStatusProp, new GUIContent("ItemStatus"));

        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    private void EnsureCheckerTexture()
    {
        if (_checkerTexture != null)
            return;

        // 2x2 checker pattern 생성
        _checkerTexture = new Texture2D(2, 2);
        Color c0 = new Color(0.8f, 0.8f, 0.8f); // 밝은 회색
        Color c1 = new Color(0.6f, 0.6f, 0.6f); // 어두운 회색
        _checkerTexture.SetPixels(new Color[] { c0, c1, c1, c0 });
        _checkerTexture.filterMode = FilterMode.Point;
        _checkerTexture.wrapMode = TextureWrapMode.Repeat;
        _checkerTexture.Apply();
    }
}