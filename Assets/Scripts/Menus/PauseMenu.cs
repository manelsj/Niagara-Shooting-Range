using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private AudioMixerSnapshot pausedSnapshot;
    [SerializeField]
    private AudioMixerSnapshot unpausedSnapshot;
    [SerializeField]
    private AudioMixer mainMixer;

    private bool paused = false;

    private bool canPause = false;
    public bool CanPause
    {
        get { return canPause; }
        set { canPause = value; }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && canPause)
        {
            Pause(!paused);
        }
    }

    private void Pause (bool pause)
    {
        paused = pause;
        pauseMenu.SetActive(pause);
        if (paused)
        {
            pausedSnapshot.TransitionTo(1f);
            Time.timeScale = 0;
        }
        else
        {
            unpausedSnapshot.TransitionTo(1f);
            Time.timeScale = 1;
        }
    }

    
}
