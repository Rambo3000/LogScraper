using System;
using System.Collections.Generic;

namespace LogScraper.Log.LogAppState
{
    /// <summary>
    /// Holds a single piece of observable state.
    /// Raises <see cref="Changed"/> only when the value actually changes.
    /// </summary>
    public sealed class StateSlice<T>
    {
        private T _value;

        public T Value
        {
            get => _value;
        }

        public event EventHandler Changed;

        /// <summary>
        /// Sets the value and fires <see cref="Changed"/> when the new value differs from the current one.
        /// </summary>
        public void Set(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            _value = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Forces the value to be set and always fires <see cref="Changed"/>,
        /// regardless of equality. Useful for value types that are reset to a new default instance.
        /// </summary>
        public void ForceSet(T value)
        {
            _value = value;
            Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}
