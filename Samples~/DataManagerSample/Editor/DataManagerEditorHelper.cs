#if ODIN_INSPECTOR

using ED.PrefsDataManagement.Editor;
using ED.PrefsDataManagement.Logic;
using UnityEditor;

namespace ED.PrefsDataManagement.Samples.Editor
{
    public static class DataManagerEditorHelper
    {
        [MenuItem(Constants.EditorButtonPath)]
        private static void OpenEditorWindow()
        {
            if (DataManagerEntryPoint.Instance != null)
                PrefsDataManagerWindow.OpenWindow(DataManagerEntryPoint.Instance.DataManager);
            else
                PrefsDataManagerWindow.OpenWindow(new PrefsDataManager(new UnityPrefsDataProvider()));
        }
    }
}

#endif