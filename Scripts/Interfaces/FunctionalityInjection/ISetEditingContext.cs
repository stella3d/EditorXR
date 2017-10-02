#if UNITY_EDITOR
using System.Collections.Generic;

namespace UnityEditor.Experimental.EditorVR
{
	public interface ISetEditingContext : IInjectedFunctionality<ISetEditingContextProvider>
	{
	}

	public interface ISetEditingContextProvider
	{
		/// <summary>
		/// Get the currently available editing contexts
		/// NOTE: Dynamic contexts can be added to the list to make them available
		/// </summary>
		/// <returns>List of the currently available editing contexts</returns>
		List<IEditingContext> GetAvailableEditingContexts();

		/// <summary>
		/// Get the previous editing contexts that were set
		/// </summary>
		/// <returns>List of the previous editing contexts (last one used is first in list)</returns>
		List<IEditingContext> GetPreviousEditingContexts();

		/// <summary>
		/// Set the editing context, which will dispose of the current editing context
		/// </summary>
		/// <param name="context">The editing context to use</param>
		void SetEditingContext(IEditingContext context);

		/// <summary>
		/// Restore the previous editing context, which will dispose of the current editing context
		/// </summary>
		void RestorePreviousEditingContext();
	}

	public static class ISetEditingContextMethods
	{
		/// <summary>
		/// Get the currently available editing contexts
		/// NOTE: Dynamic contexts can be added to the list to make them available
		/// </summary>
		/// <returns>List of the currently available editing contexts</returns>
		public static List<IEditingContext> GetAvailableEditingContexts(this ISetEditingContext @this)
		{
			return @this.provider.GetAvailableEditingContexts();
		}

		/// <summary>
		/// Get the previous editing contexts that were set
		/// </summary>
		/// <returns>List of the previous editing contexts (last one used is first in list)</returns>
		public static List<IEditingContext> GetPreviousEditingContexts(this ISetEditingContext @this)
		{
			return @this.provider.GetPreviousEditingContexts();
		}

		/// <summary>
		/// Set the editing context, which will dispose of the current editing context
		/// </summary>
		/// <param name="context">The editing context to use</param>
		public static void SetEditingContext(this ISetEditingContext @this, IEditingContext context)
		{
			@this.provider.SetEditingContext(context);
		}

		/// <summary>
		/// Restore the previous editing context, which will dispose of the current editing context
		/// </summary>
		public static void RestorePreviousEditingContext(this ISetEditingContext @this)
		{
			@this.provider.RestorePreviousEditingContext();
		}
	}

}
#endif
