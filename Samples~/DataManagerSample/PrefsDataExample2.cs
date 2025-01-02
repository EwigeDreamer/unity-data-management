using System;
using System.Collections.Generic;
using ED.DataManagement.Base;
using UnityEngine.Scripting;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ED.DataManagement.Samples
{
    [Preserve]
    [System.Serializable]
    public class PrefsDataExample2 : BaseData
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private DateTime _field1;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private TimeSpan _field2;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private Dictionary<string, string> _field3 = new()
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" },
        };
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        private List<string> _field4 = new()
        {
            "item1",
            "item2",
            "item3",
        };
    }
}