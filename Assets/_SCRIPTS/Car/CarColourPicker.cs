using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwistedSteel
{
	[RequireComponent(typeof(Renderer))]
	public class CarColourPicker : MonoBehaviour
	{
		public CarController m_controller;
		Renderer m_ren;

		// Start is called before the first frame update
		void Start()
		{
			m_ren = GetComponent<Renderer>();
			Color newColor = new Color();

			int num = m_controller.m_photonView.Owner.ActorNumber * 1000;

			ColorUtility.TryParseHtmlString(IntToARGB(HashCode((num * num * num * num * num).ToString())), out newColor);
			m_ren.material.color = newColor;
		}

		// Hash any string into an integer value
		// Then we'll use the int and convert to hex.
		int HashCode(string str)
		{
			if (str != null)
				DebugUI.Log(str);

			var hash = 0;
			for (var i = 0; i < str.Length; i++)
			{
				hash = str[i] + ((hash << 5) - hash);
			}
			return hash;
		}

		// Convert an int to hexadecimal with a max length
		// of six characters.
		string IntToARGB(int i)
		{
			string hex = ((i >> 24) & 0xFF).ToString("X") +
					((i >> 16) & 0xFF).ToString("X") +
					((i >> 8) & 0xFF).ToString("X") +
					(i & 0xFF).ToString("X");
			// Sometimes the string returned will be too short so we 
			// add zeros to pad it out, which later get removed if
			// the length is greater than six.
			hex += "000000";
			return "#" + hex.Substring(0, 6);
		}
	}
}