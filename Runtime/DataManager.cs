using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace ED.DataManagement
{
    public class DataManager : IDisposable
    {
        private const string NamePrefix = "player_data";
        
        private readonly IDataProvider _dataProvider;
        
        private readonly Dictionary<Type, BaseData> _repository = new();
        
        public DataManager(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
            foreach (var type in GetDataTypes())
                _repository[type] = LoadData(type);
        }
        
        public List<BaseData> GetAllData() => _repository.Values.ToList();
        
        public T GetData<T>() where T : BaseData, new()
        {
            if (_repository.TryGetValue(typeof(T), out var data))
                return (T)data;
            return (T)(_repository[typeof(T)] = LoadData<T>());
        }

        private T LoadData<T>() where T : BaseData, new()
        {
            var data = new T();
            data.OnSave += () => SaveData(data);
            var fields = LoadFields(GetDataName(typeof(T)));
            SetFields(data, fields);
            data.OnAfterLoad();
            return data;
        }

        private BaseData LoadData(Type type)
        {
            if (!type.IsSubclassOf(typeof(BaseData))) return null;
            var data = (BaseData)Activator.CreateInstance(type);
            data.OnSave += () => SaveData(data);
            var fields = LoadFields(GetDataName(type));
            SetFields(data, fields);
            data.OnAfterLoad();
            return data;
        }

        private void SaveData(BaseData data)
        {
            data.OnBeforeSave();
            var fields = GetFields(data);
            SaveFields(GetDataName(data.GetType()), fields);
        }

        private static string GetDataName(Type type) => $"{NamePrefix}_{type.Name.ToLower()}";

        private static FieldData[] GetFields(BaseData source)
        {
            return source.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(a => !a.GetCustomAttributes<NonSerializedAttribute>().Any())
                .Select(field => new FieldData
                {
                    name = field.Name,
                    type = field.FieldType.FullName,
                    data = JsonConvert.SerializeObject(field.GetValue(source))
                })
                .ToArray();
        }

        private static void SetFields(BaseData target, FieldData[] fields)
        {
            var targetFields = target.GetType()
                .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(a => !a.GetCustomAttributes<NonSerializedAttribute>().Any())
                .ToDictionary(a => a.Name);
            
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
            Debug.Log($"{nameof(DataManager)}: Data [{name}] loaded. Data:\n{json}");
            return data;
        }

        private void SaveFields(string name, FieldData[] fields)
        {
            var json = JsonConvert.SerializeObject(fields, Formatting.Indented);
            _dataProvider.SaveData(name, json);
            Debug.Log($"{nameof(DataManager)}: Data [{name}] saved. Data:\n{json}");
        }

        private static Type[] GetDataTypes()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(a => a.IsSubclassOf(typeof(BaseData)) && !a.IsAbstract)
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