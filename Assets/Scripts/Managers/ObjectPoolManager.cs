﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public enum PoolTypes
    {
        Bullet1,
        Bullet2,
        TurretBullet
    }

    private static ObjectPoolManager instance;
    public static ObjectPoolManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private ObjectPool[] objectPools;


    private void Awake()
    {
        instance = this;
    }

    public GameObject GetPooledObject(PoolTypes type)
    {
        return objectPools[(int)type].GetPooledObject();
    }

    public void ResetPool(PoolTypes type)
    {
        objectPools[(int)type].resetPool();
    }
}
