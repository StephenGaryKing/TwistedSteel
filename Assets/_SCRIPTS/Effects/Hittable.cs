using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwistedSteel
{
	[RequireComponent(typeof(Collider))]
	public class Hittable : MonoBehaviour
	{
		public MATERIAL_TYPE m_material;
		Damageable m_damagable;

		public UnityEvent m_onHit;

		private void Start()
		{
			m_damagable = GetComponent<Damageable>();
		}

		public virtual void Hit(RaycastHit hit, int damage)
		{
			if (m_damagable)
				m_damagable.Damage(damage);
			m_onHit.Invoke();
			GameObject go = Instantiate(MaterialManager.Instance.Materials[m_material].hitParticle, hit.collider.transform);
			go.transform.position = hit.point;
			go.transform.forward = hit.normal;
			Destroy(go, go.GetComponent<ParticleSystem>().main.duration);
		}
	}
}