using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using static DeathManager;

[CustomEditor(typeof(DeathCommentHolder))]
public class CommentHolderEditor : Editor
{
    private bool bFolding = false;  //  折りたたむか
    private DeathCommentHolder holder;
    private GUIStyle style;

    private void Awake()
    {
        holder = target as DeathCommentHolder;
        style = EditorStyles.label;
        style.wordWrap = true;
        
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();

        //  カスタム表示---------------------------------------
        EditorGUILayout.LabelField("!!--- プレイヤー死亡時にDeathCommentHolderのIncreaseDeathCount()を呼び出してください ---!!", style);

        //  トラップ情報
        holder.trapType = (TrapType)EditorGUILayout.EnumPopup("トラップの種類", holder.trapType);
        holder.trapNumber = EditorGUILayout.IntField("トラップ番号", holder.trapNumber);
        holder.bActiveTrigger = EditorGUILayout.Toggle("トリガーを使用", holder.bActiveTrigger);
        holder.bScreenSpaceComment = EditorGUILayout.Toggle("スクリーンスペースに表示", holder.bScreenSpaceComment);

        EditorGUI.BeginDisabledGroup(holder.bScreenSpaceComment);
        holder.dispPos = EditorGUILayout.Vector3Field("ワールドスペース表示位置", holder.dispPos);
        EditorGUI.EndDisabledGroup();

        //  コメント情報
        SerializedProperty _list = serializedObject.FindProperty("comments");
        DrawList(_list, 0);

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawList(SerializedProperty self, int mode)
    {
        if (!self.isArray || self.propertyType == SerializedPropertyType.String)
        {
            EditorGUILayout.PropertyField(self, new GUIContent(self.displayName), true);
            return;
        }

        using (new GUILayout.HorizontalScope())
        {
            string _str = (mode == 0) ? string.Format("{0} [{1}]", "死亡コメント", self.arraySize) : "コメント内容";
            EditorGUILayout.PropertyField(self, new GUIContent(_str), false);
            GUILayout.FlexibleSpace();
            if (GUILayout.Button(EditorGUIUtility.TrIconContent("d_winbtn_graph_max_h"), "RL FooterButton", GUILayout.Width(16)))
            {
                self.isExpanded = true;
                for (var i = 0; i < self.arraySize; i++)
                    self.GetArrayElementAtIndex(i).isExpanded = true;
                return;
            }
            if (GUILayout.Button(EditorGUIUtility.TrIconContent("d_winbtn_graph_min_h"), "RL FooterButton", GUILayout.Width(16)))
            {
                self.isExpanded = false;
                for (var i = 0; i < self.arraySize; i++)
                    self.GetArrayElementAtIndex(i).isExpanded = false;
                return;
            }
            if (GUILayout.Button(EditorGUIUtility.TrIconContent("Toolbar Plus"), "RL FooterButton", GUILayout.Width(16)))
                self.InsertArrayElementAtIndex(self.arraySize);
        }
        if (!self.isExpanded)
            return;

        using (new EditorGUI.IndentLevelScope(1))
        {
            if (self.arraySize <= 0)
                EditorGUILayout.LabelField("Array is Empty");

            for (var i = 0; i < self.arraySize; i++)
            {
                var prop = self.GetArrayElementAtIndex(i);
                using (new EditorGUILayout.HorizontalScope())
                {
                    string _str = (mode == 0) ? "コメントその" + (i + 1).ToString() : (i+1).ToString() + "行目";
                    EditorGUILayout.PropertyField(prop, new GUIContent(_str), prop.propertyType != SerializedPropertyType.Generic);
                    if (GUILayout.Button(EditorGUIUtility.TrIconContent("Toolbar Minus"), "RL FooterButton", GUILayout.Width(16)))
                    {
                        self.DeleteArrayElementAtIndex(i);
                        return;
                    }
                }

                if (prop.propertyType != SerializedPropertyType.Generic || !prop.isExpanded)
                    continue;
                using (new EditorGUI.IndentLevelScope(1))
                {
                    using (new GUILayout.VerticalScope("box"))
                    {
                        var skipCount = 0;
                        while (prop.NextVisible(true))
                        {
                            if (skipCount > 0)
                            {
                                skipCount--;
                                continue;
                            }
                            if (prop.depth != self.depth + 2)
                                break;
                            if (prop.isArray && prop.propertyType != SerializedPropertyType.String)
                            {
                                DrawList(prop, 1);
                                skipCount = prop.arraySize + 1;
                                continue;
                            }

                            EditorGUILayout.PropertyField(prop, false);
                        }
                    }
                }
            }
        }
    }
}

#endif