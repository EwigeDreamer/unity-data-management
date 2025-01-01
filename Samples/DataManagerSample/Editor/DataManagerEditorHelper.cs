using ED.DataManagement.Editor;
using UnityEditor;

namespace ED.DataManagement.Samples.Editor
{
    public static class DataManagerEditorHelper
    {
        [MenuItem("Tools/Custom/DataManager")]
        private static void OpenEditorWindow()
        {
            if (DataManagerEntryPoint.Instance != null)
                DataManagerWindow.OpenWindow(DataManagerEntryPoint.Instance.DataManager);
            else
                DataManagerWindow.OpenWindow(new DataManager(new PrefsDataProvider()));
        }
    }
}