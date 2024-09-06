using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.VersionControl;

public class PUCCPhoton : MonoBehaviourPunCallbacks
{
    public static PUCCPhoton Instance;
    private PlayerController playerMyself;
    public TextMeshProUGUI lista;
    public TextMeshProUGUI campoTexto;
    public TextMeshProUGUI campoMensagem;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //atualizar a versao do photon pra separar os
        //jogadores em servidores diferentes
        PhotonNetwork.GameVersion = "0.1";
        PhotonNetwork.NickName = Environment.UserName;
        Debug.Log("[PUCCPhoton] Conectando ao servidor com nickname: " + PhotonNetwork.NickName);

        //conecta no servidor do Photon usando
        //o App Id que está na pasta 
        //Assets/Photon/PhotonUnityNetwork/Resources
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("[PUCCPhoton] Conectou no servidor!");

        //agora que estamos conectados no servidor do Photon
        //precisamos entrar em Lobby para receber a lista de
        //salas ou criar um sala propria
        Debug.Log("[PUCCPhoton] Entrando ao lobby...");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[PUCCPhoton] Entrou no lobby!");
        //agora que estamos em um lobby
        //podermos entrar numa sala ou criar uma se for necessario
        Debug.Log("[PUCCPhoton] Entrando ou Criando um sala...");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;
        PhotonNetwork.JoinOrCreateRoom("PUCCRoom", roomOptions, null);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        string log = "";
        foreach(RoomInfo room in roomList)
        {
            log += "ROOM: " + room.Name + "\n";
        }
        Debug.Log("[PUCCPhoton] RoomListUpdate: "+log);
    }

    public override void OnJoinedRoom()
    {
        //esse evento acontece quando EU entro na sala
        Debug.Log("[PUCCPhoton] Entrei na sala!");
        string nome = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            nome += $"{player.NickName}\n";
        }
        lista.text = nome;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //esse evento acontece quando outros players entram na sala.
        Debug.Log("[PUCCPhoton] Player: " + newPlayer.NickName + " entrou na sala...");
        playerMyself = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<PlayerController>();

        string nome = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            nome += $"{player.NickName}\n";
        }
        lista.text = nome;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //esse evento acontece quando outros players saem da na sala.
        Debug.Log("[PUCCPhoton] Player: " + otherPlayer.NickName + " saiu da sala!");

        string nome = "";
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            nome += $"{player.NickName}\n";
        }
        lista.text = nome;
    }

    public void OnButtonPress()
    {
        playerMyself.myPhotonView.RPC("ReceiveChat", RpcTarget.All, campoMensagem.text);
        campoMensagem.text = "";
    }

    public void UpdateMessageContainer(string newMessage, PhotonMessageInfo info)
    {
        campoTexto.text += newMessage + "\n";
    }
}
