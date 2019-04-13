using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[System.Serializable]
	public enum MATERIAL_TYPE
	{
		Metal,
		Wood,
		Flesh
	}

	[System.Serializable]
	struct MaterialData
	{
		public MATERIAL_TYPE type;
		public Material data;
	}

	[System.Serializable]
	public struct Material
	{
		public GameObject hitParticle;
	}

	public class MaterialManager : MonoBehaviour
	{
		public static MaterialManager Instance;
		[SerializeField]
		List<MaterialData> m_materials;
		public Dictionary<MATERIAL_TYPE, Material> Materials { get; private set; }

		private void Start()
		{
			if (!Instance)
				Instance = this;

			Materials = new Dictionary<MATERIAL_TYPE, Material>();
			foreach (var material in m_materials)
			{
				if (!Materials.ContainsKey(material.type))
				{
					Material newMaterial = material.data;
					Materials.Add(material.type, newMaterial);
				}
				else
					Debug.LogError("ONLY ONE MATERIAL TYPE ALLOWED\nPlease remove the material : " + material.type.ToString());
			}
		}
	}
}