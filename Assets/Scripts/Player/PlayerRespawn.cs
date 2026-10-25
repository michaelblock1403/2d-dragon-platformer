using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpointSound; //Sound that we will play when picking up a new checkpoint
    private Transform currentCheckpoint; //We will store our last checkpoint here
    private Health playerHealth; //Reset Player health when he/she respawns

    public GameObject Player;
    public GameObject startPoint; //A replacement Object if a checkpoint has not been reached by the Player

    // Awake is called when the script instance loads
    void Awake()
    {
        playerHealth = GetComponent<Health>(); //Grab reference for Health from object and store it in the variable (cf.: playerHealth)
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Respawn()
    {
        if (currentCheckpoint != null)
        {
            transform.position = currentCheckpoint.position; //Move player to checkpoint position
            playerHealth.Respawn(); //Restore player health and reset animation
        }
        else //Wenn kein Checkpoint eingesammelt worden ist, dann soll der Player auf die Position vom startPoint-Objekt gesetzt werden
        {
            Player.transform.position = startPoint.transform.position;
        }

        //Move camera back to checkpoint room (for this to work the checkpoint object has to be placed as a child of the room object)
        //Camera.main.GetComponent<Camera1>().MoveToNewRoom(currentCheckpoint.parent);
    }

    //Activate checkpoints
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Checkpoint")
        {
            currentCheckpoint = collision.transform; //Store the checkpoint that we activated as the current one
            SoundManager.instance.PlaySound(checkpointSound);
            collision.GetComponent<Collider2D>().enabled = false; //Deactivate checkpoint collider
            collision.GetComponent<Animator>().SetTrigger("appear"); //Trigger checkpoint animation
        }
    }
}
