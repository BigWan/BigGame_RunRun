using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// 在层级面板的分隔符
/// </summary>
[ExecuteInEditMode]
public class HierarchySeparator : MonoBehaviour {

    public string sepName;

    private char fillChar = '-';

    private int length = 40;

    [ContextMenu("设置")]
    public void SetName() {
        int fillLength = length - sepName.Length-15;
        string fillString = new string(fillChar, fillLength );
        gameObject.name = new string(fillChar, 8) + sepName + fillString;
    }

    public void SetAllName() {
        Object[] gos = FindObjectsOfType(typeof(HierarchySeparator));
        foreach (var item in gos) {
            (item as HierarchySeparator).SetName();
        }
    }


    

    

}

[CustomEditor(typeof(HierarchySeparator))]
[CanEditMultipleObjects]
public class HierarchySeparatorEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if(GUILayout.Button("设置",GUILayout.Height(500f))){
            foreach (var target in targets) {
                (target as HierarchySeparator).SetName();
            }
        }


        EditorGUI.BeginChangeCheck();
        if (EditorGUI.EndChangeCheck()) {
            (target as HierarchySeparator).SetName();
        }
    }
}
