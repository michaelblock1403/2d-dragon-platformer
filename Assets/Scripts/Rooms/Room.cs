using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject[] enemies; //Alle Feinde von einem Raum werden in diesem Array gespeichert
    private Vector3[] initialPosition; //Speichern der Anfangsposition von allen Feinden

    // Awake is called when the script instance loads
    void Awake()
    {
        //Save the initial positions of the enemies
        initialPosition = new Vector3[enemies.Length]; //initialPosition hat genau die gleiche Länge wie das enemies-Array
        for (int i = 0; i < enemies.Length; i++) //Iterieren über alle feindliche Objekte und Speichern der Position im initialPosition-Array
        {
            if(enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateRoom(bool _status)
    {
        //Activate/Deactivate enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i] != null)
            {
                enemies[i].SetActive(_status); //Activate/Deactivate enemies
                enemies[i].transform.position = initialPosition[i]; //Reset position to initalPosition
            }
        }
    }
}
