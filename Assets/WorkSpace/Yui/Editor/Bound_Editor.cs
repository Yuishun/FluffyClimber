using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(BoundComponent))]
public class Bound_Editor : Editor
{
    private BoundComponent _target;

    private void Awake()
    {
        _target = target as BoundComponent;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.LabelField("ぶつかっていい方向");
        _target.PowFlag = (BoundComponent.CanPowerFlag)
            EditorGUILayout.EnumFlagsField("Pow Flag", _target.PowFlag);
    }
}
#endif