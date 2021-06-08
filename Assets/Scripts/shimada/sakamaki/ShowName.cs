using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class ShowName : MonoBehaviour
{
    /// <summary>InputFieldから受け取ったTextを表示するText</summary>
    [SerializeField] private Text _nameText = null;
    /// <summary>プレイヤーの入力を受け付けるUI</summary>
    [SerializeField] private InputField _inputField = null;
    /// <summary>Player(操作する人)のGameObject</summary>
    [SerializeField] private PlayerController2D _player = null;
    /// <summary>割り振られた番号</summary>
    [SerializeField] private int _myActorNumber = 0;
  
    private NetworkGameManager _networkGM;

    void Start()
    {
        _inputField = _inputField.GetComponent<InputField>();
        _nameText = _inputField.GetComponent<Text>();
    }
    void Update()
    {
        if (_myActorNumber == 0)
        {
            _myActorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Debug.Log($"{_myActorNumber}");
        }
    }

    /// <summary>
    /// Textの入力が終わりEnterが押されたときに呼ばれる
    /// </summary>
    public void OnEndEdit()
    {
        Debug.Log("OnEndEditが呼ばれた");
        _nameText.text = _inputField.text;
    }
}
