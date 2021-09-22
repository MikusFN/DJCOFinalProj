using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftArrow;
    [SerializeField]
    private GameObject _rightArrow;
    [SerializeField]
    private Text _text;

    public int level = 1;

    public void OnClick_PreviousLevel()
    {
        level = 1;
        _leftArrow.SetActive(false);
        _rightArrow.SetActive(true);
        _text.text = "Level 1";
    }

    public void OnClick_NextLevel()
    {
        level = 2;
        _leftArrow.SetActive(true);
        _rightArrow.SetActive(false);
        _text.text = "Level 2";
    }
}

