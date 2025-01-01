using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace ED.DataManagement.Editor
{
    public class DataManagerWindow : OdinMenuEditorWindow
    {
        private readonly Dictionary<DataBase, DataHandler> _handlers = new();
        
        public static void OpenWindow(DataManager manager)
        {
            if (manager == null) return;
            var window = GetWindow<DataManagerWindow>();
            foreach (var data in manager.GetAllData())
            {
                var handler = CreateInstance<DataHandler>();
                handler.data = data;
                window._handlers[data] = handler;
            }
            window.Show();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (var handler in _handlers.Values)
                DestroyImmediate(handler);
            _handlers.Clear();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            foreach (var pair in _handlers)
                tree.Add(pair.Key.GetType().Name, pair.Value);
            return tree;
        }

        protected override void DrawEditor(int index)
        {
            base.DrawEditor(index);
        }

        [System.Serializable]
        private class DataHandler : ScriptableObject
        {
            [HideLabel] [HideReferenceObjectPicker] [ShowInInspector]
            public DataBase data;
        }
    }
}
