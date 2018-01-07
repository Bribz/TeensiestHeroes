#if false
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EntityStats))]
public class EntityStatsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EntityStats targetStats = (EntityStats)target;
        EditorGUILayout.LabelField("Some help", "Some other text");

        targetStats.HEALTH = EditorGUILayout.IntField(targetStats.HEALTH);
        //targetStats.MAX_HEALTH = EditorGUILayout.LabelField(targetStats.HEALTH);

        EditorGUILayout.Space();

        targetStats.MOVE_SPEED = EditorGUILayout.FloatField(targetStats.MOVE_SPEED);

        EditorGUILayout.Space();

        targetStats.ARMOR = EditorGUILayout.IntField(targetStats.ARMOR);
        targetStats.WARDING = EditorGUILayout.IntField(targetStats.WARDING);


        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
#endif
