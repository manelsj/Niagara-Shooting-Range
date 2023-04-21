using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button continueButton;
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button optionsButton;
    [SerializeField]
    private Button quitButton;
    [SerializeField]
    private Button optionsQuitButton;
    [SerializeField]
    private Text masterVolume;
    [SerializeField]
    private Text musicVolume;
    [SerializeField]
    private Text effectsVolume;
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider effectsSlider;
    [SerializeField]
    private AudioMixer masterMixer;
    [SerializeField]
    private AudioMixer musicMixer;
    [SerializeField]
    private AudioMixer effectsMixer;

    public void OnEnable()
    {
        continueButton.interactable = GameManager.Instance.SaveGamePresent;

        GameManager.Instance.LoadVolumes();
        masterSlider.value = GameManager.Instance.GetMasterVolume;
        musicSlider.value = GameManager.Instance.GetMusicVolume;
        effectsSlider.value = GameManager.Instance.GetEffectsVolume;
        
    }

    public void NewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Options()
    {
        continueButton.gameObject.SetActive(false);
        newGameButton.gameObject.SetActive(false);
        optionsButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        optionsQuitButton.gameObject.SetActive(true);
        masterVolume.gameObject.SetActive(true);
        musicVolume.gameObject.SetActive(true);
        effectsVolume.gameObject.SetActive(true);
        masterSlider.gameObject.SetActive(true);
        musicSlider.gameObject.SetActive(true);
        effectsSlider.gameObject.SetActive(true);
    }

    public void QuitOptions()
    {
        continueButton.gameObject.SetActive(true);
        newGameButton.gameObject.SetActive(true);
        optionsButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        optionsQuitButton.gameObject.SetActive(false);
        masterVolume.gameObject.SetActive(false);
        musicVolume.gameObject.SetActive(false);
        effectsVolume.gameObject.SetActive(false);
        masterSlider.gameObject.SetActive(false);
        musicSlider.gameObject.SetActive(false);
        effectsSlider.gameObject.SetActive(false);

        GameManager.Instance.SaveVolume(masterSlider.value, musicSlider.value, effectsSlider.value);
    }

    public void EffectMaster(float slider)
    {
        masterMixer.SetFloat("Master", slider);
    }
    public void EffectMuisc(float slider)
    {
        masterMixer.SetFloat("Music", slider);
    }

    public void EffectEffects(float slider)
    {
        masterMixer.SetFloat("Effects", slider);
    }
}
