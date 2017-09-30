#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	public interface IRequestFeedback : IInjectedFunctionality<IRequestFeedbackProvider>
	{
	}

	public interface IRequestFeedbackProvider
	{
		/// <summary>
		/// Add a feedback request to the system
		/// </summary>
		/// <param name="request">The feedback request</param>
		void AddFeedbackRequest(FeedbackRequest request);

		/// <summary>
		/// Remove a feedback request from the system
		/// </summary>
		/// <param name="request">The feedback request</param>
		void RemoveFeedbackRequest(FeedbackRequest request);

		/// <summary>
		/// Clear all feedback requests submitted by this caller from the system
		/// </summary>
		/// <param name="caller">The caller object</param>
		void ClearFeedbackRequests(IRequestFeedback caller);
	}

	public static class IRequestFeedbackMethods
	{
		/// <summary>
		/// Add a feedback request to the system
		/// </summary>
		/// <param name="this">The caller object</param>
		/// <param name="request">The feedback request</param>
		public static void AddFeedbackRequest(this IRequestFeedback @this, FeedbackRequest request)
		{
			request.caller = @this;
			@this.provider.AddFeedbackRequest(request);
		}

		/// <summary>
		/// Remove a feedback request from the system
		/// </summary>
		/// <param name="this">The caller object</param>
		/// <param name="request">The feedback request</param>
		public static void RemoveFeedbackRequest(this IRequestFeedback @this, FeedbackRequest request)
		{
			request.caller = @this;
			@this.provider.RemoveFeedbackRequest(request);
		}

		/// <summary>
		/// Clear all feedback requests submitted by this caller from the system
		/// </summary>
		/// <param name="this">The caller object</param>
		public static void ClearFeedbackRequests(this IRequestFeedback @this)
		{
			@this.provider.ClearFeedbackRequests(@this);
		}
	}
}
#endif
