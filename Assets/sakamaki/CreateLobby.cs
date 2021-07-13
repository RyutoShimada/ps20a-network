using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField m_inputRoomName;
   
    private string m_roomName = null;

    private void Start()
    {
        Connect("1,0");
    }

    public void  CreateMyRoom()
    {
        m_roomName = m_inputRoomName.text.ToString();
        CreateRandomRoom();
        SceneManager.LoadScene("Game_Sakamaki");
    }

    /// <summary>
    /// Photon�ɐڑ�����
    /// </summary>
    private void Connect(string gameVersion)
    {
        if (PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.GameVersion = gameVersion;    // �����o�[�W�������w�肵�����̓��m���ڑ��ł���
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    /// <summary>
    /// �w�肵�����O�̃��[��������ĎQ������
    /// </summary>
    private void CreateRandomRoom()
    {
        if (PhotonNetwork.IsConnected)
        {
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsVisible = false;   // �N�ł��Q���ł��Ȃ��悤�ɂ���
            /* **************************************************
             * spawPositions �̔z�񒷂��ő�v���C�l���Ƃ���B
             * �����łł͍ő�20�܂Ŏw��ł���B
             * MaxPlayers �̌^�� byte �Ȃ̂ŃL���X�g���Ă���B
             * MaxPlayers �̌^�� byte �ł��闝�R�͂����炭1���[���̃v���C�l����255�l�ɐ������������߂ł��傤�B
             * **************************************************/
            roomOptions.MaxPlayers = 4;
            PhotonNetwork.CreateRoom(m_roomName, roomOptions); // ���[������ null ���w�肷��ƃ����_���ȃ��[������t����
           // Debug.Log()
        }
    }

    /// <summary>�������쐬������</summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("�������쐬");
    }
}
