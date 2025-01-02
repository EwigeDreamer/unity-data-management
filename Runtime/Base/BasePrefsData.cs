using System;

namespace ED.PrefsDataManagement.Base
{
    [System.Serializable]
    public abstract class BasePrefsData : IDisposable
    {
        [field: NonSerialized] public event Action OnSave;
        
        public void Save() => OnSave?.Invoke();

        public virtual void OnAfterLoad() { }
        public virtual void OnBeforeSave() { }

        public virtual void Dispose()
        {
            OnSave = null;
        }
    }
}