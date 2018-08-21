namespace RunRun {

    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// 段落配置编辑器
    /// </summary>
    [CustomEditor(typeof(RoadSectionData))]
    public class RoadSectionDataEditor : Editor {

        GUIContent headGC = new GUIContent("Section Editor");

        private static GUIContent
            moveButtonContent = new GUIContent("↓", "move down"),
            deleteButtonContent = new GUIContent("-", "delete element"),
            addButtonContent = new GUIContent("+", "add element");

        protected override void OnHeaderGUI() {
            base.OnHeaderGUI();
            EditorGUILayout.HelpBox(headGC, true);
        }

        

        public override void OnInspectorGUI() {
            serializedObject.Update();

            SerializedProperty list = serializedObject.FindProperty("commands");

            // size
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Size:");
            EditorGUILayout.LabelField(list.arraySize.ToString());
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();


            if (list.arraySize == 0) {
                if (GUILayout.Button("添加一个Block规则")) {
                    list.InsertArrayElementAtIndex(0);
                }
            }


            for (int groupIndex = 0; groupIndex < list.arraySize; groupIndex++) {                

                EditorGUILayout.BeginHorizontal("U2D.createRect"); // box start

                EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(groupIndex),GUIContent.none);

                EditorGUILayout.BeginVertical("box"); // box start
                if (GUILayout.Button(addButtonContent, GUILayout.Height(33f), GUILayout.Width(33f))) {
                    list.InsertArrayElementAtIndex(groupIndex);
                }
                if(GUILayout.Button(deleteButtonContent,GUILayout.Height(33f), GUILayout.Width(33f))) {
                    list.DeleteArrayElementAtIndex(groupIndex);
                }
                if(GUILayout.Button(moveButtonContent, GUILayout.Height(33f), GUILayout.Width(33f))) {
                    list.MoveArrayElement(groupIndex, groupIndex + 1);
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal(); // end box

                EditorGUILayout.Space();
            }
                       

            serializedObject.ApplyModifiedProperties();
        }




        /// <summary>
        /// Group的标题
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        void DrawGroupTitleAndButtons(SerializedProperty list,int index) {
            // title and buttons
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent($"GROUP {index}"));

            if (GUILayout.Button(addButtonContent)) {
                list.InsertArrayElementAtIndex(index);
            }
            if (GUILayout.Button(deleteButtonContent)) {
                list.DeleteArrayElementAtIndex(index);
            }
            if (GUILayout.Button(moveButtonContent)) {
                list.MoveArrayElement(index, index + 1);
            }

            EditorGUILayout.EndHorizontal();
        }

        
        /// <summary>
        /// Group内的元素
        /// </summary>
        /// <param name="element"></param>
        void DrawGroupElement(SerializedProperty element) {

            // blocks 
            EditorGUI.indentLevel++;


            EditorGUILayout.PrefixLabel("Block数量:");
            EditorGUILayout.BeginHorizontal();
            EditorGUI.indentLevel++;



            SerializedProperty minProperty = element.FindPropertyRelative("min");
            SerializedProperty maxProperty = element.FindPropertyRelative("max");
            SerializedProperty blocks = element.FindPropertyRelative("blocks");

            // min
            EditorGUILayout.PropertyField(minProperty, GUIContent.none);

            if (GUILayout.Button(addButtonContent, GUILayout.Width(30f))) {
                minProperty.intValue++;
            }
            if (GUILayout.Button(deleteButtonContent, GUILayout.Width(30f))) {
                minProperty.intValue--;
            }

            // max
            EditorGUILayout.PropertyField(maxProperty, GUIContent.none);

            if (GUILayout.Button(addButtonContent, GUILayout.Width(30f))) {
                maxProperty.intValue++;
            }
            if (GUILayout.Button(deleteButtonContent, GUILayout.Width(30f))) {
                maxProperty.intValue--;
            }
            EditorGUI.indentLevel--;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent($"本节可选块 {blocks.arraySize} "), GUILayout.Width(150f));
            if (GUILayout.Button(addButtonContent, EditorStyles.miniButton, GUILayout.Width(30f))) {
                blocks.InsertArrayElementAtIndex(blocks.arraySize);
            }
            EditorGUILayout.EndHorizontal();

            // blocks list
            EditorGUI.indentLevel++;
            for (int blockIndex = 0; blockIndex < blocks.arraySize; blockIndex++) {
                EditorGUILayout.BeginHorizontal();
                SerializedProperty sp = blocks.GetArrayElementAtIndex(blockIndex);
                EditorGUILayout.PropertyField(sp, GUIContent.none);

                if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButton, GUILayout.Width(30f))) {
                    blocks.DeleteArrayElementAtIndex(blockIndex);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;



            EditorGUI.indentLevel--;
            EditorGUILayout.Separator();


        }
    }
}