using UnityEditor;
using UnityEngine;

/// <summary>
/// 跑道段落数据配置文件
/// </summary>
[CustomPropertyDrawer(typeof(RunRun.SpawnBlockCommand))]
public class SpawnBlockCommandPropertyDraw : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        // return EditorGUIUtility.wideMode ? 16f:34f;
        return property.FindPropertyRelative("blocks").arraySize * 18f + 60f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        SerializedProperty
            blocks = property.FindPropertyRelative("blocks"),
            min = property.FindPropertyRelative("min"),
            max = property.FindPropertyRelative("max");

        GUIContent 
            minContent = new GUIContent("min:", "数量下线"),
            maxContent = new GUIContent("max:", "Block数量上限"),
            blocksContent = new GUIContent("Blocks:", "随机列表");


        //EditorGUIUtility.labelWidth = 60f;
        //EditorGUI.PrefixLabel(position, label);

        //EditorGUI.indentLevel +=1;
        position = EditorGUI.IndentedRect(position);
        //position.y += 18f;


        Rect lineRect = new Rect(position) {
            height = 16f
        };



        //EditorGUI.indentLevel = 1;

        //if (position.height > 16f) {
        //    position.height = 16f;
        //    EditorGUI.indentLevel += 1;
        //    lineRect = EditorGUI.IndentedRect(lineRect);
        //    lineRect.y += 18f;
        //}

        EditorGUIUtility.labelWidth = 45f;
        lineRect.width = position.width * 0.3f;
        EditorGUI.LabelField(lineRect, $"数量区间:    {min.intValue} / {max.intValue}");
        lineRect.x += lineRect.width;
        lineRect.width = position.width * 0.7f;

        float min2 = min.intValue, max2 = max.intValue;

        EditorGUI.BeginChangeCheck();
        EditorGUI.MinMaxSlider(lineRect,ref min2,ref max2, 0, 5);
        if (EditorGUI.EndChangeCheck()) {
            min.intValue = (int)min2;
            max.intValue = (int)max2;
        }


        lineRect.y += 18f;
        lineRect.x = position.x;
        lineRect.width = position.width;
        //EditorGUI.LabelField(lineRect,"Blocks:");
        lineRect.height = 34f;
        if (GUI.Button(lineRect, $"Blocks Add(当前{blocks.arraySize})",new GUIStyle("flow node 5"))) {
            blocks.arraySize++;
        }

        lineRect.height = 16f;
        lineRect.y += 22f;
        EditorGUI.indentLevel += 1;
        lineRect = EditorGUI.IndentedRect(lineRect);
        for (int i = 0; i < blocks.arraySize; i++) {
            //lineRect.x = position.x;
            //lineRect.width = position.width*-60f;
            lineRect.y += 18f;
            Rect fieldRect = new Rect(lineRect) {
                width = lineRect.width - 75f
            };


            EditorGUI.PropertyField(fieldRect, blocks.GetArrayElementAtIndex(i),GUIContent.none);
            fieldRect = MoveRight(fieldRect);
            fieldRect.width = 20f;
            if (GUI.Button(fieldRect, "-")) {
                int oldSize = blocks.arraySize;

                blocks.DeleteArrayElementAtIndex(i);
                if(blocks.arraySize == oldSize) {
                    blocks.DeleteArrayElementAtIndex(i);
                }
            }

        }
        EditorGUI.indentLevel -= 1;
        //EditorGUI.indentLevel -= 1;

    }


    float sep = 2f;
    Rect MoveRight(Rect r) {
        return new Rect(r) {
            x = r.x + r.width + sep,
        };
    }

}
