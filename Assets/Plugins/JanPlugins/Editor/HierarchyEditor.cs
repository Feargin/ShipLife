using UnityEditor;

public class HierarchyEditor : EditorWindow
{
    [MenuItem("Tools/Hierarchy Editor")]
    public static void ShowWindow()
    {
        GetWindow<HierarchyEditor>("HierarchyEditor");
    }
    private void OnGUI()
    {
        CustomHierarchy.backgroundColorSlash = EditorGUILayout.ColorField("// Color", CustomHierarchy.backgroundColorSlash);
        CustomHierarchy.backgroundColorStars = EditorGUILayout.ColorField("** Color", CustomHierarchy.backgroundColorStars);
        CustomHierarchy.backgroundColorPlus = EditorGUILayout.ColorField("++ Color", CustomHierarchy.backgroundColorPlus);
        CustomHierarchy.backgroundColorMinus = EditorGUILayout.ColorField("-- Color", CustomHierarchy.backgroundColorMinus);
        CustomHierarchy.FontColorDefault = EditorGUILayout.ColorField("FontDefault Color", CustomHierarchy.FontColorDefault);
        CustomHierarchy.FontColorInvisibl = EditorGUILayout.ColorField("FontInvisibl Color", CustomHierarchy.FontColorInvisibl);
    }
}