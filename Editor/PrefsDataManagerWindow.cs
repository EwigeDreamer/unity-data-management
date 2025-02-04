#if ODIN_INSPECTOR

using System.Collections.Generic;
using ED.PrefsDataManagement.Base;
using ED.PrefsDataManagement.Interfaces;
using ED.PrefsDataManagement.Logic;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace ED.PrefsDataManagement.Editor
{
    public class PrefsDataManagerWindow : OdinMenuEditorWindow
    {
        private static readonly string DataLostMessage = $"The {nameof(PrefsDataManager)} has been lost.\nPlease, close this window and open again.";
        
        private IPrefsDataManager _manager;
        private readonly HashSet<BasePrefsData> _dirty = new();
        private GUIStyle _toolbarStyle;
        
        protected override void Initialize()
        {
            _toolbarStyle = new GUIStyle(SirenixGUIStyles.ToolbarButton);
            _toolbarStyle.richText = true;
            EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
        }

        public static void OpenWindow(IPrefsDataManager manager)
        {
            if (manager == null) return;
            var window = GetWindow<PrefsDataManagerWindow>();
            window._manager = manager;
            window.Show();
        }

        protected override void OnImGUI()
        {
            if (_manager != null) base.OnImGUI();
            else SirenixEditorGUI.MessageBox(DataLostMessage, MessageType.Warning);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            foreach (var data in _manager.GetAllData())
                tree.Add(data.GetType().Name, data);
            return tree;
        }

        protected override void DrawEditor(int index)
        {
            var target = (BasePrefsData)CurrentDrawingTargets[index];
            var isDirty = _dirty.Contains(target);
            
            SirenixEditorGUI.BeginHorizontalToolbar();
            {
                GUILayout.FlexibleSpace();
                
                if (GUILayout.Button(isDirty ? "<b>Save*</b>" : "Save", _toolbarStyle ?? SirenixGUIStyles.ToolbarButton))
                {
                    target.Save();
                    _dirty.Remove(target);
                }
            }
            SirenixEditorGUI.EndHorizontalToolbar();
            
            using (var checkScope = new EditorGUI.ChangeCheckScope())
            {
                base.DrawEditor(index);
                if (checkScope.changed)
                    _dirty.Add(target);
            }
        }

        private void OnPlaymodeStateChanged(PlayModeStateChange _)
        {
            _manager = null;
        }

        protected override void OnDestroy()
        {
            _manager = null;
            _dirty?.Clear();
            EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
        }
    }
}

#endif
