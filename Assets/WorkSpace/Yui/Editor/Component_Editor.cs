﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using C_G = Component_Gimmick;
using C_;


[CustomEditor(typeof(Component_Gimmick))]
public class Component_Editor : Editor
{
    Mochineko.SimpleReorderableList.ReorderableList reorderableList;
    private C_G _target;

    private void Awake()
    {
        _target = target as Component_Gimmick;
        //list = serializedObject.FindProperty("_Comp");
    }

    private void OnEnable()
    {
        reorderableList = new Mochineko.SimpleReorderableList.ReorderableList(
            serializedObject.FindProperty("_Comp")
            );
    }

    public override void OnInspectorGUI()
    {        
        serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        SerializedProperty list = serializedObject.FindProperty("_Comp");
        

        for (int i = 0; i < _target.Comp.Count; i++)
        {
            if (_target.Comp[i] == null)
            {
                if (_target.Comp.Count == 1)
                    _target.Comp[i] = NumToComponent_(Component_Kind.Pos);
                else
                    _target.Comp[i] = NumToComponent_(_target.Comp[i - 1].type);
                var prop = list.GetArrayElementAtIndex(i);
                prop.managedReferenceValue = _target.Comp[i];
            }
            
            var comp = NumToComponent_(_target.Comp[i].type);
            if(_target.Comp[i].GetType() != comp.GetType())
            {
                _target.Comp[i] = comp;                
                var prop = list.GetArrayElementAtIndex(i);
                prop.managedReferenceValue = comp;                
            }
            //EditorGUILayout.EnumPopup("モード" ,_target.Comp[i].type);
            //EditorGUILayout.PropertyField(prop);
        }

        //EditorGUILayout.PropertyField(serializedObject.FindProperty("_Comp"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("usebasisDefault"));
        if (!_target.usebasisDefault)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("basisPos"));        
        EditorGUILayout.PropertyField(serializedObject.FindProperty("I_movement"));
        if (_target.I_movement == C_G.IndexMovement.N_Count)
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxCount"));
        //CustomEditorUtility.DrawList(serializedObject.FindProperty("_Comp"), _target);
        Mochineko.SimpleReorderableList.Samples.Editor
            .EditorFieldUtility.ReadOnlyComponentField(target as MonoBehaviour, this);
        if (reorderableList != null)
            reorderableList.Layout();

        // GUIの更新があったら実行
        if (EditorGUI.EndChangeCheck())
        {
            // EditorUtility.SetDirty(_target);
            serializedObject.ApplyModifiedProperties();
        }
    }

    Component_ NumToComponent_(Component_Kind kind)
    {
        Component_ cp;
        switch(kind)
        {
            case Component_Kind.Pos:
                cp = new Component_Pos();
                break;
            case Component_Kind.Vec:
                cp = new Component_Vec();
                break;
            case Component_Kind.Rot:
                cp = new Component_Rot();
                break;
            case Component_Kind.Move:
                cp = new Component_Move();
                break;
            case Component_Kind.Time:
                cp = new Component_Time();
                break;
            case Component_Kind.Enable:
                cp = new Component_Enable();
                break;
            case Component_Kind.Particle:
                cp = new Component_Particle();
                break;
            case Component_Kind.Audio:
                cp = new Component_Audio();
                break;
            case Component_Kind.Concurrent:
                cp = new Component_Concurrent();
                break;
            case Component_Kind.Event:
                cp = new Component_Event();
                break;
            case Component_Kind.Text:
                cp = new Component_Text();
                break;

            default:
                if (_target.Comp.Count > 0)
                    cp = _target.Comp[_target.Comp.Count - 1];
                else
                    cp = new Component_Pos();
                break;
        }
        return cp;
    }
}

#endif