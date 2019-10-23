using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public Rigidbody2D body;

    [Range(1, 10)]
    public float jumpVelocity;
    public float walkSpeed;
    public bool doubleJump;
    private int jumpCounter = 0;

    // collisions
    public bool onGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ----------------------------------------------------------
        // reload scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Level1");
        }

        // ----------------------------------------------------------
        // auto run
        Movement();

        // ----------------------------------------------------------
        // check ground collision
        onGround = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.35f, transform.position.y - 0.5f), 
            new Vector2(transform.position.x + 0.35f, transform.position.y - 0.5f), groundLayers);
        
        if (onGround)
        {
            jumpCounter = 0;
        }

        // ----------------------------------------------------------
        // jump input
        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Jump"))
        {
            if (jumpCounter == 0)
            {
                rb.velocity = Vector2.up * jumpVelocity;
                jumpCounter++;
            }
            else if (jumpCounter == 1)
            {
                if (doubleJump)
                {
                    rb.velocity = Vector2.up * jumpVelocity;
                    jumpCounter++;
                    FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position)); // shader
                }
            }
        }
    }

    void Movement()
    {
        // horizontal
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        //Debug.Log("Velo: "+rb.velocity);

        // vertical (to climb walls) - 
        // TODO
    }

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
            walkSpeed = 5;
        }

        if (collision.tag == "speed2")
        {
            walkSpeed = 6;
        }

        if (collision.tag == "default_speed")
        {
            walkSpeed = 4;
        }

        // trap
        if (collision.tag == "trap_check")
        {
            if (GameObject.Find("rock_trap"))
            {
                body.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
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
            body.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            collision.gameObject.transform.position = respawnRock.transform.position;
            //Object.Destroy(collision.gameObject);
        }
    }
}
