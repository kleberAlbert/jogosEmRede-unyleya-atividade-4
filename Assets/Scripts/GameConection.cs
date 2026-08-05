using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class GameConection : MonoBehaviourPunCallbacks
{
    public Text chatLog;
    public string playerPrefabName = "GamePlayer"; // Nome exato do Prefab dentro da pasta Assets/Resources/
  
    public Vector3 spawnPosition = new Vector3(292f, 192f, 31f);

    private string myNickName;

    private void Start()
    {
        // 1. Gera o apelido
        myNickName = "Koelho_" + Random.Range(1000, 9999);
        PhotonNetwork.NickName = myNickName;

        if (chatLog != null)
        {
            chatLog.text = myNickName + " - Conectando ao servidor...";
        }

        // 2. Conecta aos servidores do Photon
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.NickName = myNickName;

        if (chatLog != null) chatLog.text = "Conectado! Entrando no Lobby...";
        
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if (chatLog != null) chatLog.text = "Entrando na Sala 'Atividade 4'...";
        PhotonNetwork.JoinRoom("Atividade 4");    
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (chatLog != null) chatLog.text = "Sala não encontrada, criando sala...";
        PhotonNetwork.CreateRoom("Atividade 4", new RoomOptions { MaxPlayers = 10 });
    }

    public override void OnJoinedRoom()
    {
        string localNick = !string.IsNullOrEmpty(PhotonNetwork.NickName) ? PhotonNetwork.NickName : myNickName;

        if (chatLog != null)
        {
            chatLog.text = "Entrou na sala " + PhotonNetwork.CurrentRoom.Name + "! UserName: " + localNick;
        }

        Debug.Log("NickName confirmado: " + localNick);

        if (PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.Instantiate(playerPrefabName, spawnPosition, Quaternion.identity);
        }
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        Debug.LogError("Erro de conexão Photon: " + errorInfo.Info);
        if (chatLog != null)
            chatLog.text = "Erro: " + errorInfo.Info;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning("Desconectado do Photon. Motivo: " + cause);
        if (chatLog != null)
            chatLog.text = "Desconectado: " + cause.ToString();
    }
}