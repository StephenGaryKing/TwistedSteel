using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineManager : MonoBehaviour
{
	public static CoroutineManager Instance;

	private void Start()
	{
		if (!Instance)
			Instance = this;
	}

	public Coroutine StartCoroutine(Coroutine coroutine)
	{
		return StartCoroutine(coroutine);
	}
}
