using System;

namespace Orego.Util
{
    public sealed class Data<T>
    {
        #region Event

        public event Action<object, T> OnValueChangedEvent;

        #endregion

        public T value
        {
            get { return this._value; }
            set { this.SetValue(null, value); }
        }

        private T _value;

        public Data()
        {
        }

        public Data(T value)
        {
            this._value = value;
        }

        public void SetValue(object sender, T value)
        {
            this._value = value;
            this.OnValueChangedEvent?.Invoke(null, value);
        }
    }
}