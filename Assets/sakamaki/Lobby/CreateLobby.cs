using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CreateLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] GameObject m_loadImage = null;
    /// <summary>
    /// Photon�ɐڑ�����
    /// </summary>
    public void Connect()
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = "1.0";    // �����o�[�W�������w�肵�����̓��m���ڑ��ł���
            PhotonNetwork.ConnectUsingSettings();
            if (m_loadImage)
            {
                m_loadImage.SetActive(true);
            }
        }
    }

    /// <summary>
    /// MasterSaever�ɐڑ�
    /// </summary>
    public override void OnConnectedToMaster()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("MasterSaever�ɐڑ�����");
            PhotonNetwork.JoinRandomRoom();
        }
    }

    /// <summary>
    /// �����̕������Ȃ������ꍇ�ɁA�������쐬����
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("�����̕������Ȃ������ꍇ�ɁA�������쐬����");
        CreateRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("MasterGameScene");
        }
    }

    /// <summary>
    /// �������쐬
    /// </summary>
    public void CreateRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;

            PhotonNetwork.CreateRoom(null, roomOptions); // ���[������ null ���w�肷��ƃ����_���ȃ��[������t����
        }
    }

    /// <summary>�������쐬�������ɌĂ΂��</summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("�������쐬");
    }
}
