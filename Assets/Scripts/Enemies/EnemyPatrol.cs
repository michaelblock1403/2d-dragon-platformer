using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy; //To move the enemy around

    [Header("Movement parameters")]
    [SerializeField] private float speed;
    private Vector3 initScale; //Initial scale
    private bool movingLeft;

    [Header("Idle Behaviour")]
    [SerializeField] private float idleDuration;
    private float idleTimer;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    // Awake is called when the script instance loads
    void Awake()
    {
        initScale = enemy.localScale; //Save the enemies localScale at the beginning of the game
    }

    private void OnDisable()
    {
        anim.SetBool("movement", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x) //Check if the enemy has NOT reached the leftEdge yet
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x) //Check if the enemy has NOT reached the rightEdge yet
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    //Turn the enemy around
    private void DirectionChange()
    {
        anim.SetBool("movement", false);
        idleTimer += Time.deltaTime; //Inkrementieren der Zeit, die der Feind bisher stillgestanden ist

        if (idleTimer > idleDuration)
            movingLeft = !movingLeft; //Change movingLeft to false if it was true and vice versa
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0; //idleTimer auf 0 setzen, wenn sich der Feind wieder bewegt
        anim.SetBool("movement", true);

        //Make enemy face direction
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, //Mathf.Abs, da die initScale ja positiv, aber auch negativ sein kann
            initScale.y, initScale.z);

        //Move in that direction
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
            enemy.position.y, enemy.position.z);
    }
}
