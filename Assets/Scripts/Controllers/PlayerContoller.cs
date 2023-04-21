using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
    [SerializeField]
    private PlayerMovement player;//the player movement script that operates along with this one.
    //[SerializeField]
    //private PauseMenu pause;

    private int health = 3;//the player health

    // Update is called once per frame
    public void FixedUpdate()
    {
        //values obtained from player input.
        float dx = Input.GetAxis("Horizontal");
        float dz = Input.GetAxis("Vertical");
        bool leftTrigger = Input.GetMouseButton(0);
        bool rightTrigger = Input.GetMouseButton(1);
        int fire = 0;//initial valued of integer 'fire'

        Vector3 dir = new Vector3(dx, 0, dz);//returns direction based off key input.

        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }

        player.SetMovementDir(dir);//calls additional script to set player movement to the currently held direction on keypad.

        if (leftTrigger == true)//sets fire button depending on current mouse click. If no mouse click is detected, 'fire' remains 0.
        {
            fire = 1;
        }
        else if (rightTrigger == true)
        {
            fire = 2;
        }

        player.SetFireButton(fire);//calls additional script to set player current fire to whatever was clicked on the mouse.

        //Set of commands that detected mouse position in the game world and create a new vector3 direction based off of it.
        Plane plane = new Plane(Vector3.up, player.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distanceToPlane;
        plane.Raycast(ray, out distanceToPlane);
        Vector3 mouseWorldPos = ray.GetPoint(distanceToPlane);

        Vector3 aimDir = mouseWorldPos - player.transform.position;
        aimDir.y = 0;
        aimDir.Normalize();
        player.UpdateAim(aimDir);//calls additional script to set player aim direction to wherever the player is aiming.

        if (player.GetHit() == true)//if the playerbody returns a hit, subtract the player health by one.
        {
            health--;
            player.SetHitFalse();
            if (health == 0)//if the player health is zero, then call the player movement command Death().
            {
                Time.timeScale = 0;
                GameManager.Instance.GameOver();
            }
        }
    }

    public string Health()
    {
        return health.ToString();
    }

    public void Reset()
    {
        health = 3;
        player.resetBullets();
    }
}
