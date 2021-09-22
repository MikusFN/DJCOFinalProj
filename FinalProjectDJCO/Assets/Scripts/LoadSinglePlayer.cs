using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSinglePlayer : MonoBehaviour
{
    [SerializeField]
    private SelectColor _color;

    [SerializeField]
    private BackgroundMusic _musicScript;

    public void OnClick_loadLevel()
    {
        StaticClass.players = 1;
        if (_color.color == "blue")
        {
            StaticClass.player1 = "blue";
        }

        else if (_color.color == "purple")
        {
            StaticClass.player1 = "purple";
        }

        else if (_color.color == "green")
        {
            StaticClass.player1 = "green";
        }
        else
        {
            StaticClass.player1 = "red";
        }
        _musicScript.ChangeMusic();
        SceneManager.LoadScene("Level" + StaticClass.level);
    }
}
