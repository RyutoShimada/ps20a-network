using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShowName : MonoBehaviour
{
    [SerializeField] Text _nameText = null;
    [SerializeField] InputField _inputField = null;
    [SerializeField] Transform _playertransform;

    Vector3 _namePostion;

    void Start()
    {
        _inputField = _inputField.GetComponent<InputField>();
        _nameText = _inputField.GetComponent<Text>();
        _namePostion = _nameText.transform.position;

    }
    void Update()
    {
        _namePostion = new Vector3(_playertransform.position.x, _playertransform.position.y + 0.5f, _playertransform.position.z);
    }
}
