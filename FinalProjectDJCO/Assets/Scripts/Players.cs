using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    SortedList<string, GameObject> players;

    public bool HasPlayers { get => players.Count > 0; }
    public bool CanAddPlayer { get => players!= null; }
    public int PlayerCount { get => players.Count; }

    // Start is called before the first frame update
    void Awake()
    {
        players = new SortedList<string, GameObject>();
    }

    public void AddPlayer(string playerName, GameObject player)
    {
        players.Add(playerName, player);
    }
}
