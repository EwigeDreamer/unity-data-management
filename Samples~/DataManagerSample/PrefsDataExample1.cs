using System;
using ED.DataManagement.Base;
using UnityEditor;
using UnityEngine;
using UnityEngine.Scripting;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ED.DataManagement.Samples
{
    [Preserve]
    [System.Serializable]
    public class PrefsDataExample1 : BaseData
    {
        [SerializeField] private int _field1;
        [SerializeField] private float _field2;
        [SerializeField] private string _field3;
        
        [NonSerialized] private int _ignoredField;

#if ODIN_INSPECTOR
        [Button]
#endif
        private void Test1()
        {
            
        }
        
#if ODIN_INSPECTOR
        [Button]
#endif
        public void SetRandom()
        {
            _field1 = Random.Range(0, 100);
            _field2 = Random.Range(0f, 1f);
            _field3 = GUID.Generate().ToString();
        }
    }
}