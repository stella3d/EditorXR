#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	public interface IUsesGizmos
	{
	}

	public static class IUsesGizmosMethods
	{
		internal static Action<Vector3, Vector3, Color, float> drawRay { private get; set; }
		internal static Action<Vector3, float, Color> drawSphere { private get; set; }
		internal static Action<Vector3, Quaternion, Vector3, Color> drawCube { private get; set; }

		/// <summary>
		/// Draw a ray for debug purposes
		/// </summary>
		/// <param name="origin">The origin of the ray</param>
		/// <param name="direction">The direction of the ray</param>
		/// <param name="color">The color to use when drawing the ray</param>
		/// <param name="rayLength">The length of the ray</param>
		public static void DrawRay(this IUsesGizmos usesGizmos, Vector3 origin, Vector3 direction, Color color, float rayLength = GizmoModule.RayLength)
		{
			drawRay(origin, direction, color, rayLength);
		}

		/// <summary>
		/// Draw a sphere for debug purposes
		/// </summary>
		/// <param name="center">The center of the sphere</param>
		/// <param name="radius">The radius of the sphere</param>
		/// <param name="color">The color to use when drawing the sphere</param>
		public static void DrawSphere(this IUsesGizmos usesGizmos, Vector3 center, float radius, Color color)
		{
			drawSphere(center, radius, color);
		}

		/// <summary>
		/// Draw a cube for debug purposes
		/// </summary>
		/// <param name="center">The center of the cube</param>
		/// <param name="scale">The scale of the cube</param>
		/// <param name="color">The color to use when drawing the cube</param>
		public static void DrawCube(this IUsesGizmos usesGizmos, Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			drawCube(position, rotation, scale, color);
		}
	}
}
#endif