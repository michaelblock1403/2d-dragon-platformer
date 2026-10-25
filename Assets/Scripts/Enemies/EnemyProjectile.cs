using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : EnemyDamage //Will damage the player every time they touch
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime; //Deaktivierung des Objekts nach einer bestimmten Zeit
    private float lifetime;
    public void SetDirection(float _direction)
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movementSpeed = speed * Time.deltaTime; //Frame-independent movement
        transform.Translate(movementSpeed, 0, 0); //Bewegung des Objekts auf der x-Achse

        lifetime += Time.deltaTime; //Inkrementieren der lifetime-Variable bei jedem Frame
        if (lifetime > resetTime) //Wenn lifetime größer ist als die resetTime-Variable, dann gameObject deaktivieren
            gameObject.SetActive(false);
    }

    public void ActivateProjectile()
    {
        lifetime = 0;
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); //Execute logic from parent script (EnemyDamage) first
        gameObject.SetActive(false); //When this hits any object it will deactivate
    }
}
