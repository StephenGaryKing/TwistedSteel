using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
	public static DebugUI Instance;

	public Text m_textBox;

	private void Start()
	{
		if (!Instance)
			Instance = this;
	}

	public void ClearDebug()
	{
		m_textBox.text = "";
	}

	public static void Log(string text, bool addNewLine = true)
	{
		if (text != null)
		{
			Debug.Log(text);
			Instance.m_textBox.text += text;
			if (addNewLine) Instance.m_textBox.text += "\n";
		}
		else
		{
			Log("NULL ERROR");
		}
	}
}
