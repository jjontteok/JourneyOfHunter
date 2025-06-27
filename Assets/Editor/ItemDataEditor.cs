using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemData), true)]
public class ItemDataEditor : Editor
{
    private static Texture2D _checkerTexture;

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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemData item = (ItemData)target;

        if (item.IconImage != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Icon Preview", EditorStyles.boldLabel);

            float aspect = item.IconImage.texture.height / (float)item.IconImage.texture.width;
            Rect rect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(100));

            EnsureCheckerTexture();
            // 배경 체커보드
            GUI.DrawTextureWithTexCoords(rect, _checkerTexture, new Rect(0, 0, rect.width / 16f, rect.height / 16f));

            // 아이콘 이미지 (투명 적용)
            GUI.DrawTexture(rect, item.IconImage.texture, ScaleMode.ScaleToFit, true);
        }
    }
}