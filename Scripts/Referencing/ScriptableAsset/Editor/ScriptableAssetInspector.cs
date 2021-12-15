using UnityEngine;
using System.Collections;
using UnityEditor;

namespace anogamelib
{
    [CustomEditor(typeof(ScriptableAsset), true)]
    public class ScriptableAssetInspector : Editor
    {
        private string currentGUID;

        public override void OnInspectorGUI()
        {
            if (string.IsNullOrEmpty(currentGUID))
            {
                currentGUID = $"GUID: {((IReferenceableAsset)target).GetGuid()}";
            }
            EditorGUILayout.TextArea(currentGUID, EditorStyles.centeredGreyMiniLabel);

            base.DrawDefaultInspector();
        }
    }
}
