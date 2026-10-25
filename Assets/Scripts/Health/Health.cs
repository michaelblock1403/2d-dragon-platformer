using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private float startingHealth; //Health when the game is started
    public float currentHealth { get; private set; } //Current health of the Player
                                                     //Kann NUR in dieser Klasse verändert werden
                                                     //ABER kann in anderen Klassen gelesen werden
    private Animator anim;
    private bool dead; //So die animation does not play twice

    [Header("iFrames")] //Invincibility/Invulnerability Frames
    [SerializeField] private float iFramesDuration; //Invulnerability duration
    [SerializeField] private float numberOfFlashes; //Wie oft der Player während der Dauer der Unsterblichkeit rot aufleuchten wird
    private SpriteRenderer spriteRend; //Ändern der Farbe des Players

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;
    [SerializeField] private HealthCollectible[] hearts;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    // Awake is called when the script instance loads
    void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>(); //Grab reference for Animator from object and store it in the variable (cf.: anim)
        spriteRend = GetComponent<SpriteRenderer>(); //Grab reference for SpriteRenderer from object and store it in the variable (cf.: spriteRend)
    }

    // Update is called once per frame
    void Update()
    {
        /*//Debug
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(1);*/
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return; //Wenn der Player unverwundbar ist, dann return und nicht die anderen Anweisungen von der Methode ausführen
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth); //Safeguard so that the health does not go beneath 0 or above the max value

        if(currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            //iframes
            StartCoroutine(Invulnerability());
            SoundManager.instance.PlaySound(hurtSound);
        }
        else
        {
            if(!dead) //Can only be entered if dead-variable is false
            {
                /*//Player
                if (GetComponent<PlayerMovement>() != null)
                    GetComponent<PlayerMovement>().enabled = false;

                //Enemy
                if (GetComponentInParent<EnemyPatrol>() != null)
                    GetComponentInParent<EnemyPatrol>().enabled = false;

                if (GetComponent<MeleeEnemy>() != null)
                    GetComponent<MeleeEnemy>().enabled = false;*/

                //Deactivate all attached components
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");

                dead = true; //From false to true so that the dead behaviour does not repeat

                SoundManager.instance.PlaySound(deathSound);
            }
        }
    }

    //Gleicher Code wie bei der TakeDamage-Methode aber jetzt mit plus anstatt minus
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }

    public void Respawn()
    {
        dead = false; //dead = false, da Player nicht mehr tot ist
        AddHealth(startingHealth); //Reset Player health
        anim.ResetTrigger("die"); //Get Player out of the die animation
        anim.Play("Idle");
        StartCoroutine(Invulnerability()); //Zugriff auf die IEnumerator Methode

        //Activate all attached components
        foreach (Behaviour component in components)
            component.enabled = true;

        //Activate all heart collectibles
        foreach (HealthCollectible component in hearts)
            component.gameObject.SetActive(true);
    }

    private IEnumerator Invulnerability() //Kann NICHT wie eine normale Methode aufgerufen werden!
                                          //-> Verwendung von StartCoroutine
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true); //Kollisionen mit allen feindlichen Gegenständen ignorieren
                                                      //Player = Layer 10; Feinde = Layer 11; Kollisionen werden ignoriert = true
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f); //Rote Farbe
            /*Hintergründe für die Anweisung: iFramesDuration dauert 2 Sekunden und wir müssen 3 mal aufleuchten -> 2 Sek. / 3 = 0.66 Sek.
             *Wir müssen aber die Farbe zu rot ändern und dann wieder zurück zu weiß -> 2 Sek. / 3 * 2 = 0.33 Sek.*/
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }

        //Ende der Invulnerability
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }
}
