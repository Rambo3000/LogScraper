using System;
using System.Collections.Generic;
using System.Threading;

namespace LogScraper.Log.LogAppState
{
    /// <summary>
    /// Non-generic holder for the shared synchronization context used by all <see cref="StateSlice{T}"/> instances.
    /// Keeping it here avoids the per-closed-type static field problem of generic classes.
    /// </summary>
    public static class StateSlice
    {
        internal static SynchronizationContext SynchronizationContext { get; private set; }

        /// <summary>
        /// Must be called once from the UI thread before any background processing starts.
        /// Captures the current <see cref="SynchronizationContext"/> so all <see cref="StateSlice{T}.Changed"/>
        /// events are marshalled back to that thread.
        /// </summary>
        public static void SetSynchronizationContext()
        {
            SynchronizationContext = SynchronizationContext.Current;
        }
    }

    /// <summary>
    /// Holds a single piece of observable state.
    /// Raises <see cref="Changed"/> only when the value actually changes.
    /// Always marshals <see cref="Changed"/> to the synchronization context that was
    /// active when <see cref="StateSlice.SetSynchronizationContext"/> was called (typically the UI thread),
    /// so subscribers can safely update controls without needing their own marshal checks.
    /// </summary>
    public sealed class StateSlice<T>
    {
        private T _value;

        public T Value => _value;

        public event EventHandler Changed;

        /// <summary>
        /// Sets the value and fires <see cref="Changed"/> when the new value differs from the current one.
        /// </summary>
        public void Set(T value)
        {
            if (EqualityComparer<T>.Default.Equals(_value, value)) return;
            _value = value;
            RaiseChanged();
        }

        /// <summary>
        /// Forces the value to be set and always fires <see cref="Changed"/>,
        /// regardless of equality. Useful for value types that are reset to a new default instance.
        /// </summary>
        public void ForceSet(T value)
        {
            _value = value;
            RaiseChanged();
        }

        /// <summary>
        /// Fires <see cref="Changed"/> on the captured synchronization context.
        /// If already on that context, or no context was set, fires directly.
        /// Uses <see cref="SynchronizationContext.Post"/> so the background thread
        /// never blocks waiting for UI handlers to finish.
        /// </summary>
        private void RaiseChanged()
        {
            if (Changed == null) return;

            SynchronizationContext ctx = StateSlice.SynchronizationContext;
            if (ctx == null || ctx == SynchronizationContext.Current)
                Changed.Invoke(this, EventArgs.Empty);
            else
                ctx.Post(_ => Changed?.Invoke(this, EventArgs.Empty), null);
        }
    }
}
