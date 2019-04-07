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
		connectAndJoinRandom.ConnectNow();
	}

	public override void OnConnected()
	{
		DebugUI.Instance.Log("Version number is " + m_gameVersion);
		DebugUI.Instance.Log("Connected");
		//DebugUI.Instance.Log("Server address is " + PhotonNetwork.ServerAddress);
		//DebugUI.Instance.Log("Server is " + PhotonNetwork.Server);
		//DebugUI.Instance.Log("Region is " + PhotonNetwork.CloudRegion);
	}

	public override void OnConnectedToMaster()
	{
		DebugUI.Instance.Log("Connected to Master");
		DebugUI.Instance.Log("Server address is " + PhotonNetwork.ServerAddress);
		DebugUI.Instance.Log("Server is " + PhotonNetwork.Server);
		DebugUI.Instance.Log("Region is " + PhotonNetwork.CloudRegion);
		//PhotonNetwork.JoinRandomRoom();
	}

	public override void OnDisconnected(DisconnectCause cause)
	{
		DebugUI.Instance.Log("Disconnected");
	}

	public override void OnCreatedRoom()
	{
		DebugUI.Instance.Log("Room Created");
	}

	public override void OnJoinedRoom()
	{
		DebugUI.Instance.Log("Joined Room " + PhotonNetwork.CurrentRoom.Name);
		if (!PlayerManger.Instance.m_localPlayerInstance)
			PhotonNetwork.Instantiate(m_playerPrefab.name, new Vector3(0, 1, 0), Quaternion.identity);
	}

	public override void OnJoinRandomFailed(short returnCode, string message)
	{
		DebugUI.Instance.Log("No room present : " + returnCode + " : " + message);
		//PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }, null);
	}

	public override void OnLeftRoom()
	{
		DebugUI.Instance.Log("Left Room");
	}

	public override void OnJoinedLobby()
	{
		DebugUI.Instance.Log("Joined Lobby");
		//PhotonNetwork.JoinRandomRoom();
	}

	public override void OnLeftLobby()
	{
		DebugUI.Instance.Log("Left Lobby");
	}
}
