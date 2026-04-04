
using LogScraper.Log.Content;

namespace LogScraper.Log.Rendering
{
    /// <summary>
    /// Settings for rendering a log flow tree.
    /// </summary>
    public class LogFlowTreeRenderSettings(bool showTree, LogContentProperty logContentProperty)
    {
        /// <summary>
        /// Indicates whether the log flow tree should be shown.
        /// </summary>
        public bool ShowTree { get; set; } = showTree;

        /// <summary>
        /// The log content property to use for rendering the log flow tree.
        /// Only log content properties that are marked as IsBeginFlowTreeFilter should be used here.
        /// </summary>
        public LogContentProperty LogContentProperty { get; set; } = logContentProperty;
    }
}
