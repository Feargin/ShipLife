using UnityEditor;
using UnityEngine;
using System.Linq;

[InitializeOnLoad]
public class CustomHierarchy : MonoBehaviour
{
    private static Vector2 offset = new Vector2(0, 1);
    public static Color FontColorDefault = new Color(0.8235295f, 0.8235295f, 0.8235295f);
    public static Color FontColorInvisibl = new Color(0.6037736f, 0.6037736f, 0.6037736f);
    public static FontStyle styleFont = FontStyle.Bold;
    public static Color ColorFont = new Color(0.6037736f, 0.6037736f, 0.6037736f);
    public static Color backgroundColorSlash = new Color(0.4901961f, 0.2235294f, 0.2196079f);
    public static Color backgroundColorStars = new Color(0.3882353f, 0.1803922f, 0.1647059f);
    public static Color backgroundColorMinus = new Color(0.2901961f, 0.2431373f, 0.2431373f);
    public static Color backgroundColorPlus = new Color(0.101961f, 0.131373f, 0.2431373f);
    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    }
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        Color ColorBackground = new Color(0.2f, 0.2f, 0.2f);
        
        GameObject gameObj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        
        if (obj != null)
        {
            if (gameObj.activeInHierarchy == false)
            {
                ColorFont = FontColorInvisibl;
                styleFont = FontStyle.Normal;
            }
            else
            {
                ColorFont = FontColorDefault;
                styleFont = FontStyle.Bold;
            }
            if(obj.name.Length > 2)
            {
                if(obj.name.Remove(2) == "//")  
                {
                    ColorBackground = backgroundColorSlash;
                    SetColorData(ColorBackground, selectionRect, obj.name);
                }
                else if(obj.name.Remove(2) == "**") 
                {
                    ColorBackground = backgroundColorStars;
                    SetColorData(ColorBackground, selectionRect, obj.name);
                }
                else if(obj.name.Remove(2) == "--") 
                {
                    ColorBackground = backgroundColorMinus;
                    SetColorData(ColorBackground, selectionRect, obj.name);
                }
                else if(obj.name.Remove(2) == "++") 
                {
                    ColorBackground = backgroundColorPlus;
                    SetColorData(ColorBackground, selectionRect, obj.name);
                }
            }
        }
    }
    private static void SetColorData(Color color, Rect selection, string name)
    {
        Rect offsetRect = new Rect(selection.position + offset, selection.size);
        EditorGUI.DrawRect(selection, color);
        EditorGUI.LabelField(offsetRect, name, new GUIStyle() { normal = new GUIStyleState() { textColor = ColorFont }, fontStyle = styleFont});
    }
}