using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.Scripting;

namespace ED.DataManagement.Samples
{
    [Preserve]
    public class PrefsDataExample2 : BaseData
    {
        [ShowInInspector] private DateTime _field1;
        [ShowInInspector] private TimeSpan _field2;
        [ShowInInspector] private Dictionary<string, string> _field3 = new()
        {
            { "key1", "value1" },
            { "key2", "value2" },
            { "key3", "value3" },
        };
        [ShowInInspector] private List<string> _field4 = new()
        {
            "item1",
            "item2",
            "item3",
        };
    }
}