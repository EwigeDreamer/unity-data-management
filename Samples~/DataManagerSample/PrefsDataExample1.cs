using ED.PrefsDataManagement.Attributes;
using ED.PrefsDataManagement.Base;
using UnityEditor;
using UnityEngine.Scripting;
using Random = UnityEngine.Random;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ED.PrefsDataManagement.Samples
{
    [Preserve]
    public class PrefsDataExample1 : BasePrefsData
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty("custom_field_name_1")] private int _field1;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private float _field2;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private string _field3;
        
        private int _ignoredField;

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