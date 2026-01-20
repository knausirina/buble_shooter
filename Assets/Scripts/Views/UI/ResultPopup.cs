using System.Collections;
using UnityEngine;

public class ResultPopup : Popup
{
    [SerializeField] private GameObject _winGameObject;
    [SerializeField] private GameObject _loseGameObject;

    public bool IsWin { get; set; }

    public override void Show()
    {
        base.Show();
        _winGameObject.SetActive(IsWin);
        _loseGameObject.SetActive(!IsWin);
    }   
}