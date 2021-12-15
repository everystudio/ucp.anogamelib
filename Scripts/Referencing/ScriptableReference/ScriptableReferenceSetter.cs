using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#pragma warning disable 649

namespace anogamelib
{
    public class ScriptableReferenceSetter : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReference target;

        private void Awake()
        {
            target.Reference = this.gameObject;
        }
    }
}

