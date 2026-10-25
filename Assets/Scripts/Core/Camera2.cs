using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2 : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float aheadDistance; //Wie weit die Kamera nach vorne schaut
    [SerializeField] private float cameraSpeed; //Wie schnell sich die Kamera bewegt
    private float lookAhead;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(target.transform.position.x + lookAhead, transform.position.y, -10);
        lookAhead = Mathf.Lerp(lookAhead, (aheadDistance * target.localScale.x), //target.localScale.x = 1 (Blick nach rechts); ... = -1 (Blick nach links)
            Time.deltaTime * cameraSpeed); //Time.deltaTime = makes the game frame-independent, d. h.: das Spiel wird immer mit der gleichen Geschwindigkeit ausgeführt,
        //unabhängig von den FPS
    }
}
