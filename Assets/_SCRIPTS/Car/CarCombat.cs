using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace TwistedSteel
{
	[RequireComponent(typeof(Rigidbody),typeof(CarController))]
	public class CarCombat : MonoBehaviourPun, IPunObservable
	{
		class Gun
		{
			public LineRenderer m_line;
			public List<Bullet> m_bullets = new List<Bullet>();
		}

		class Bullet
		{
			public Vector3 forward = Vector3.forward;
			public Vector3 position = Vector3.zero;
		}

		const int GUN_DAMAGE = 1;

		public float m_bashForce = 10;
		public LineRenderer[] m_gunLineRenderers;
		[Range(0.1f, 10f)]
		public float m_segmentLength;
		[Range(2f, 50f)]
		public int m_maxNumberOfSegments;
		List<Gun> m_guns = new List<Gun>();
		public Hittable m_hittable;
		[HideInInspector]
		public Rigidbody m_rb;

		//Networking
		CarController m_carController;
		float m_startTime;
		bool m_shooting = false;

		private void Awake()
		{
			m_startTime = Time.time;
			m_rb = GetComponent<Rigidbody>();
			m_carController = GetComponent<CarController>();
			if (m_carController.m_photonView.IsMine)
				DebugUI.Log("My Number is " + m_carController.m_photonView.Owner);
			foreach (var line in m_gunLineRenderers)
				m_guns.Add(new Gun() { m_line = line });
		}

		void OnEnable()
		{
			SetShooting(false);
		}

		private void Update()
		{
			if (m_shooting)
				RenderBullets();
		}

		//Shooting
		public void CheckFireInput()
		{
			if (Input.GetButtonDown("Fire1"))
			{
				UpdateLineRenderers(true);
				SetShooting(true);
			}
			if (Input.GetButtonUp("Fire1"))
			{
				UpdateLineRenderers(false);
				SetShooting(false);
			}
		}

		void SetShooting(bool value)
		{
			m_shooting = value;
			UpdateLineRenderers(value);
		}

		void UpdateLineRenderers(bool TurnOn)
		{
			if (TurnOn)
				foreach (var gun in m_guns)
				{
					gun.m_line.enabled = true;
					gun.m_bullets.Add(new Bullet() { forward = transform.forward, position = gun.m_line.transform.position });
				}
			else
				foreach (var gun in m_guns)
				{
					gun.m_line.enabled = false;
					gun.m_bullets.Clear();
				}
		}

		void RenderBullets()
		{
			foreach (var gun in m_guns)
			{
				while (gun.m_bullets.Count <= m_maxNumberOfSegments)
				{
					foreach (var bullet in gun.m_bullets)
						bullet.position += bullet.forward * m_segmentLength;

					gun.m_bullets.Add(new Bullet() { forward = transform.forward, position = gun.m_line.transform.position });
				}

				if (gun.m_bullets.Count > m_maxNumberOfSegments)
					gun.m_bullets.RemoveAt(0);
			}

			//Generate bullet locations
			foreach (var gun in m_guns)
			{
				List<Vector3> bulletLocations = new List<Vector3>();
				bulletLocations.Add(gun.m_line.transform.position);

				for (int i = gun.m_bullets.Count - 1; i >= 0; i--)
				{
					Bullet bullet = gun.m_bullets[i];
					//perform a hit test for other cars/terrain
					Vector3 hitPos = DoBulletHitTest(bulletLocations[bulletLocations.Count - 1], bullet.position);
					bulletLocations.Add(hitPos);

					if (hitPos != bullet.position)
						break;
				}
				gun.m_line.positionCount = bulletLocations.Count;
				gun.m_line.SetPositions(bulletLocations.ToArray());
			}
		}

		//Perform a segmented stepped raycast to catch any objects that are in the path of the bullets
		Vector3 DoBulletHitTest(Vector3 startpos, Vector3 endPos)
		{
			Vector3 dir = endPos - startpos;
			Ray ray = new Ray(startpos, dir.normalized);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				if (hit.collider.gameObject != gameObject)
				{ 
					if (hit.distance <= dir.magnitude)
					{
						//Spawn a hit particle
						Hittable other = hit.collider.GetComponent<Hittable>();
						if (other)
							other.Hit(hit, GUN_DAMAGE);
						return hit.point;
					}
				}
			}
			return endPos;
		}

		/*
		//Ramming
		private void OnCollisionEnter(Collision collision)
		{
			if (m_controller.m_photonView.IsMine)
			{
				if (Time.time > m_startTime + 5)
				{
					Debug.Log(m_controller);
					if (collision.collider.gameObject != gameObject && m_controller != null && m_controller.m_photonView.IsMine)
					{
						if (collision.collider.CompareTag("Player"))
						{
							Vector3 moveVector = (transform.position - collision.transform.position).normalized;
							float bashForce = m_bashForce * m_controller.m_rb.velocity.magnitude;
							CarCombat other = collision.gameObject.GetComponent<CarCombat>();
							DebugUI.Log(other.photonView.Owner + " Was Hit By " + m_controller.m_photonView.Owner);
							m_bumpForce = -moveVector * bashForce;
							ApplyForce();
							m_shouldSend = true;
						}
					}
				}
			}
		}

		void ApplyForce()
		{
			m_controller.m_rb.AddForce(m_bumpForce, ForceMode.Impulse);
			m_bumpForce = Vector3.zero;
		}
		*/

		//Networking
		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				//1: shooting
				stream.SendNext(m_shooting);
			}
			else
			{
				//1: shooting
				bool shooting = (bool)stream.ReceiveNext();
				if (shooting != m_shooting)
					SetShooting(shooting);
			}
		}
	}
}