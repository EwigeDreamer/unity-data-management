using System;
using System.Collections.Generic;
using ED.PrefsDataManagement.Attributes;
using ED.PrefsDataManagement.Base;
using UnityEngine.Scripting;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace ED.PrefsDataManagement.Samples
{
    [Preserve]
    public class PrefsDataExample2 : BasePrefsData
    {
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private DateTime _field1;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private TimeSpan _field2;
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private Dictionary<string, string> _field3 = new()
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" },
        };
        
#if ODIN_INSPECTOR
        [ShowInInspector]
#endif
        [PrefsDataProperty] private List<string> _field4 = new()
        {
            "item1",
            "item2",
            "item3",
        };
    }
}