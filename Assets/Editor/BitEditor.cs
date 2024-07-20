using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Bit), true)]
public class BitEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();

        DrawDefaultInspector();

        Bit bit = (Bit)target;

        if (bit.SlotCols.Count > 0) {
            EditorGUILayout.LabelField("Slots");
            EditorGUI.indentLevel++;

            for (int i = 0; i < bit.SlotCols.Count; i++) {
                Collider2D col = bit.SlotCols[i];
                Bit attachedBit = bit.Slots.ContainsKey(i) ? bit.Slots[i] : null;
                string bitInfo = attachedBit != null ? $"{col.offset} | {attachedBit.gameObject.name}" : "None";

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(bitInfo);
                EditorGUILayout.EndHorizontal();
            }

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }
}