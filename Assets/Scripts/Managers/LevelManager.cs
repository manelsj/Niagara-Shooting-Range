using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    private Transform spawnPoint;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelManager>();
            }

            

            return instance;
        }
    }

    private List<TurretControllerAI> enemies;

    [SerializeField]
    private AudioClip levelMusic;
    public AudioClip LevelMusic
    {
        get { return instance.levelMusic; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        enemies = new List<TurretControllerAI>();

        GameObject spawnGO = GameObject.FindGameObjectWithTag("Respawn");
        if (spawnGO)
        {
            spawnPoint = spawnGO.transform;
        }
    }

    public void RegisterEnemy(TurretControllerAI turret)
    {
        enemies.Add(turret);
    }

    private bool check = true;

    private void Update()
    {
        if (check && checkAllEnemiesDead() == true && enemies.Count > 0)
        {
            StartCoroutine("CompleteLevel");
            check = false;
        }
    }

    private bool checkAllEnemiesDead()
    {
        bool retVal = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            retVal = retVal && enemies[i].getIsDead();
        }

        return retVal;
    }

    public Transform getSpawnPoint()
    {
        return spawnPoint;
    }

    public string getTurrets
    {
        get { return RemainingTurrets().ToString(); }
    }

    public void Reset()
    {
        StopCoroutine("CompleteLevel");
        check = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].Reset();
        }
    }

    private IEnumerator CompleteLevel()
    {
        yield return new WaitForSeconds(3f);

        GameManager.Instance.LevelComplete();
    }

    private int RemainingTurrets()
    {
        int turretNum = 0;

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].getIsDead() == false) turretNum++;
        }

        return turretNum;
    }
}
