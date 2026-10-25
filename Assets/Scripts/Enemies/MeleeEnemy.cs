using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range; //Range of the Gizmo attack box
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance; //Variable to make sure the Gizmo boxCollider does not go too far away
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity; //Mathf.Infinity, sodass der Feind sofort angreifen kann

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;
    
    //References
    private Animator anim;
    private Health playerHealth;

    private EnemyPatrol enemyPatrol;

    // Awake is called when the script instance loads
    void Awake()
    {
        anim = GetComponent<Animator>(); //Grab reference for Animator from object and store it in the variable (cf.: anim)
        enemyPatrol = GetComponentInParent<EnemyPatrol>(); //Grab reference for EnemyPatrol from object and store it in the variable (cf.: enemyPatrol)
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight
        if(PlayerInSight())
        {
            //Attack if cooldowntimer >= attackCooldown AND if playerHealth > 0
            if(cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0; //Reset to 0 after enemy just attacked
                anim.SetTrigger("meleeAttack");
                SoundManager.instance.PlaySound(attackSound);
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            //Use center of boxCollider for origin
            //transform.localScale.x to change direction of Gizmo box
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            //Size of the boxCollider
            //Vector3 holds the size of the boxCollider that we had before but we will multiply it with the range-variable
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            //Angle (0 = straight and not tilted), direction, distance, LayerMask
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null) //If Player is in sight (NOT null) then hit
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }

    //Visualize the sight of the enemy in Unity
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red; //Color of the Gizmo box
        //Draw the box
        //transform.localScale.x to change direction of Gizmo box
        //Vector3 holds the size of the boxCollider that we had before but we will multiply it with the range-variable
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void DamagePlayer()
    {
        //If Player still in range then damage the Player by the damage amount
        if(PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
