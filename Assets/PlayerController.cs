using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PhotonView myPhotonView;

    public TextMeshProUGUI campoTexto;
    public TextMeshProUGUI campoMensagem;
    private void Start()
    {
        myPhotonView = GetComponent<PhotonView>();
    }

    public void SendChatMessage(string message)
    {
        myPhotonView.RPC(nameof(ReceiveChat), RpcTarget.All, message);
    }

    [PunRPC] public void ReceiveChat(string message, PhotonMessageInfo info)
    {
        Debug.Log($"nova mensagem: {message}");
        PUCCPhoton.Instance.UpdateMessageContainer(message, info);
    }

}
