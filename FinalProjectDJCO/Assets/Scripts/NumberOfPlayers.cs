using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPlayers : MonoBehaviour
{
    public GameObject GameManager;
    public Button pressButton1;
    public Button pressButton2;
    [SerializeField]
    private LevelSelect _level;

    public void returnScreen() {

        if(GameManager.GetComponent<GameManagerScript>().button1) {
            pressButton1.Select();
        }
        if(GameManager.GetComponent<GameManagerScript>().button2) {
            pressButton2.Select();
        }
    }

    public void OnClick_SelectLevel()
    {
        StaticClass.level = _level.level;
    }
}

public static class StaticClass
{
    public static string player1 { get; set; }
    public static string player2 { get; set; }
    public static int players { get; set; }
    public static int level { get; set; }
}
