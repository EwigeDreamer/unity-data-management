using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ED.PrefsDataManagement.Attributes;
using ED.PrefsDataManagement.Base;
using ED.PrefsDataManagement.Interfaces;
using Newtonsoft.Json;
using UnityEngine;

namespace ED.PrefsDataManagement.Logic
{
    public class PrefsDataManager : IPrefsDataManager, IDisposable
    {
        private const string NamePrefix = "player_data";
        
        private readonly IPrefsDataProvider _dataProvider;
        
        private readonly Dictionary<Type, BasePrefsData> _repository = new();
        
        public PrefsDataManager(IPrefsDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            foreach (var type in GetDataTypes())
                _repository[type] = LoadData(type);
        }
        
        public List<BasePrefsData> GetAllData() => _repository.Values.ToList();
        
        public T GetData<T>() where T : BasePrefsData, new()
        {
            if (_repository.TryGetValue(typeof(T), out var data))
                return (T)data;
            return (T)(_repository[typeof(T)] = LoadData(typeof(T)));
        }

        private BasePrefsData LoadData(Type type)
        {
            if (!type.IsSubclassOf(typeof(BasePrefsData))) return null;
            var data = (BasePrefsData)Activator.CreateInstance(type);
            data.OnSave += () => SaveData(data);
            var fields = LoadFields(GetDataName(type));
            SetFields(data, fields);
            data.OnAfterLoad();
            return data;
        }

        private void SaveData(BasePrefsData data)
        {
            data.OnBeforeSave();
            var fields = GetFields(data);
            SaveFields(GetDataName(data.GetType()), fields);
        }

        private static string GetDataName(Type type) => $"{NamePrefix}_{type.Name.ToLower()}";

        private static FieldData[] GetFields(BasePrefsData source)
        {
            return source.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(a => a.GetCustomAttribute<PrefsDataPropertyAttribute>() != null)
                .Select(field => new FieldData
                {
                    name = GetFieldName(field),
                    type = field.FieldType.FullName,
                    data = JsonConvert.SerializeObject(field.GetValue(source))
                })
                .ToArray();
        }

        private static string GetFieldName(FieldInfo field)
        {
            var nameAttribute = field.GetCustomAttribute<PrefsDataPropertyAttribute>();
            return nameAttribute?.Name ?? field.Name;
        }

        private static void SetFields(BasePrefsData target, FieldData[] fields)
        {
            var targetFields = target.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(a => a.GetCustomAttribute<PrefsDataPropertyAttribute>() != null)
                .ToDictionary(GetFieldName);
            
            foreach (var field in fields)
            {
                if (!targetFields.TryGetValue(field.name, out var targetField)) continue;
                var type = Type.GetType(field.type);
                if (type == null || type != targetField.FieldType) continue;
                var fieldData = JsonConvert.DeserializeObject(field.data, type);
                if (fieldData == null) continue;
                targetField.SetValue(target, fieldData);
            }
        }

        private FieldData[] LoadFields(string name)
        {
            var json = _dataProvider.LoadData(name);
            if (string.IsNullOrWhiteSpace(json)) return Array.Empty<FieldData>();
            var data = JsonConvert.DeserializeObject<FieldData[]>(json);
            if (data == null) return Array.Empty<FieldData>();
            Debug.Log($"{nameof(PrefsDataManager)}: Data [{name}] loaded. Data:\n{json}");
            return data;
        }

        private void SaveFields(string name, FieldData[] fields)
        {
            var json = JsonConvert.SerializeObject(fields, Formatting.Indented);
            _dataProvider.SaveData(name, json);
            Debug.Log($"{nameof(PrefsDataManager)}: Data [{name}] saved. Data:\n{json}");
        }

        private static Type[] GetDataTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(a => a.IsSubclassOf(typeof(BasePrefsData)) && !a.IsAbstract)
                .ToArray();
        }

        public void SaveAll()
        {
            foreach (var data in _repository.Values)
                SaveData(data);
        }

        public void Dispose()
        {
            foreach (var data in _repository.Values)
            {
                SaveData(data);
                data.Dispose();
            }
        }
    }
}