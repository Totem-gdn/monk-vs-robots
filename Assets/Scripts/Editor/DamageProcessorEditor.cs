using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(DamageProcessor))]
public class DamageProcessorEditor : Editor
{
    private SerializedProperty onDamage;

    private void OnEnable()
    {
        onDamage = serializedObject.FindProperty("OnDamage");
    }

    public override void OnInspectorGUI()
    {
        var damageProcessor = target as DamageProcessor;

        using(new EditorGUI.DisabledScope(true))
        {
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour(damageProcessor), GetType(), false);
        }
        damageProcessor.isInvincibleAfterHit = EditorGUILayout.Toggle("Is Invincible After Hit", damageProcessor.isInvincibleAfterHit);
        if(damageProcessor.isInvincibleAfterHit)
        {
            damageProcessor.hitDelay = EditorGUILayout.FloatField("Hit Delay", damageProcessor.hitDelay);
        }
        EditorGUILayout.PropertyField(onDamage);
    }
}
