using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1Script : MonoBehaviour
{
    float speed = 15f;//bullet speed.
    private Rigidbody rb;//bullet body.
    private Vector3 direction;//bullet direction.

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (speed * direction * Time.fixedDeltaTime));//bullet moves.
    }

    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    //This method destroys the bullet when it collides with a wall or target.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.other.tag == "Target") || (collision.other.tag == "Wall") || (collision.other.tag == "Player"))
        {
            gameObject.SetActive(false);
        }
    }
}
