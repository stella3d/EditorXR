#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Data;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// For the purpose of interacting with MiniWorlds
	/// </summary>
	interface IMiniWorld
	{
		/// <summary>
		/// Gets the root transform of the miniWorld itself
		/// </summary>
		Transform miniWorldTransform { get; }

        /// <summary>
        /// The boundary of the actual world space shown by the miniworld
        /// </summary>
		Bounds referenceBounds { get; set; }

        /// <summary>
        /// The boundary inside the miniworld tool
        /// </summary>
		Bounds localBounds { get; set; }

		/// <summary>
		/// Tests whether a point is contained within the actual miniWorld bounds (not the reference bounds)
		/// </summary>
		/// <param name="position">World space point to be tested</param>
		/// <returns>True if the point is contained</returns>
		bool Contains(Vector3 position);

		/// <summary>
		/// Gets the reference transform used to represent the origin and size of the space represented within the miniWorld
		/// </summary>
		Transform referenceTransform { get; }

		/// <summary>
		/// Matrix that converts from the mini world space to reference space (which may have scale and translation)
		/// </summary>
		Matrix4x4 GetWorldToCameraMatrix(Camera camera);

		/// <summary>
		/// Sets a list of renderers to be skipped when rendering the MiniWorld
		/// </summary>
		List<Renderer> ignoreList { set; }

        /// <summary>
        /// Sets the structure that contains all the renderers that can potentially be rendered as a miniworld
        /// </summary>
        SpatialHash<Renderer> spatialHash { set; }
	}
}
#endif
