using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonRealtimeManager : MonoBehaviourPunCallbacks
{
	public static PhotonRealtimeManager Instance;
	public Photon.Pun.UtilityScripts.ConnectAndJoinRandom connectAndJoinRandom;
	public string m_gameVersion = "D00";
	public byte maxPlayersPerRoom = 10;
	public GameObject m_playerPrefab;
	public GameObject m_aiPrefab;

	public bool Connected
	{
		get
		{
			return PhotonNetwork.InRoom;
		}
	}

	void Awake()
	{
		if (!Instance)
			Instance = this;
	}

	public void ConnectToServer()
	{
		PhotonNetwork.GameVersion = m_gameVersion;
		PhotonNetwork.ConnectUsingSettings();
		//connectAndJoinRandom.ConnectNow();
	}

	public override void OnConnected()
	{
		DebugUI.Log("Version number is " + m_gameVersion);
		DebugUI.Log("Connected");
	}

	public override void OnConnectedToMaster()
	{
		DebugUI.Log("Connected to Master");
		DebugUI.Log("Server address is " + PhotonNetwork.ServerAddress);
		DebugUI.Log("Server is " + PhotonNetwork.Server);
		DebugUI.Log("Region is " + PhotonNetwork.CloudRegion);
		PhotonNetwork.JoinOrCreateRoom("Room 1", new RoomOptions() { MaxPlayers = 4 }, null);
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		DebugUI.Log("Disconnected");
	}

	public override void OnCreatedRoom()
	{
		DebugUI.Log("Room Created");
	}

	public override void OnJoinedRoom()
	{
		DebugUI.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);
		if (!PlayerManger.Instance.m_localPlayerInstance)
		{
			PhotonNetwork.Instantiate(m_playerPrefab.name, new Vector3(0, 2, 0), Quaternion.identity);
			if (PhotonNetwork.IsMasterClient)
			{
				PhotonNetwork.Instantiate(m_aiPrefab.name, new Vector3(5, 2, 0), Quaternion.identity);
			}
		}
	}

	public override void OnLeftRoom()
	{
		DebugUI.Log("Left Room");
	}

	public override void OnJoinedLobby()
	{
		DebugUI.Log("Joined Lobby");
	}

	public override void OnLeftLobby()
	{
		DebugUI.Log("Left Lobby");
	}
}
