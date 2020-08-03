/*
*Class PlayerController2 contains all of player 2's functions: control buttons, damage/health system, 
*and animations.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController2 : MonoBehaviour
{
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    //how fast character can shoot
    //players are not allowed to spam trigger

    public float speed;
    //how fast character will move
    
    public float jumpForce;
    //how far up a player can jump

    private float moveInput2;
    //left and right keys

    private Rigidbody2D rb2D;
    //rigidbody component

    private bool facingRight = true;
    //boolean for direction

    private bool isGrounded;
    //is character on ground 

    public Transform groundCheck;
    //sensor under player 

    public float checkRadius;
    //how big is the circle of ground check

    public LayerMask whatIsGrounded;
    //ground is assigned; layer mask

    private int extraJumps;
    //made to private to not hard code it

    public int extraJumpsValue;
    //allows for double jump

    private Animator animator;
    
    public int startingHealth = 100;
    // Player starting helath set to 100

    public int currentHealth;
    // Variable for health if damaged

    public Slider healthSlider;
    // Slider to update to health of player

    public bool isDead = false;
    // Flag for if player health is 0

    public bool damaged;
    // Flag for if player takes damage

    [SerializeField]
    private Transform firePoint;
    //where bullet comes out of player when shooting

    [SerializeField]
    private GameObject weapon;
    //the weapon prefab
    void Start()
    {
        healthSlider = GameObject.Find("/HUDCanvas/HealthUI2/HealthSlider").GetComponent<Slider>();
        //assigns appropriate health bar to player 
        extraJumps = extraJumpsValue;
        rb2D = GetComponent<Rigidbody2D>();
        //tweak rigidbody with script
        //using animator component
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {//manage physics related aspects of game
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);
        //generate circle by players feet to detect ground
        float slower2 = 0f;


        //Controller for player 2
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput2 = Input.GetAxis("HorizontalP2"); //moving right = 1; moving left = -1
            slower2 = moveInput2;
            rb2D.velocity = new Vector2(moveInput2 * speed, rb2D.velocity.y);
        }
        else
        {
            moveInput2 = Input.GetAxis("HorizontalP2"); //moving right = 1; moving left = -1
            slower2 = slower2 - .00001f;
            rb2D.velocity = new Vector2(moveInput2 * slower2, rb2D.velocity.y);
        }
       
        //not to influence y axis
        if (facingRight == false && moveInput2 > 0)
        {//moving left while facing left
            Flip();
        }
        else if (facingRight == true && moveInput2 < 0)
        {//moving right while facing right
            Flip();
        }

    }

    void Update()
    {

        if (isGrounded == true)
        {//resets jumps once player hits the ground
            extraJumps = extraJumpsValue;
        }
        //More player 2 controls
        if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps > 0)
        {//when up arrow is pressed
            rb2D.velocity = Vector2.up * jumpForce;
            extraJumps--;
            //jump amount goes down by one
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow) && extraJumps == 0 && isGrounded == true)
        {//no extrajumps left but would still jump
            rb2D.velocity = Vector2.up * jumpForce;
        }

        if (moveInput2 == 0)
        {//character not moving animation 
            animator.SetBool("AnimationState", false);
        }
        else
        {//character moving animation
            animator.SetBool("AnimationState", true);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)&&Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            animator.SetTrigger("Attack");
            Shoot();
        }

    }
    //Shoot player 2's weapon
    void Shoot()
    {
        Instantiate(weapon, firePoint.position, firePoint.rotation);
    }
    //Player 2 faces the other way
    void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    //Player 2's health system
    //This method is called whenever player 2 receives damage.
    public void TakeDamage(int amount) //damage amount
    {
        damaged = true;

        currentHealth -= amount; //lower health

        healthSlider.value = currentHealth; //move healthbar UI

        if (currentHealth <= 0 && !isDead) //Kill player if health drops below 100
        {
            Death();
        }
    }
    //This method is called when player 2's health drops below 0
    void Death()
    {
        Destroy(gameObject);

        isDead = true;

        isGrounded = false;

        GlobalControl.Instance.Win = 1; //Tell end screen UI which player won the game.

        SceneManager.LoadScene(5); //Transition to end screen

    }

    //Player does animation on contact of weapon.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            animator.SetTrigger("Hurt");
            TakeDamage(10);
        }
        else if (other.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(100);
        }

    }

    

}
