#if UNITY_EDITOR

namespace UnityEditor.Experimental.EditorVR
{
	public interface IInjectedFunctionality<TProvider>
	{
		TProvider provider { get; set; }
	}
}
#endif
