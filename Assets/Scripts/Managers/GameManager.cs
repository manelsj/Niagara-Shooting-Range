using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private PauseMenu pause;
    [SerializeField]
    private Loading load;
    [SerializeField]
    private GameOver gameOver;
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private string mainMenu;
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject playerGO;
    public GameObject PlayerGO
    {
        get { return playerGO; }
    }
    
    [SerializeField]
    private PlayerContoller player;

    [SerializeField]
    private string[] levelName;

    private bool isLoading = false;
    private bool saveGamePresent = false;
    private string saveFilePath;
    private string saveSettingsPath;
    public bool SaveGamePresent
    {
        get { return saveGamePresent; }
    }

    private int currentLevel = 0;
    private string currentLevelName;

    float master = 0;
    float music = 0;
    float effects = 0;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }


        saveFilePath = Application.persistentDataPath + "/NiagaraShootingRange.dat";
        saveSettingsPath = Application.persistentDataPath + "/NiagaraShootingRangeSettings.dat";
    }
    // Start is called before the first frame update
    void Start()
    {
        instance.Load();

        ReturnToMainMenu();
    }

    private IEnumerator LoadLevel(string levelName)
    {
        isLoading = true;

        playerGO.transform.position = new Vector3(0, 20, 0);
        playerGO.SetActive(false);
        camera.gameObject.SetActive(true);
        load.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        if (!string.IsNullOrEmpty(currentLevelName))
        {
            yield return SoundManager.Instance.StartCoroutine("UnLoadLevel");
            if (SceneManager.GetActiveScene() == null)
            {
                Debug.Log("No Level");
            }
            AsyncOperation aSyncUnload = SceneManager.UnloadSceneAsync(currentLevelName);

            while (!aSyncUnload.isDone)
            {
                yield return null;
            }
        }

        AsyncOperation aSyncLoad = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        while (!aSyncLoad.isDone)
        {
            yield return null;
            load.UpdateSlider(aSyncLoad.progress);
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelName));
        SoundManager.LevelLoadComplete();

        if (LevelManager.Instance && (LevelManager.Instance.getSpawnPoint() != null))
        {
            PlayerRespawn();
        }

        currentLevelName = levelName;

        load.gameObject.SetActive(false);
        camera.gameObject.SetActive(false);
        isLoading = false;
        
    }

    public void LevelComplete()
    {
        currentLevel++;
        
        if (currentLevel < levelName.Length)
        {
            StartCoroutine(LoadLevel(levelName[currentLevel]));
        }

        else
        {
            pause.CanPause = false;
            victory.SetActive(true);
            currentLevel--;
        }

        instance.Save();
    }

    public void PlayerRespawn()
    {
        Time.timeScale = 1;
        gameOver.gameObject.SetActive(false);
        playerGO.transform.position = LevelManager.Instance.getSpawnPoint().position;
        playerGO.transform.position = LevelManager.Instance.getSpawnPoint().position;
        player.Reset();
        playerGO.SetActive(true);
        LevelManager.Instance.Reset();
        pause.CanPause = true;
    }

    public void StartNewGame()
    {

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
        }

        currentLevel = 0;
        victory.SetActive(false);
        StartCoroutine("LoadLevel", levelName[0]);
    }

    public void ContinueGame()
    {
        StartCoroutine("LoadLevel", levelName[currentLevel]);
    }

    public void GameOver()
    {
        pause.CanPause = false;
        gameOver.gameObject.SetActive(true);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(saveFilePath);

        SaveGame data = new SaveGame();
        data.CurrentLevel = currentLevel;

        bf.Serialize(file, data);
        file.Close();
    }

    public void ReturnToMainMenu()
    {
        pause.CanPause = false;
        StartCoroutine("LoadLevel", mainMenu);
        gameOver.gameObject.SetActive(false);

    }

    public void Load()
    {

        Debug.Log(Application.persistentDataPath);

        if (File.Exists(saveFilePath))
        {
            saveGamePresent = true;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(saveFilePath, FileMode.Open);
            SaveGame data = (SaveGame)bf.Deserialize(file);
            file.Close();

            currentLevel = data.CurrentLevel;
        }

    }

    public void SaveVolume(float master, float music, float effects)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream settings = File.Create(saveSettingsPath);

        SaveSettings data = new SaveSettings();
        data.MasterVolume = master;
        data.MusicVolume = music;
        data.EffectsVolume = effects;

        bf.Serialize(settings, data);
        settings.Close();
    }

    public void LoadVolumes()
    {

        if (File.Exists(saveSettingsPath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream settings = File.Open(saveSettingsPath, FileMode.Open);
            SaveSettings data = (SaveSettings)bf.Deserialize(settings);
            master = data.MasterVolume;
            music = data.MusicVolume;
            effects = data.EffectsVolume;
            settings.Close();
        }

    }

    public float GetMasterVolume
    {
        get { return master; }
    }

    public float GetMusicVolume
    {
        get { return music; }
    }

    public float GetEffectsVolume
    {
        get { return effects; }
    }
}

[Serializable]
class SaveGame
{
    public int CurrentLevel { get; set; }

}

[Serializable]
class SaveSettings
{
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float EffectsVolume { get; set; }
}