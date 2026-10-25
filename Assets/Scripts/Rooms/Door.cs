using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(collision.transform.position.x < transform.position.x) //Wenn Player den neuen Raum betreten hat,
                                                                      //dann Aktivieren des nächsten Raumes
                                                                      //und Deaktivieren des vorherigen Raumes
            {
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else //Wenn Player den vorherigen Raum wieder betreten hat,
                 //dann Aktivieren des vorherigen Raumes
                 //und Deaktivieren des nächsten Raumes
            {
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }
        }
    }
}
