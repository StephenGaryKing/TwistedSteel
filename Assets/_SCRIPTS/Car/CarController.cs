using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[System.Serializable]
	struct Wheel
	{
		public string name;
		public bool hasMotor;
		public bool canSteer;
		public Transform transform;
		public WheelCollider collider;
	}

	[RequireComponent(typeof(PhotonView))]
	public class CarController : MonoBehaviour
	{

		public float m_horizontalInput;
		public float m_verticalInput;
		float m_currentSteeringAngle;
		float m_currentMotorForce;

		[SerializeField] List<Wheel> m_wheels;
		public float m_maxSteerAngle = 30;
		public float m_motorForce = 50;

		public PhotonView m_photonView;

		// Start is called before the first frame update
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		public void ResetCar()
		{
			transform.position += Vector3.up * 2;
			transform.rotation = Quaternion.identity;
		}

		void ApplyInputData()
		{
			m_currentSteeringAngle = m_maxSteerAngle * m_horizontalInput;
			m_currentMotorForce = m_motorForce * m_verticalInput;
		}

		void UpdateWheelPos(Wheel wheel)
		{
			Vector3 pos = wheel.transform.position;
			Quaternion rot = wheel.transform.rotation;

			wheel.collider.GetWorldPose(out pos, out rot);

			wheel.transform.position = pos;
			wheel.transform.rotation = rot;
		}

		void SteerWheel(Wheel wheel)
		{
			if (wheel.canSteer)
				wheel.collider.steerAngle = m_currentSteeringAngle;
		}

		void DriveWheel(Wheel wheel)
		{
			if (wheel.hasMotor)
				wheel.collider.motorTorque = m_currentMotorForce;
		}

		void FixedUpdate()
		{
			ApplyInputData();

			foreach (var wheel in m_wheels)
			{
				// Steer wheels
				SteerWheel(wheel);
				// Drive wheels
				DriveWheel(wheel);
				// Update the visual location of each wheel
				UpdateWheelPos(wheel);
			}
		}
	}
}
