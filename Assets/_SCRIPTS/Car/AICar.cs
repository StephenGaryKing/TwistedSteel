using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[RequireComponent(typeof(CarController))]
	public class AICar : Car
	{
		const float MAX_TURN_ANGLE = 100f;
		public Transform m_target;

		protected override void Start()
		{
			base.Start();
		}

		void Update()
		{
			if (!m_target && PlayerManger.Instance.m_localPlayerInstance)
			{
				m_target = PlayerManger.Instance.m_localPlayerInstance.transform;
			}
			if (m_target != null)
			{
				Vector3 targetVector = m_target.position;
				targetVector.y = transform.position.y;
				targetVector -= transform.position;

				float angle = Vector3.SignedAngle(transform.forward, targetVector, Vector3.up);
				float turn = 0;
				float force = 0;

				turn = Mathf.Clamp(angle, -1, 1);
				Debug.Log(turn);

				force = 1.5f - Mathf.Abs(turn);

				if (Mathf.Abs(angle) > MAX_TURN_ANGLE)
				{
					//turn *= -1f;
					//force *= -1f;
					force = 1;
				}

				m_carController.m_horizontalInput = turn;
				m_carController.m_verticalInput = force;
			}
		}
	}
}