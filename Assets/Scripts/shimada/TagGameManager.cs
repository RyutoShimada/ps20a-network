﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
// Photon 用の名前空間を参照する
using Photon.Pun;   // PhotonNetwork を使うため
using Photon.Realtime;  // RaiseEventOptions/ReceiverGroup を使うため
using ExitGames.Client.Photon;  // SendOptions を使うため
using System.Collections.Generic;

/// <summary>
/// 鬼ごっこゲームのゲーム状態を制御するコンポーネント
/// 現状では部屋がいっぱいにならないとゲームが始まらない
/// </summary>
public class TagGameManager : MonoBehaviour
{
    public static TagGameManager instance { get; private set; }
    /// <summary>ロビーのシーン名</summary>
    [SerializeField] string m_LobbySceneName = "Test";
    /// <summary>画面に文字を表示するための Text</summary>
    [SerializeField] Text m_console = null;
    /// <summary>鬼のプレイヤー名</summary>
    [SerializeField] Text m_loser;
    /// <summary>鬼以外ののプレイヤー名</summary>
    [SerializeField] Text m_winner;
    /// <summary>鬼以外のプレイヤーリスト</summary>
    List<string> m_winners;
    /// <summary>ResultTextの間隔</summary>
    [SerializeField] float m_resultTextSpace = 1.5f;
    /// <summary>ゲームが始まっているかを管理するフラグ</summary>
    bool m_isGameStarted = false;
    [SerializeField] int m_maxPlayerCount = 4;
    [SerializeField] int m_startPlayerCount = 1;
    [SerializeField] Button m_startButton = null;
    PhotonView[] m_view = null;
    PhotonView m_timerView;
    PlayerController2D[] m_players;
    bool IsFirstTime = true;

    public Event m_eventState;

    public enum Event
    {
        GameStart,
        GameOver
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!m_console || !m_startButton) return;
        m_timerView = GetComponent<PhotonView>();
        m_console.text = "Wait for other players...";
        m_startButton.gameObject.SetActive(false);
        m_winners = new List<string>();
        m_eventState = new Event();
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom || !m_console || !m_startButton) return;

        if (!m_isGameStarted)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            m_console.text = $"{playerCount} / {m_maxPlayerCount}"; //部屋の最大人数と現在の人数を表示

            // 入室していて、まだゲームが始まっていない状態で、人数が揃った時
            if (playerCount >= m_startPlayerCount && PhotonNetwork.IsMasterClient)
            {
                m_players = GameObject.FindObjectsOfType<PlayerController2D>();
                m_startButton.gameObject.SetActive(true);//STARTボタンを有効にする
                m_view = new PhotonView[PhotonNetwork.CurrentRoom.PlayerCount];
                //for (int i = 0; i < playerCount; i++)
                //{
                //    m_view[i] = m_players[i].GetComponent<PhotonView>();
                //}
            }
        }
        else
        {
            m_timerView.RPC("DisplayTime", RpcTarget.All);
        }
        m_players = GameObject.FindObjectsOfType<PlayerController2D>();
    }

    /// <summary>
    /// ボタンから呼ばれる
    /// </summary>
    public void PlayGame()
    {
        if (!m_startButton) return;
        if (m_startButton.gameObject.activeSelf)
        {
            m_startButton.gameObject.SetActive(false);//STARTボタンを無効にする
        }
        // ゲームを開始する
        m_eventState = Event.GameStart;
        Raise();
        m_console.text = "Game Start!";
        Invoke("ClearConsole", 1.5f);   // 1.5秒後に表示を消す
        m_isGameStarted = true; // ゲーム開始フラグを立てる

        // マスタークライアントにより、ランダムに鬼を決める
        if (PhotonNetwork.IsMasterClient)
        {
            //PlayerController2D[] players = GameObject.FindObjectsOfType<PlayerController2D>();
            PhotonView view = m_players[Random.Range(0, m_players.Length)].GetComponent<PhotonView>();
            view.RPC("Tag", RpcTarget.All);
        }
    }

    /// <summary>
    /// ボタンから呼ばれる
    /// </summary>
    public void ShowResult()
    {
        if (!m_console) return;
        // ゲームを開始する
        m_console.text = "Show Result";

        foreach (var text in m_players)
        {
            if (text.m_tagMark.activeSelf)
            {
                if (m_loser)
                {
                    m_loser.text = text.gameObject.GetComponentInParent<DisplayName>().MyName;
                }
            }
            else
            {
                m_winners.Add(text.gameObject.GetComponentInParent<DisplayName>().MyName);
            }
        }
        if (IsFirstTime)
        {
            foreach (var item in m_winners)
            {
                if (!m_winner) return;
                m_winner.text += item + "\n";
            }
        }
        IsFirstTime = false;
        
    }

    void ClearConsole()
    {
        if (!m_console) return;
        m_console.text = "";
    }

    public void ReturnLobby()
    {
        //PhotonNetwork.Disconnect();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(m_LobbySceneName);

    }

    public void ReturnSameLobby()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    /// <summary>
    /// イベントを起こす
    /// </summary>
    public void Raise()
    {
        //イベントとして送るものを作る
        //byte eventCode = 0; // イベントコード 0~199 まで指定できる。200 以上はシステムで使われているので使えない。
        //RaiseEventOptions raiseEventOptions = new RaiseEventOptions
        //{
        //    Receivers = ReceiverGroup.All,  // 全体に送る 他に MasterClient, Others が指定できる
        //};  // イベントの起こし方

        RaiseEventOptions op = new RaiseEventOptions();
        op.Receivers = ReceiverGroup.All;

        SendOptions sendOptions = new SendOptions(); // オプションだが、特に何も指定しない

        switch (m_eventState)
        {
            case Event.GameStart:
                // イベントを起こす
                PhotonNetwork.RaiseEvent((byte)m_eventState, "GameStart", op, sendOptions);
                break;
            case Event.GameOver:
                // イベントを起こす
                PhotonNetwork.RaiseEvent((byte)m_eventState, "GameOver", op, sendOptions);
                break;
            default:
                break;
        }
        
    }
}
