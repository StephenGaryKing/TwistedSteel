using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[RequireComponent(typeof(CarController))]
	public class PlayerCar : Car
	{
		protected override void Start()
		{
			base.Start();
			if (m_carController.m_photonView.IsMine)
				PlayerManger.Instance.m_localPlayerInstance = gameObject;
		}

		void Update()
		{
			if (!m_carController.m_photonView.IsMine && PhotonNetwork.IsConnected)
				return;
			GetInput();
			m_carCombat.CheckFireInput();

			//Debugging
			if (Input.GetKeyDown(KeyCode.Delete))
				SetAlive(false);
		}

		void GetInput()
		{
			m_carController.m_horizontalInput = Input.GetAxis("Horizontal");
			m_carController.m_verticalInput = Input.GetAxis("Vertical");
		}
	}
}