#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	public class GizmoModule : MonoBehaviour, IUsesViewerScale
	{
		public static GizmoModule instance;

		[SerializeField]
		bool m_DrawingEnabled = true;

		public const float RayLength = 100f;
		const float k_RayWidth = .001f;

		readonly List<Renderer> m_Rays = new List<Renderer>();
		int m_RayCount;

		readonly List<Renderer> m_Spheres = new List<Renderer>();
		int m_SphereCount;

		readonly List<Renderer> m_Cubes = new List<Renderer>();
		int m_CubeCount;

		public Material gizmoMaterial
		{
			get { return m_GizmoMaterial; }
		}
		[SerializeField]
		Material m_GizmoMaterial;

		int m_QueueCount;

		void Awake()
		{
			instance = this;
		}

		void LateUpdate()
		{
			for (int i = m_RayCount; i < m_Rays.Count; i++)
			{
				m_Rays[i].gameObject.SetActive(false);
			}

			for (int i = m_SphereCount; i < m_Spheres.Count; i++)
			{
				m_Spheres[i].gameObject.SetActive(false);
			}

			for (int i = m_CubeCount; i < m_Cubes.Count; i++)
			{
				m_Cubes[i].gameObject.SetActive(false);
			}

			m_SphereCount = 0;
			m_RayCount = 0;
			m_CubeCount = 0;
			m_QueueCount = m_GizmoMaterial.renderQueue;
		}

		public void DrawRay(Vector3 origin, Vector3 direction, Color color, float rayLength = RayLength)
		{
			if (!m_DrawingEnabled)
				return;

			if (direction == Vector3.zero)
				return;

			Renderer ray;
			if (m_Rays.Count > m_RayCount)
			{
				ray = m_Rays[m_RayCount];
			}
			else
			{
				ray = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Renderer>();
				ObjectUtils.Destroy(ray.GetComponent<Collider>());
				ray.transform.parent = transform;
				ray.sharedMaterial = Instantiate(m_GizmoMaterial);
				m_Rays.Add(ray);
			}

			ray.gameObject.SetActive(true);
			ray.sharedMaterial.color = color;
			ray.sharedMaterial.renderQueue = m_QueueCount++;
			var rayTransform = ray.transform;
			var rayWidth = k_RayWidth * this.GetViewerScale();
			rayTransform.localScale = new Vector3(rayWidth, rayWidth, rayLength);
			direction.Normalize();
			rayTransform.position = origin + direction * rayLength * 0.5f;
			rayTransform.rotation = Quaternion.LookRotation(direction);

			m_RayCount++;
		}

		public void DrawSphere(Vector3 center, float radius, Color color)
		{
			if (!m_DrawingEnabled)
				return;

			Renderer sphere;
			if (m_Spheres.Count > m_SphereCount)
			{
				sphere = m_Spheres[m_SphereCount];
			}
			else
			{
				sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<Renderer>();
				sphere.transform.parent = transform;
				sphere.sharedMaterial = Instantiate(m_GizmoMaterial);
				m_Spheres.Add(sphere);
			}

			sphere.gameObject.SetActive(true);
			sphere.sharedMaterial.color = color;
			sphere.sharedMaterial.renderQueue = m_QueueCount++;
			var sphereTransform = sphere.transform;
			sphereTransform.localScale = Vector3.one * radius * this.GetViewerScale();
			sphereTransform.position = center;

			m_SphereCount++;
		}

		public void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale, Color color)
		{
			if (!m_DrawingEnabled)
				return;

			Renderer cube;
			if (m_Cubes.Count > m_CubeCount)
			{
				cube = m_Cubes[m_CubeCount];
			}
			else
			{
				cube = GameObject.CreatePrimitive(PrimitiveType.Cube).GetComponent<Renderer>();
				cube.transform.parent = transform;
				cube.sharedMaterial = Instantiate(m_GizmoMaterial);
				m_Cubes.Add(cube);
			}

			cube.gameObject.SetActive(true);
			cube.sharedMaterial.color = color;
			cube.sharedMaterial.renderQueue = m_QueueCount++;
			var cubeTransform = cube.transform;
			cubeTransform.position = position;
			cubeTransform.rotation = rotation;
			cubeTransform.localScale = scale;

			m_CubeCount++;
		}

		void OnDestroy()
		{
			foreach (var ray in m_Rays)
			{
				ObjectUtils.Destroy(ray.GetComponent<Renderer>().sharedMaterial);
			}

			foreach (var sphere in m_Spheres)
			{
				ObjectUtils.Destroy(sphere.GetComponent<Renderer>().sharedMaterial);
			}

			foreach (var cube in m_Cubes)
			{
				ObjectUtils.Destroy(cube.GetComponent<Renderer>().sharedMaterial);
			}
		}
	}
}
#endif