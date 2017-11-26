#if UNITY_EDITOR
using UnityEditor.Experimental.EditorVR.Core;

namespace UnityEditor.Experimental.EditorVR
{
    /// <summary>
    /// Implement this interface to create an editing context. You can also specify your own custom
    /// settings within the context to be applied to the specified VR editor (e.g. EditorVR).
    /// </summary>
    public interface IEditingContext
    {
        /// <summary>
        /// Name for this specific instance of an editing context
        /// </summary>
        string name { get; }

        /// <summary>
        /// Copy of the scene camera's (component) settings, and relevant image effects
        /// </summary>
        ContextSettings sceneCameraSettingsAndFX { get; }

        /// <summary>
        /// Perform one-time setup for the context when pushed to the stack.
        /// </summary>
        void Setup();

        /// <summary>
        /// Allow the context to dispose of any created objects when popped from the stack.
        /// </summary>
        void Dispose();
    }
}
#endif
