using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public LayerMask groundLayers;

    public GameObject canvasObject;
    public GameObject respawn1;
    public GameObject respawn2;
    public GameObject respawn3;
    public GameObject respawn4;
    public GameObject respawn5;
    public GameObject respawnRock;
    public GameObject bridgeSupport;
    public GameObject levelChanger;
    public Rigidbody2D rockBody;
    
    public float walkSpeed;
    [Range(1, 10)]
    public float jumpVelocity;
    public bool doubleJump;
    private int jumpCounter = 0;
    private bool jumpPressed;
    public bool onGround;

    // ----- better jump -----
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // --- manual physics ---
    private float timerSimulated;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Screen.SetResolution(450, 800, true); //(good fps)
        /*Create option so user can use this. Conserves mobile battery running on platform specific fps.
         * Maybe let user choose fps.*/
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = -1 or 60 or value; // needs vSyncCount = 0
    }

    // Called every physhics step
    private void FixedUpdate()
    {
        GroundCheck();
        Movement();
        Jump();
    }

    private void GroundCheck()
    {
        // check ground collision
        onGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.35f, transform.position.y - 0.40f),
            new Vector2(transform.position.x + 0.35f, transform.position.y - 0.40f), groundLayers);

        if (onGround)
        {
            jumpCounter = 0;
        }
    }

    void Movement()
    {
        // horizontal
        rb.velocity = new Vector2(walkSpeed * Time.deltaTime, rb.velocity.y); // testar com deltatime
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            // first jump
            if (jumpCounter == 0)
            {
                JumpAction();
            }
            // second jump
            else if (jumpCounter == 1)
            {
                if (doubleJump)
                {
                    JumpAction();
                    FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position)); // shader
                }
            }
        }

        BetterJump();
    }

    private void JumpAction()
    {
        rb.velocity = Vector2.up * jumpVelocity;
        jumpCounter++;
    }

    private void BetterJump()
    {
        // falling
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        // climbing
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !Input.GetMouseButton(0))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // jump input
        if ((Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump")))
        {
            jumpPressed = true;
        }

        // tests, remove after
        if (Input.GetKeyDown(KeyCode.V))
        {
            walkSpeed = 350;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            walkSpeed = 200;
        }
        // tests, remove after
    }

    // ----------------------------------------------------------------------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bridge support
        if (collision.tag == "bridge_check")
        {
            bridgeSupport.SetActive(false);
        }

        // speed
        if (collision.tag == "speed")
        {
            walkSpeed = 230;
        }

        if (collision.tag == "speed2")
        {
            walkSpeed = 350;
        }

        if (collision.tag == "default_speed")
        {
            walkSpeed = 200;
        }

        // trap
        if (collision.tag == "trap_check")
        {
            if (GameObject.Find("rock_trap"))
            {
                rockBody.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            }
        }

        // respawn 1
        if (collision.tag == "spike")
        {
            gameObject.transform.position = respawn1.transform.position;
        }

        // respawn 2
        if (collision.tag == "spike2")
        {
            gameObject.transform.position = respawn2.transform.position;
        }

        // respawn 4 - falling from the platforms
        if (collision.tag == "fall1")
        {
            gameObject.transform.position = respawn4.transform.position;
        }

        // respawn 5 - falling from the platforms
        if (collision.tag == "fall2")
        {
            gameObject.transform.position = respawn5.transform.position;
        }

        // win game
        if (collision.tag == "dog")
        {
            //// Load replay menu - maneira antiga
            //canvasObject.SetActive(true);

            // nova maneira com level changer
            levelChanger.GetComponent<LevelChanger>().FadeToNextLevel();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "trap")
        {
            // player goes to respawn 3
            gameObject.transform.position = respawn3.transform.position;

            // respawnPedra
            rockBody.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.gameObject.transform.position = respawnRock.transform.position;
            //Object.Destroy(collision.gameObject);
        }
    }
}
