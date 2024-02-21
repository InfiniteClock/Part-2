using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public static Player selectedPlayer { get; private set; }

    public static void SelectPlayer(Player selection)
    {
        if (selectedPlayer != null) selectedPlayer.Selected(false);
        selectedPlayer = selection;
        selection.Selected(true);
    }
}
