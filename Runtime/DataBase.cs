using System;

namespace ED.DataManagement
{
    public abstract class DataBase : IDisposable
    {
        public event Action OnSave;
        
        public void Save() => OnSave?.Invoke();

        public void Dispose()
        {
            OnSave = null;
        }
    }
}