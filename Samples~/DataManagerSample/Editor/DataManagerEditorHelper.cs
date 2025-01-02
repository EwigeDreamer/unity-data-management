#if ODIN_INSPECTOR

using ED.DataManagement.Editor;
using ED.DataManagement.Logic;
using UnityEditor;

namespace ED.DataManagement.Samples.Editor
{
    public static class DataManagerEditorHelper
    {
        [MenuItem(Constants.EditorButtonPath)]
        private static void OpenEditorWindow()
        {
            if (DataManagerEntryPoint.Instance != null)
                DataManagerWindow.OpenWindow(DataManagerEntryPoint.Instance.DataManager);
            else
                DataManagerWindow.OpenWindow(new DataManager(new PrefsDataProvider()));
        }
    }
}

#endif