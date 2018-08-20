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
            moveButtonContent = new GUIContent("\u21b4", "move down"),
            duplicateButtonContent = new GUIContent("+", "duplicate"),
            deleteButtonContent = new GUIContent("-", "delete"),
            addButtonContent = new GUIContent("+", "add element");

        protected override void OnHeaderGUI() {
            base.OnHeaderGUI();
            EditorGUILayout.HelpBox(headGC, true);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            SerializedProperty list = serializedObject.FindProperty("commands");


            //if (list.arraySize == 0) {
            //    EditorGUILayout.HelpBox(new GUIContent("这是一个空的段落,点击按钮新增数据"));
            //    if (GUILayout.Button("添加一个Block规则")) {
            //        list.InsertArrayElementAtIndex(0);
            //    }
            //} else {

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Size:");
                EditorGUILayout.LabelField(list.arraySize.ToString());
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Space();

                EditorGUILayout.Separator();

                //EditorGUILayout.PropertyField(size);
                for (int i = 0; i < list.arraySize; i++) {

                    EditorGUILayout.BeginVertical("box");


                    EditorGUILayout.BeginHorizontal();

                    EditorGUILayout.LabelField(new GUIContent($"GROUP {i}"));

                    if (GUILayout.Button(addButtonContent)) {
                        list.InsertArrayElementAtIndex(i);
                    }
                    if (GUILayout.Button(deleteButtonContent)) {
                        list.DeleteArrayElementAtIndex(i);

                    }
                    if (GUILayout.Button(moveButtonContent)) {
                        list.MoveArrayElement(i, i + 1);
                    }



                    EditorGUILayout.EndHorizontal();

                    EditorGUI.indentLevel++;


                    EditorGUILayout.PrefixLabel("Block数量:");
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.indentLevel++;

                if (list.arraySize > 0) {
                    SerializedProperty element = list.GetArrayElementAtIndex(i);
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

                    // blocks
                    EditorGUI.indentLevel++;
                    for (int j = 0; j < blocks.arraySize; j++) {
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.PropertyField(blocks.GetArrayElementAtIndex(j), GUIContent.none);

                        if (GUILayout.Button(deleteButtonContent, EditorStyles.miniButton, GUILayout.Width(30f))) {
                            blocks.DeleteArrayElementAtIndex(j);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;

                }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Separator();




                    EditorGUILayout.EndVertical(); // end box

                }

            

            serializedObject.ApplyModifiedProperties();
        }


        void ShowButtons() {
        }

    }
}