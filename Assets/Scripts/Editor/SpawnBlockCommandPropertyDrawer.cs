using UnityEditor;
using UnityEngine;

/// <summary>
/// 跑道段落数据配置文件
/// </summary>
//[CustomPropertyDrawer(typeof(RunRun.SpawnBlockCommand))]
public class SpawnBlockCommandPropertyDraw : PropertyDrawer {

    bool foldout;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        // return EditorGUIUtility.wideMode ? 16f:34f;
        return property.FindPropertyRelative("blocks").arraySize*16f+80f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){

        SerializedProperty
            list = property.FindPropertyRelative("blocks"),
            start = property.FindPropertyRelative("start"),
            end = property.FindPropertyRelative("end");


        GUIStyle titleStyle = new GUIStyle("AssetLabel");
        titleStyle.fixedHeight = 20f;
        titleStyle.fontSize = 12;
        titleStyle.fontStyle = FontStyle.Bold;


        Vector2 halfSize = new Vector2(EditorGUIUtility.currentViewWidth * 0.4f, 16f);

        label = EditorGUI.BeginProperty(position, GUIContent.none, property);

        GUI.Label(position, "SpawnBlockCommand",titleStyle);



        GUIStyle bgStyle = new GUIStyle("CN Box");

        EditorGUI.indentLevel += 1;
        position.y += 5f;
        Rect bodyPosition = EditorGUI.IndentedRect(position);
        EditorGUI.indentLevel -= 1;

        bodyPosition = new Rect(bodyPosition.x, bodyPosition.y + 20, bodyPosition.width, bodyPosition.height - 40);




        EditorGUIUtility.labelWidth = 50f;
        Rect startRect = new Rect(bodyPosition.position, halfSize);
        EditorGUI.PropertyField(startRect, start);
        Rect endRect = new Rect(bodyPosition.position + new Vector2(halfSize.x,0),halfSize);
        EditorGUI.PropertyField(endRect, end);


        Rect listRect = new Rect(bodyPosition.position - Vector2.down * 18f, bodyPosition.size - Vector2.down * 36f);

        foldout = EditorGUI.Foldout(listRect, foldout, new GUIContent("数组"));

        //foldout = EditorGUI.Foldout(position,foldout,"name");
        EditorGUI.indentLevel+=1;
        position = EditorGUI.IndentedRect(position);
        EditorGUI.indentLevel-=1;

        if(foldout){
            position = new Rect(listRect.x, listRect.y, listRect.width,18f);
            for (int i = 0; i < list.arraySize; i++) {
                position.y+=18f;                
                EditorGUI.PropertyField(position,list.GetArrayElementAtIndex(i));
            }
        }
        // Debug.Log(position.height);


        EditorGUI.EndProperty();




    }

}
