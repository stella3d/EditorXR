#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provide the ability to request a new unique stencil ref value
	/// </summary>
	public interface IRequestStencilRef : IInjectedFunctionality<IRequestStencilRefProvider>
	{
	}

	public interface IRequestStencilRefProvider
	{
		/// <summary>
		/// Get a new unique stencil ref value
		/// </summary>
		byte RequestStencilRef();
	}

	public static class IRequestStencilRefMethods
	{
		/// <summary>
		/// Get a new unique stencil ref value
		/// </summary>
		public static byte RequestStencilRef(this IRequestStencilRef obj)
		{
			return obj.provider.RequestStencilRef();
		}
	}
}
#endif
