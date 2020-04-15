using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Auxilliary
    private Rigidbody2D rb;
    private Transform spriteTransform;
    [Header("Auxilliary")]
    public LayerMask groundLayers;
    public ParticleSystem jumpDust;
    public Animator animator;

    // scene objects
    [Header("Scene Objects")]
    public GameObject respawn1;
    public GameObject respawn2;
    public GameObject respawn3;
    public GameObject respawn4;
    public GameObject respawn5;
    public GameObject respawnRock;
    public GameObject bridgeSupport;
    public GameObject levelChanger;
    public Rigidbody2D rockBody;

    // player stats
    [Header("Player Stats")]
    public float walkSpeed;
    [Range(1, 10)]
    public float jumpVelocity;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // internal
    public bool doubleJump; // if double jump is enabled
    public bool onGround;
    private bool jumpPressed; // if input for jump was pressed
    private int jumpCounter = 0; // first or second jump
    private bool canMove = true; // if the user can move

    // animation auxiliaries
    private bool fallAux = true; // because character starts in the air
    private bool groundAux = false;
    private bool runAux = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Create options so user can choose configurations. Can conserve mobile battery running on platform specific fps.
        Screen.SetResolution(450, 800, true); //(good fps)
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60; // -1 or any fps value (30, 60, etc) | needs vSyncCount = 0
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
        // check ground collision (with capsule collider)
        onGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.25f, transform.position.y - 0.40f),
            new Vector2(transform.position.x + 0.25f, transform.position.y - 0.40f), groundLayers);

        if (onGround)
        {
            jumpCounter = 0;
            IsGrounded(true);
            IsJumping(false);
            IsFalling(false);
        }
    }

    void Movement()
    {
        // horizontal
        if (canMove)
        {
            IsRunning(true);
            rb.velocity = new Vector2(walkSpeed * Time.deltaTime, rb.velocity.y);
        } else
        {
            IsRunning(false);
        }
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            // make sure this is called once, because FixedUpdate can happen multiple times between Updates
            jumpPressed = false;
            // first jump
            if (jumpCounter == 0)
            {

                JumpDustParticle();
                JumpAction();
                IsGrounded(false);
                IsJumping(true);
                IsFalling(false);
                canMove = true;

            }
            // second jump
            else if (jumpCounter == 1)
            {
                canMove = true;
                if (doubleJump)
                {
                    FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position)); // shader
                    JumpAction();
                    IsGrounded(false);
                    IsJumping(true);
                    IsFalling(false);
                    animator.Play("Jump", 0, 0f);
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
            IsGrounded(false);
            IsJumping(false);
            IsFalling(true);
        }

        // climbing
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !Input.GetMouseButton(0))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            IsGrounded(false);
            IsJumping(true);
            IsFalling(false);
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
        }

        // testing ground slippery with diferent tags
        //if (collision.gameObject.tag == "groundStop")
        //{
        //    Debug.Log("Ground");
        //    rb.velocity = new Vector2(0, rb.velocity.y);
        //}

        //if (collision.gameObject.tag == "verticalSlope")
        //{
        //    Debug.Log("Vertical");
        //    canMove = false;
        //}
    }

    // ----------------------------------------------------------------------------------------------------------
    private void JumpDustParticle()
    {
        if (onGround)
        {
            jumpDust.Play();
        }
    }

    private void IsGrounded(bool state)
    {
        animator.SetBool("isGrounded", state);
    }

    private void IsRunning(bool state)
    {
        animator.SetBool("isRunning", state);
    }

    private void IsFalling(bool state)
    {
        animator.SetBool("isFalling", state);
    }

    private void IsJumping(bool state)
    {
        animator.SetBool("isJumping", state);
    }

}
