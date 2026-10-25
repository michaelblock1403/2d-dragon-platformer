using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint; //Wo jeder Pfeil mit der Bewegung beginnt
    [SerializeField] private GameObject[] arrows; //Beinhaltet alle Projektile, die die ArrowTrap schießen kann
    private float cooldownTimer;

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime; //cooldownTimer erhöht sich bei jedem Frame

        if (cooldownTimer >= attackCooldown) //Wenn der cooldownTimer größer oder gleich dem attackCooldown ist,
                                             //dann wird die Attack-Methode ausgeführt
            Attack();
    }

    private void Attack()
    {
        cooldownTimer = 0; //Wenn die ArrowTrap schießt wird der Wert wieder auf 0 gesetzt

        SoundManager.instance.PlaySound(arrowSound);
        arrows[FindArrow()].transform.position = firePoint.position; //Jedes mal wenn geschossen wird muss die Position vom Projektil
                                                                     //auf die Firepoint Position zurückgesetzt werden
        arrows[FindArrow()].GetComponent<EnemyProjectile>().ActivateProjectile(); //Die Richtung vom Projektil setzen
    }

    private int FindArrow()
    {
        //Cycle through all the arrows and return the first one that is not active in the hierarchy
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
                return i;
        }
        return 0; //Wenn die Methode keine Pfeile findet, die inaktiv sind, dann wird einfach der erste Pfeil verwendet
    }
}
