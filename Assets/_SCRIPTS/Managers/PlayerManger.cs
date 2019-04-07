using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManger : MonoBehaviour
{
	public static PlayerManger Instance;
	[HideInInspector]
	public GameObject m_localPlayerInstance;

	private void Awake()
	{
		if (!Instance)
			Instance = this;
	}
}
