using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed; //Horizontale Bewegungsgeschwindigkeit
    [SerializeField] private float jumpPower; //Vertikalte Bewegungsgeschwindigkeit

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps; //Variable für Doppelsprünge
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Horizontal wall jump force
    [SerializeField] private float wallJumpY; //Vertical wall jump force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; //Verwendung bei der isGrounded()-Methode
    [SerializeField] private LayerMask wallLayer; //Verwendung bei der onWall()-Methode

    [Header("Sounds")]
    [SerializeField] private AudioClip jumpSound;

    private Rigidbody2D body; //Physischer Körper, der durch äußere Kraefte (z. B.: Schwerkraft) beeinflusst werden kann
    private BoxCollider2D boxCollider; //Kollisionserkennung
    private Animator anim; //Player animations
    public Vector2 scale;
    private float horizontalInput; //Variable zum Speichern der horizontalen Bewegungen
    private float wallJumpCooldown;

    public bool isFacingRight; //Zum Überprüfen der Blickrichtung des Players (wurde zum Debuggen verwendet)

    //Unterschied von Awake und Start:
    //Awake:
    //Wird zuerst aufgerufen und sollte als Konstruktor verwendet werden.
    //Mit Awake() kann also ein Objekt konstruiert werden und eine Referenz auf ein anderes Skript erstellt werden.
    //Start:
    //Start wird verwendet, wenn die Reihenfolge der Initialisierung verzögert werden soll
    //und Informationen über das konstruierte Objekt an das andere Skript übergeben werden soll
    //und wenn man Dinge tun möchten, die nicht eng mit der Instanziierung verbunden sind.

    // Awake is called when the script instance loads
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>(); //Grab reference for Rigidbody from object and store it in the variable (cf.: body)
        boxCollider = GetComponent<BoxCollider2D>(); //Grab reference for BoxCollider from object and store it in the variable (cf.: boxCollider)
        anim = GetComponent<Animator>(); //Grab reference for Animator from object and store it in the variable (cf.: anim)

        isFacingRight = true; //Zum Überprüfen der Blickrichtung des Players
    }

    // Update is called once per frame
    private void Update()
    {
        //Change isFacingRight variable depending on left or right movement
        if(Input.GetAxisRaw("Horizontal") > 0.5 || Input.GetAxisRaw("Horizontal") < -0.5)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.5)
            {
                isFacingRight = true;
                //print(isFacingRight); //Wurde zum Debuggen verwendet
            }
            if (Input.GetAxisRaw("Horizontal") < -0.5)
            {
                isFacingRight = false;
                //print(isFacingRight); //Wurde zum Debuggen verwendet
            }
        }

        //Change Player sprite (facing direction) depending on isFacingRight variable
        if(isFacingRight) {
            transform.localScale = new Vector2(scale.x * 1, scale.y * 1);
        } else if(!isFacingRight)
        {
            transform.localScale = new Vector2(scale.x * -1, scale.y * 1);
        }

        //Variable für die horizontalen Bewegungen
        horizontalInput = Input.GetAxis("Horizontal");

        //Set Animator parameters
        anim.SetBool("run", horizontalInput != 0); //Player rennt nur, wenn es eine horizontale Bewegung gibt
        anim.SetBool("grounded", isGrounded()); //Player befindet sich auf dem Boden, wenn die isGrounded()-Methode true ergibt

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        //Adjustable jump height
        if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2); //Reduzierung der y-Geschwindigkeit, wenn die Space-Taste nicht komplett gedrückt wird

        //Wall jumps
        if (onWall())
        {
            //Player bleibt an der Wand kleben und kann nicht herunterfallen
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            //Player fällt hinunter, wenn sich der Player von der Wand wegbewegt
            body.gravityScale = 3;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                jumpCounter = extraJumps; //Reset jump couter to extra jump value
            }
        }
    }

    //Jump logic
    private void Jump()
    {
        if (!onWall() && jumpCounter <= 0) return; //If not on the wall
                                                   //and don't have any extra jumps
                                                   //then don't do anything

        SoundManager.instance.PlaySound(jumpSound);

        //WallJump, wenn sich der Player an einer Wand befindet. Normaler Sprung, falls dies nicht der Fall ist.
        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower); //Horizontale Bewegung bleibt gleich, aber vertikale Bewegung mit dem Wert von jumpPower
            else
            {
                if (jumpCounter > 0) //If we have extra jumps then jump and decrease jump counter
                {
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    jumpCounter--;
                }
            }
        }
    }

    //Wall jump logic
    private void WallJump()
    {
        //Mathf = -1, wenn der Player nach links schaut
        //Mathf = 1, wenn der Player nach rechts schaut
        //Mathf mit "-" vor der Funktion, um einen Wandsprung in die entgegengesetzte Richtung zu ermöglichen.
        //-> also weg von der Wand und gleichzeitig auch nach oben
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        //wallJumpCooldown += Time.deltaTime;
        wallJumpCooldown = 0;
    }

    //Ground contact check with BoxCast
    //BoxCast betrachtet die Box des jeweiligen BoxColliders.
    //Hier kann man also auch mit dem Player nur noch "halb" auf dem Boden stehen und er berührt noch den Boden (da die Box vom BoxCollider den Boden berührt).
    private bool isGrounded()
    {
        //If nothing is underneath the player then the variable raycastHit == null and isGrounded will be false
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        //If player is on ground then the raycastHit.collider != null and isGrounded will be true
        return raycastHit.collider != null;
    }

    //Wall contact check with BoxCast
    //BoxCast betrachtet die Box des jeweiligen BoxColliders.
    //Hier kann man also auch mit dem Player nur noch "halb" gegen einer Wand gedrückt sein und er berührt noch die Wand (da die Box vom BoxCollider die Wand berührt).
    private bool onWall()
    {
        //If nothing is left or right of the player then the variable raycastHit == null and onWall will be false
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        //If player is on a wall then the raycastHit.collider != null and onWall will be true
        return raycastHit.collider != null;
    }
}
