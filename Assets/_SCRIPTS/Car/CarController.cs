using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct Wheel
{
	public string name;
	public bool hasMotor;
	public bool canSteer;
	public Transform transform;
	public WheelCollider collider;
}

[RequireComponent(typeof(Rigidbody), typeof(PhotonView))]
public class CarController : MonoBehaviour, IPunObservable
{

	float m_horizontalInput;
	float m_verticalInput;
	float m_currentSteeringAngle;
	float m_currentMotorForce;

	[SerializeField] List<Wheel> m_wheels;
	[SerializeField] float m_maxSteerAngle = 30;
	[SerializeField] float m_motorForce = 50;

	public PhotonView m_photonView;
	Rigidbody m_rb;

	// Start is called before the first frame update
	void Awake()
    {
		m_rb = GetComponent<Rigidbody>();

		if (m_photonView.IsMine)
			PlayerManger.Instance.m_localPlayerInstance = gameObject;

		DontDestroyOnLoad(gameObject);
	}

	void GetInput()
	{
		m_horizontalInput = Input.GetAxis("Horizontal");
		m_verticalInput = Input.GetAxis("Vertical");
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

	void Update()
	{
		if (!m_photonView.IsMine && PhotonNetwork.IsConnected)
			return;
		GetInput();
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

	#region IPunObservable Implementation
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		
	}
	#endregion
}
