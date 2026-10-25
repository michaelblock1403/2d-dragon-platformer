using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firetrap : MonoBehaviour
{
    [SerializeField] private float damage;
    
    [Header("Firetrap Timers")]
    [SerializeField] private float activationDelay; //Menge an Zeit, die die Falle zum Aktivieren braucht, nachdem der Spieler auf die Falle getreten ist
    [SerializeField] private float activeTime; //Wie lange die Falle aktiv bleibt, nachdem sie aktiviert worden ist
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip firetrapSound;

    private bool triggered; //When trap gets triggered
    private bool active; //When the trap is active and can hurt the player

    private Health playerHealth;

    // Awake is called when the script instance loads
    void Awake()
    {
        anim = GetComponent<Animator>(); //Grab reference for Animator from object and store it in the variable (cf.: anim)
        spriteRend = GetComponent<SpriteRenderer>(); //Grab reference for SpriteRenderer from object and store it in the variable (cf.: spriteRend)
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth != null && active)
            playerHealth.TakeDamage(damage); //Damage Player if health is not null and trap is active
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if(!triggered)
                StartCoroutine(ActivateFiretrap()); //Zugriff auf die IEnumerator Methode

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage); //Player um den damage-Wert verletzen,
                                                                     //falls der Spieler das Objekt berührt nachdem das Objekt aktiviert worden ist
        }
    }

    //Der Player sollte nicht verletzt werden, wenn er/sie den Bereich der Falle verlässt
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerHealth = null;
    }

    private IEnumerator ActivateFiretrap()
    {
        //Trigger the trap and turn the sprite red to notify the player
        triggered = true; //Trap has been triggered
        spriteRend.color = Color.red; //Turn the sprite red to notify the player

        //Wait for delay, play firetrapSound, return color back to normal, activate trap, turn on animation
        yield return new WaitForSeconds(activationDelay); //Warten bis die Dauer der activasionDelay-Variable abgelaufen ist
        SoundManager.instance.PlaySound(firetrapSound);
        spriteRend.color = Color.white; //turn the sprite back to its initial color
        active = true; //Falle wird aktiviert
        anim.SetBool("activated", true);

        //Wait until X seconds, deactivate trap and reset all variables and animator
        yield return new WaitForSeconds(activeTime); //Warten bis die Dauer der activeTime-Variable abgelaufen ist
        active = false; //Falle wird deaktiviert
        triggered = false;
        anim.SetBool("activated", false);
    }
}
