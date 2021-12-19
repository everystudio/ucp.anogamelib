using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace anogamelib
{
    [AddComponentMenu("")]
    public class SaveableInstance : MonoBehaviour
    {
        public SaveablePrefabInstanceManager m_prefabInstanceManager;
        public SaveablePrefab m_saveablePrefab;
        public Saveable m_saveable;

        public void SetSaveablePrefabInstanceManager(SaveablePrefabInstanceManager _reference, Saveable _saveable, SaveablePrefab _saveablePrefab)
        {
            m_prefabInstanceManager = _reference;
            m_saveable = _saveable;
            m_saveablePrefab = _saveablePrefab;
        }

        public void OnDisable()
        {
            m_prefabInstanceManager.RemoveListener(m_saveable, m_saveablePrefab);
        }
    }
}
