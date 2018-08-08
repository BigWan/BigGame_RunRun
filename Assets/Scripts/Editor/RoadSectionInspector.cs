using UnityEditor;
using UnityEngine;

namespace RunRun {
    /// <summary>
    /// 跑道段落数据配置文件
    /// </summary>
    [CustomEditor(typeof(RoadSectionData))]
    public class RoadSectionDataInspector : Editor {

        protected override void OnHeaderGUI() {
            base.OnHeaderGUI();
            GUILayout.Button(new GUIContent("标题"));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorList.Show(serializedObject.FindProperty("commands"));

            serializedObject.ApplyModifiedProperties();
        }

    }
}