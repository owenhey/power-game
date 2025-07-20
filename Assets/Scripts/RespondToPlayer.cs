using System;
using UnityEngine;

public class RespondToPlayer : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;

    public Camera Player1Cam;
    public GameObject UIBar;
    
    private void Awake()
    {
        if (GameSettings.PlayerCount == 0 || GameSettings.PlayerCount == 2)
        {
            SetupTwoPlayer();
        }
        else
        {
            SetupOnePlayer();
        }
    }

    private void SetupOnePlayer()
    {
        Player2.SetActive(false);
        var rect = Player1Cam.rect;
        rect.position = new Vector2(0, 0);
        rect.width = .75f;
        rect.height = 1;
        Player1Cam.rect = rect;
        
        UIBar.gameObject.SetActive(false);
    }

    private void SetupTwoPlayer()
    {
        
    }
}
