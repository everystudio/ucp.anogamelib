using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anogamelib
{
    [CreateAssetMenu(fileName = "SaveablePrefab", menuName = "Referencing/SaveablePrefab")]
    public class SaveablePrefab : ScriptableAsset
    {
        [SerializeField]
        private GameObject prefab;

        [SerializeField]
        private ScriptableReference m_instanceManagerReference;

        [System.NonSerialized]
        private SaveablePrefabInstanceManager m_instanceManager;

        public T Retrieve<T>(string identification = "") where T : UnityEngine.Object
        {
            GameObject prefabInstance = GameObject.Instantiate(prefab.gameObject);

            Saveable getSaveable = prefabInstance.GetComponent<Saveable>();

            if (getSaveable != null)
            {
                if (m_instanceManager == null)
                {
                    GameObject getManagerObject = m_instanceManagerReference?.Reference;
                    if (getManagerObject != null)
                    {
                        m_instanceManager = getManagerObject.GetComponent<SaveablePrefabInstanceManager>();
                    }
                    else
                    {
                        Debug.Log("No instance manager found within this scene. This means that a prefab will not save.");
                    }
                }

                if (m_instanceManager != null)
                {
                    SaveableInstance instanceController = prefabInstance.AddComponent<SaveableInstance>();
                    instanceController.SetSaveablePrefabInstanceManager(m_instanceManager, getSaveable, this);

                    m_instanceManager.AddListener(getSaveable, this, identification);
                }
            }
            if (getSaveable != null && typeof(T) == typeof(Saveable))
            {
                return getSaveable as T;
            }

            if (typeof(T) == typeof(GameObject))
            {
                return prefabInstance as T;
            }

            return prefabInstance.GetComponent<T>();
        }
    }
}