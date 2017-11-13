#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
    /// <summary>
    /// Gives decorated class the ability to be updated in a uniform manner
    /// </summary>
    public interface IAnimateTransition
    {
        /// <summary>
        /// Normalized value representing the transition progress, 0-1
        /// </summary>
        float currentTransitionAmount { get; }

        /// <summary>
        /// Target value towards which the currentTransitionAmount should be incremented towards
        /// </summary>
        float targetTransitionAmount { get; }

        /// <summary>
        /// Bool representing the completion status of the transition
        /// </summary>
        bool complete { get; }

        /// <summary>
        /// Ticks/updates the animated transition towards the target transition amount
        /// </summary>
        /// <param name="deltaUpdateAmount">The delta-time based amount with which to tick an animated transition</param>
        void Update(float deltaUpdateAmount);
    }
}
#endif
