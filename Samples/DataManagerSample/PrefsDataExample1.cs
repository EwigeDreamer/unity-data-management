using System;
using Sirenix.OdinInspector;
using UnityEngine.Scripting;

namespace ED.DataManagement.Samples
{
    [Preserve]
    [System.Serializable]
    public class PrefsDataExample1 : DataBase
    {
        [ShowInInspector] private int _field1;
        [ShowInInspector] private float _field2;
        [ShowInInspector] private string _field3;
        
        [NonSerialized] private int _ignoredField;

        [Button]
        private void Test1()
        {
            
        }
    }
}