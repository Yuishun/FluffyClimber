using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using C_G = Component_Gimmick;


[CustomEditor(typeof(Component_Gimmick))]
public class Component_Editor : Editor
{
    
    private C_G _target;

    private void Awake()
    {
        _target = target as Component_Gimmick;
        //list = serializedObject.FindProperty("_Comp");
    }

    public override void OnInspectorGUI()
    {        
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        //SerializedProperty list = serializedObject.FindProperty("_Comp");
        

        for (int i = 0; i < _target.Comp.Count; i++)
        {
            if (_target.Comp[i] == null)
               _target.Comp[i] = NumToComponent_(C_.Component_Kind.Pos);
            
            C_.Component_ comp = NumToComponent_(_target.Comp[i].type);
            if(_target.Comp[i].GetType() != comp.GetType())
            {                
                _target.Comp[i] = comp;                
            }
            //var prop = list.GetArrayElementAtIndex(i);            
            //EditorGUILayout.EnumPopup("モード" ,_target.Comp[i].type);
            //EditorGUILayout.PropertyField(prop);
        }

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_Comp"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useDefaultPos"));
        if (!_target.useDefaultPos)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("basisPos"));        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("I_movement"));        
        CustomEditorUtility.DrawList(serializedObject.FindProperty("_Comp"), _target);

        // GUIの更新があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            // EditorUtility.SetDirty(_target);
            serializedObject.ApplyModifiedProperties();
        }
    }

    C_.Component_ NumToComponent_(C_.Component_Kind kind)
    {
        C_.Component_ cp;
        switch(kind)
        {
            case C_.Component_Kind.Pos:
                cp = new C_.Component_Pos();
                break;
            case C_.Component_Kind.Vec:
                cp = new C_.Component_Vec();
                break;
            case C_.Component_Kind.Rot:
                cp = new C_.Component_Rot();
                break;

            default:
                cp = new C_.Component_Pos();
                break;
        }
        return cp;
    }
}
#endif