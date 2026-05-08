using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LogScraper.Utilities
{
    /// <summary>
    /// Named application shortcuts. Decoupled from key bindings to allow future configurability.
    /// </summary>
    public enum AppShortcut
    {
        ToggleCompactView,
        FocusSearch,
        SearchNext,
        SearchPrevious,
        SearchAll,
        StartRecording,
        StartTimedRecording,
        StopRecording,
        ToggleBookmark,
        NextBookmark,
        PreviousBookmark,
        ClearBookmarks,
        ClearLog,
        ResetApplication,
        OpenConfiguration,
        CloseBottomPanel,
        SaveLog,
        OpenLogInEditor,
        CollapseExpandProvider,
        ToggleFlowTree,
        SetRangeBegin,
        SetRangeEnd,
        ClearRange,
        ToggleErrorsPanel,
        ResetAllFilters,
        ToggleMetadata,
        PrettyPrint,
        TimelineZoomIn,
        TimelineZoomOut,
    }

    /// <summary>
    /// Manages application-wide keyboard shortcuts.
    /// Key bindings are mapped to <see cref="AppShortcut"/> values, and controls register
    /// handlers for the shortcuts they care about.
    /// </summary>
    public static class ShortcutManager
    {
        private sealed class ShortcutHandlerRegistration(Control owner, Action handler)
        {
            public Control Owner { get; } = owner;
            public Action Handler { get; } = handler;
        }

        // Default key bindings. Replace values here (or load from config) to make shortcuts configurable.
        private static readonly Dictionary<Keys, AppShortcut> _keyBindings = new()
        {
            { Keys.Control | Keys.F, AppShortcut.FocusSearch },
            { Keys.F3, AppShortcut.SearchNext },
            { Keys.Shift | Keys.F3, AppShortcut.SearchPrevious },
            { Keys.Control | Keys.Shift | Keys.F, AppShortcut.SearchAll },
            { Keys.F5, AppShortcut.StartRecording },
            { Keys.Control | Keys.F5, AppShortcut.StartTimedRecording },
            { Keys.Shift | Keys.F5, AppShortcut.StopRecording },
            { Keys.Control | Keys.B, AppShortcut.ToggleBookmark },
            { Keys.F2, AppShortcut.NextBookmark },
            { Keys.Shift | Keys.F2, AppShortcut.PreviousBookmark },
            { Keys.Control | Keys.Shift | Keys.B, AppShortcut.ClearBookmarks },
            { Keys.Control | Keys.L, AppShortcut.ClearLog },
            { Keys.Control | Keys.Shift | Keys.L, AppShortcut.ResetApplication },
            { Keys.Control | Keys.Oemcomma, AppShortcut.OpenConfiguration },
            { Keys.Control | Keys.W, AppShortcut.ToggleCompactView },
            { Keys.Escape, AppShortcut.CloseBottomPanel },
            { Keys.Control | Keys.S, AppShortcut.SaveLog },
            { Keys.Control | Keys.E, AppShortcut.OpenLogInEditor },
            { Keys.Control | Keys.Shift | Keys.P, AppShortcut.CollapseExpandProvider },
            { Keys.Control | Keys.T, AppShortcut.ToggleFlowTree },
            { Keys.Control | Keys.Left, AppShortcut.SetRangeBegin },
            { Keys.Control | Keys.Right, AppShortcut.SetRangeEnd },
            { Keys.Control | Keys.Down, AppShortcut.ClearRange },
            { Keys.Control | Keys.Shift | Keys.E, AppShortcut.ToggleErrorsPanel },
            { Keys.Control | Keys.R, AppShortcut.ResetAllFilters },
            { Keys.Control | Keys.M, AppShortcut.ToggleMetadata },
            { Keys.Control | Keys.P, AppShortcut.PrettyPrint },
            { Keys.Oemplus, AppShortcut.TimelineZoomIn },
            { Keys.OemMinus, AppShortcut.TimelineZoomOut },
        };

        private static readonly Dictionary<AppShortcut, List<ShortcutHandlerRegistration>> _handlers = new();

        /// <summary>
        /// Registers a handler to be invoked when the given shortcut is triggered.
        /// Multiple handlers can be registered for the same shortcut.
        /// </summary>
        public static void Register(Control owner, AppShortcut shortcut, Action handler)
        {
            ArgumentNullException.ThrowIfNull(owner);
            ArgumentNullException.ThrowIfNull(handler);

            if (!_handlers.TryGetValue(shortcut, out List<ShortcutHandlerRegistration> list))
            {
                list = [];
                _handlers[shortcut] = list;
            }

            if (list.Any(registration => ReferenceEquals(registration.Owner, owner) && registration.Handler == handler)) return;

            list.Add(new ShortcutHandlerRegistration(owner, handler));
        }

        /// <summary>
        /// Unregisters a previously registered handler.
        /// </summary>
        public static void Unregister(Control owner, AppShortcut shortcut, Action handler)
        {
            if (!_handlers.TryGetValue(shortcut, out List<ShortcutHandlerRegistration> list)) return;

            list.RemoveAll(registration => ReferenceEquals(registration.Owner, owner) && registration.Handler == handler);
        }

        public static void UnregisterAll(Control owner)
        {
            foreach (List<ShortcutHandlerRegistration> list in _handlers.Values)
                list.RemoveAll(registration => ReferenceEquals(registration.Owner, owner));
        }

        /// <summary>
        /// Processes a key combination. Call this from <see cref="System.Windows.Forms.Form.ProcessCmdKey"/>.
        /// Returns <c>true</c> if the key was handled, <c>false</c> otherwise.
        /// </summary>
        public static bool ProcessKey(Control context, Keys keyData)
        {
            if (!_keyBindings.TryGetValue(keyData, out AppShortcut shortcut))
                return false;

            if (!_handlers.TryGetValue(shortcut, out List<ShortcutHandlerRegistration> list) || list.Count == 0)
                return false;

            Form activeForm = context as Form ?? context?.FindForm();
            if (activeForm == null) return false;

            List<ShortcutHandlerRegistration> applicableHandlers = [];
            for (int i = list.Count - 1; i >= 0; i--)
            {
                ShortcutHandlerRegistration registration = list[i];
                Control owner = registration.Owner;
                if (owner.IsDisposed)
                {
                    list.RemoveAt(i);
                    continue;
                }

                Form ownerForm = owner as Form ?? owner.FindForm();
                if (ownerForm != activeForm || !owner.Visible) continue;

                applicableHandlers.Add(registration);
            }

            if (applicableHandlers.Count == 0) return false;

            foreach (ShortcutHandlerRegistration registration in applicableHandlers)
                registration.Handler();

            return true;
        }
    }
}
