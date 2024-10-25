using System.Collections;
using UnityEngine;

public class ResultGameView : MonoBehaviour
{
    [SerializeField] private GameObject _winGameObject;
    [SerializeField] private GameObject _loseGameObject;

    public void SetValue(bool isWin)
    {
        _winGameObject.SetActive(isWin);
        _loseGameObject.SetActive(!isWin);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}