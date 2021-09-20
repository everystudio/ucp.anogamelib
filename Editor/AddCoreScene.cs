using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace anogamelib
{
#if UNITY_EDITOR
    public class AddCoreScene : Editor
    {
        /*
        [MenuItem("Tools/AddCoreScene")]
        public static void AddScene()
        {
            if (EditorSceneManager.GetActiveScene().name != "Core")
            {
                //Debug.Log()
                Scene openScene = EditorSceneManager.OpenScene("Assets/Scenes/Core.unity", OpenSceneMode.Additive);
            }

        }
        */
    }
#endif
}


