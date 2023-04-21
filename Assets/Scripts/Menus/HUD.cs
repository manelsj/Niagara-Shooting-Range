using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private PlayerContoller player;
    [SerializeField]
    private Text playerHealth;
    [SerializeField]
    private Text remainingTurrets;


    private void Update()
    {
        if (LevelManager.Instance)
        {
            playerHealth.text = player.Health();
            remainingTurrets.text = LevelManager.Instance.getTurrets;
        }

    }
}
