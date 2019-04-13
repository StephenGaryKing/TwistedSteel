using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TwistedSteel
{
	public class Damageable : MonoBehaviour
	{
		public float m_maxHealth = 100;
		[HideInInspector]
		public float m_health;

		public UnityEvent m_onHeal;
		public UnityEvent m_onMaxHealth;
		public UnityEvent m_onDamage;
		public UnityEvent m_onDeath;

		protected virtual void Start()
		{
			m_health = m_maxHealth;
		}

		public virtual void Damage(float value)
		{
			m_health -= value;
			m_onDamage.Invoke();
			if (m_health <= 0)
				if (m_onDeath != null)
					m_onDeath.Invoke();
		}

		public virtual void Heal(float value)
		{
			m_health += value;
			if (m_health > m_maxHealth)
			{
				m_health = m_maxHealth;
				m_onMaxHealth.Invoke();
			}
		}
	}
}