    //namespace RunRun {

    //    using UnityEditor;
    //    using UnityEngine;

    //    /// <summary>
    //    /// 跑道段落数据配置文件
    //    /// </summary>
    //    [CustomEditor(typeof(RoadSectionData))]
    //    public class RoadSectionDataEditor : Editor {

    //        protected override void OnHeaderGUI() {
    //            base.OnHeaderGUI();
    //            GUILayout.Button(new GUIContent("标题"));
    //        }

    //        public override void OnInspectorGUI() {
    //            serializedObject.Update();

    //            SerializedProperty list = serializedObject.FindProperty("commands");

    //            if(list.isArray){
    //                EditorGUILayout.HelpBox( new GUIContent( "哈哈,是一个数组"));
    //            }

    //            for (int i = 0; i < list.arraySize; i++) {
    //                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
    //            }

    //            serializedObject.ApplyModifiedProperties();
    //        }

    //    }
    //}