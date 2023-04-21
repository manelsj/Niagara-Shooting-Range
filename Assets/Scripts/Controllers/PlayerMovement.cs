using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet1;
    [SerializeField]
    private GameObject bullet2;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private PlayerSounds sounds;
    [SerializeField]
    private ParticleSystem gunBurst;

    //Three possible locations for bullets to spawn out of.
    private GameObject fireLocationLeft;
    private GameObject fireLocationMid;
    private GameObject fireLocationRight;

    private GameObject gun;
    
    
    private Vector3 movementDir;
    private int fireButton;

    private float nextFire = 0f;
    private float fireRate = 0.5f;

    private bool isHit = false;//boolean value representing whether or not the player is hit.

    public void SetMovementDir(Vector3 dir)
    {
        movementDir = dir;//gains movementDir from PlayerController.
    }

    public void SetFireButton(int fire)
    {
        fireButton = fire;//gains fireButton from PlayerController.
    }

    private void Awake()
    {
        gun = GameObject.FindWithTag("gun");
        fireLocationLeft = GameObject.FindWithTag("fireLocationLeft");
        fireLocationMid = GameObject.FindWithTag("fireLocationMid");
        fireLocationRight = GameObject.FindWithTag("fireLocationRight");
    }

    /*Update called once every frame. Checks the value of fireButton as well if the gun is ready to fire again. 
    If gun is ready, a shot is fired by calling one of the fire methods.*/
    private void Update()
    {
        if ((fireButton == 1) && (nextFire < Time.time))
        {
            nextFire = Time.time + fireRate;
            Fire1();
        }
        else if ((fireButton == 2) && (nextFire < Time.time))
        {
            nextFire = Time.time + fireRate;
            Fire2();
        }
    }

    //Controls player movment.
    private void FixedUpdate()
    {

        transform.Translate(movementDir * speed * Time.deltaTime);//changes player position based off of direction

    }

    //This method creates a bullet1 prefab from the center fire location. The bullet1 script is enabled as well.
    public void Fire1()
    {
        if (bullet1)
        {
            Vector3 directionMid = fireLocationMid.transform.position - gun.transform.position;
            directionMid.y = 0;
            directionMid.Normalize();

            GameObject bulletGO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Bullet1);
            bulletGO.transform.position = fireLocationMid.transform.position;
            bulletGO.SetActive(true);
            bulletGO.GetComponent<Bullet1Script>().Fire(directionMid);
            sounds.Fire1();
            gunBurst.Play();
        }
    }

    //This method creates three bullet2 prefabs, one on each fire location. The bullet2 script is enabled on each.
    public void Fire2()
    {
        if (bullet2)
        {
            Vector3 directionLeft = fireLocationLeft.transform.position - gun.transform.position;
            directionLeft.y = 0;
            directionLeft.Normalize();

            Vector3 directionMid = fireLocationMid.transform.position - gun.transform.position;
            directionMid.y = 0;
            directionMid.Normalize();

            Vector3 directionRight = fireLocationRight.transform.position - gun.transform.position;
            directionRight.y = 0;
            directionRight.Normalize();

            GameObject bullet1GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Bullet2);
            bullet1GO.transform.position = fireLocationLeft.transform.position;
            bullet1GO.SetActive(true);
            bullet1GO.GetComponent<Bullet2Script>().resetLifeTime();

            GameObject bullet2GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Bullet2);
            bullet2GO.transform.position = fireLocationMid.transform.position;
            bullet2GO.SetActive(true);
            bullet2GO.GetComponent<Bullet2Script>().resetLifeTime();

            GameObject bullet3GO = ObjectPoolManager.Instance.GetPooledObject(ObjectPoolManager.PoolTypes.Bullet2);
            bullet3GO.transform.position = fireLocationRight.transform.position;
            bullet3GO.SetActive(true);
            bullet3GO.GetComponent<Bullet2Script>().resetLifeTime();



            bullet1GO.GetComponent<Bullet2Script>().Fire(directionLeft);
            bullet2GO.GetComponent<Bullet2Script>().Fire(directionMid);
            bullet3GO.GetComponent<Bullet2Script>().Fire(directionRight);

            sounds.Fire2();
            gunBurst.Play();
        }
    }

    //This method, called from the PlayerController, updates the rotation of the player based on the vector aimDir.
    public void UpdateAim(Vector3 aimDir)
    {
        float speed = 10*Time.deltaTime;
        Vector3 newDir = aimDir - transform.position;
        
        transform.rotation = Quaternion.LookRotation(aimDir);

    }

    //This method detects if a bullet has collided with the player.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.other.tag == "bullet")
        {
            isHit = true;
        }
    }

    //This method sets the player hit to false. Called from player controller after subtracting one to health.
    public void SetHitFalse()
    {
        isHit = false;
    }

    //This method returns the boolean value of isHit. Called from player controller.
    public bool GetHit()
    {
        return isHit;
    }

    //This method destroys the player object. Called from player controller if health reaches zero.
    

    public void resetBullets()
    {
        ObjectPoolManager.Instance.ResetPool(ObjectPoolManager.PoolTypes.Bullet1);
        ObjectPoolManager.Instance.ResetPool(ObjectPoolManager.PoolTypes.Bullet2);
    }
}
