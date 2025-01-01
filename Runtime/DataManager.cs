using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace ED.DataManagement
{
    public class DataManager : IDisposable
    {
        private const string NamePrefix = "player_data";
        
        private readonly IDataProvider _dataProvider;
        
        private readonly Dictionary<Type, DataBase> _repository = new();
        
        public DataManager(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        
        public T GetData<T>() where T : DataBase, new()
        {
            if (_repository.TryGetValue(typeof(T), out var data))
                return (T)data;
            return (T)(_repository[typeof(T)] = LoadData<T>());
        }

        private T LoadData<T>() where T : DataBase, new()
        {
            var data = new T();
            data.OnSave += () => SaveData(data);
            var fields = LoadFields(GetDataName(typeof(T)));
            SetFields(data, fields);
            return data;
        }

        public void SaveData<T>() where T : DataBase
        {
            if (_repository.TryGetValue(typeof(T), out var data))
                SaveData(data);
        }

        private void SaveData(DataBase data)
        {
            var fields = GetFields(data);
            SaveFields(GetDataName(data.GetType()), fields);
        }

        private static string GetDataName(Type type) => $"{NamePrefix}_{type.Name.ToLower()}";

        private static FieldData[] GetFields(DataBase source)
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

        private static void SetFields(DataBase target, FieldData[] fields)
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
            return data;
        }

        private void SaveFields(string name, FieldData[] fields)
        {
            var json = JsonConvert.SerializeObject(fields);
            _dataProvider.SaveData(name, json);
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