using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(WheelCollider))]
public class CustomWheelCollider : MonoBehaviour
{
	[SerializeField] AnimationCurve m_frictionCurve;
	WheelCollider m_wheelCollider;

	private void OnValidate()
	{
		//m_wheelCollider.
	}
}
