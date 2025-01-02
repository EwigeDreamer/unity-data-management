using System;

namespace ED.DataManagement
{
    [System.Serializable]
    public abstract class BaseData : IDisposable
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