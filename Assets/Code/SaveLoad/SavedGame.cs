using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedGame
{
    public static SavedGame current;
    public Player player;

    public SavedGame()
    {
        player = new Player();

    }

}
