using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[RequireComponent(typeof(CarCombat), typeof(CarController))]
	public abstract class Car : MonoBehaviourPun, IPunObservable
	{
		public GameObject m_explosionPrefab;

		public Hittable m_hittable;
		public Damageable m_damagable;
		protected CarCombat m_carCombat;
		protected CarController m_carController;

		//Networking
		bool m_isAlive = true;

		// Start is called before the first frame update
		protected virtual void Start()
		{
			m_carCombat = GetComponent<CarCombat>();
			m_carController = GetComponent<CarController>();
			if (m_carController.m_photonView.IsMine)
				m_damagable.m_onDeath.AddListener(() => SetAlive(false));
		}

		protected void SetAlive(bool value)
		{
			m_isAlive = value;
			if (value)
				Revive();
			else
				Die();
		}

		void Die()
		{
			m_carCombat.m_rb.isKinematic = true;
			GameObject go = Instantiate(m_explosionPrefab, transform.position, Quaternion.identity);
			Destroy(go, go.GetComponent<ParticleSystem>().main.duration);
			CoroutineManager.Instance.StartCoroutine(WaitToRevive(5));
			m_hittable.gameObject.SetActive(false);
		}

		void Revive()
		{
			m_hittable.gameObject.SetActive(true);
			m_carCombat.m_rb.isKinematic = false;
			m_carController.ResetCar();
			m_damagable.Heal(Mathf.Infinity);
		}

		IEnumerator WaitToRevive(float seconds)
		{
			yield return new WaitForSeconds(seconds);
			SetAlive(true);
		}

		public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.IsWriting)
			{
				//1: Is Alive
				stream.SendNext(m_isAlive);
			}
			else
			{
				//1: Is Alive
				bool isAlive = (bool)stream.ReceiveNext();
				if (isAlive != m_isAlive)
					SetAlive(isAlive);
			}
		}
	}
}