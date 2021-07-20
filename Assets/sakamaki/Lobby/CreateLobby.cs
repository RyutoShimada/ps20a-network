using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class CreateLobby : MonoBehaviourPunCallbacks
{
    /// <summary>�X�^�[�g�{�^��</summary>
    [SerializeField] GameObject m_startButton;
    /// <summary>�����ݒ�̑I�����j���[</summary>
    [SerializeField] GameObject m_choseButtons;
    /// <summary>�����쐬���j���[</summary>
    [SerializeField] GameObject m_createMenu;
    /// <summary>�����Q�����j���[</summary>
    [SerializeField] GameObject m_joinMenu;

    private State m_state = State.Title;

    public enum State
    {
        Title,
        Chose,
        CreateMenu,
        JoinMenu
    }

    private void Start()
    {
        Connect("1,0");
    }

    public void TransitionState(State state)
    {
        m_state = state;
        switch (m_state)
        {
            case State.Title:
                break;
            case State.Chose:
                m_startButton.SetActive(false);
                m_choseButtons.SetActive(true);
                break;
            case State.CreateMenu:
                m_choseButtons.SetActive(false);
                m_createMenu.SetActive(true);
                break;
            case State.JoinMenu:
                m_choseButtons.SetActive(false);
                m_joinMenu.SetActive(true);
                break;
            default:
                break;
        }
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
    /// �������쐬
    /// </summary>
    /// <param name="roomName">������</param>
    /// <param name="password">�p�X���[�h</param>
    public void CreateRoom(string roomName, string password)
    {
        if (PhotonNetwork.IsConnected)
        {
            string str;
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            if (password != null)
            {
                str = $"{roomName}_{password}";
                roomOptions.IsVisible = false;
                Debug.Log("�p�X���[�h��ݒ肵���������쐬");
            }
            else
            {
                str = roomName;
                roomOptions.IsVisible = true;
                Debug.Log("���J���ꂽ�������쐬");
            }
            PhotonNetwork.CreateRoom(roomName, roomOptions); // ���[������ null ���w�肷��ƃ����_���ȃ��[������t����
        }
    }

    /// <summary>�������쐬�������ɌĂ΂��</summary>
    public override void OnCreatedRoom()
    {
        Debug.Log("�������쐬");
    }
}
