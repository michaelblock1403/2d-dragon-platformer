using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Sideways : MonoBehaviour
{
    [SerializeField] private float damage; //Damage which the Enemy_Sideways will cause
    [SerializeField] private float movementDistance; //Movement distance of the object
    [SerializeField] private float speed; //Movement speed of the object
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;


    // Awake is called when the script instance loads
    void Awake()
    {
        leftEdge = transform.position.x - movementDistance; //Position of the Player - the movement distance of the object
        rightEdge = transform.position.x + movementDistance; //Position of the Player + the movement distance of the object
    }

    // Update is called once per frame
    void Update()
    {
        if(movingLeft)
        {
            if (transform.position.x > leftEdge) //Check that the object has NOT reached the leftEdge yet
            {
                //Move the object to the left
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge) //Check that the object has NOT reached the rightEdge yet
            {
                //Move the object to the right
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage); //Damange Player by the amount of the damage-variable
        }
    }
}
