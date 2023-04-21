using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet2Script : MonoBehaviour
{
    float speed = 10f;//bullet speed.
    float lifeTime = 0.5f;//bullet lifetime.
    private Rigidbody rb;//bullet body.
    private Vector3 direction;//bullet direction.

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    //Update is called once per frame. Here, the lifetime of the bullet will be checked and decreased if necessary. Otherwise, the bullet is destroyed.
    private void Update()
    {
        lifeTime -= 1*Time.deltaTime;
        if (lifeTime <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    // FixedUpdate is called once per frame. Here the bullet will move forward in its set direction.
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + (speed * direction * Time.fixedDeltaTime));
    }

    public void Fire(Vector3 dir)
    {
        direction = dir;
    }

    //This method destroys the bullet if it collides with a wall or a target.
    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.other.tag == "Target") || (collision.other.tag == "Wall") || (collision.other.tag == "Player"))
        {
            gameObject.SetActive(false);
        }
    }

    public void resetLifeTime()
    {
        lifeTime = 0.5f;
    }
}
