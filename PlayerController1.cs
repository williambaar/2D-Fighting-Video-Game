/*
*Class PlayerController1 contains all of player 1's functions: control buttons, damage/health system, 
*and animations.
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController1 : MonoBehaviour
{
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    //how fast the player can shoot
    //player cannot spam trigger

    public float speed;
    //how fast character will move

    public float jumpForce;
    //how far up a player can jump

    private float moveInput;
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
    //Where the bullet spawns when shot.

    [SerializeField]
    private GameObject weapon;
    //The weapon prefab
    void Start()
    {
        healthSlider = GameObject.Find("/HUDCanvas/HealthUI/HealthSlider").GetComponent<Slider>();
        //Appoints appropriate health bar to player
        extraJumps = extraJumpsValue;
        //Allow the player to double jump
        rb2D = GetComponent<Rigidbody2D>();
        //tweak rigidbody with script
        //using animator component
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {//manage physics related aspects of game
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGrounded);
        //generate circle by players feet to detect ground

        //Player 1 controls
        //moving right = 1; moving left = -1
        float slower = 0f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            moveInput = Input.GetAxis("HorizontalP1");
            slower = moveInput;
            rb2D.velocity = new Vector2(moveInput * speed, rb2D.velocity.y);
        }
        else
        {
            moveInput = Input.GetAxis("HorizontalP1");
            slower = slower - .0001f;
            
            rb2D.velocity = new Vector2(moveInput * slower, rb2D.velocity.y);
        }
        
        //not to influence y axis
        if (facingRight == false && moveInput > 0)
        {//moving left while facing left
            Flip();
        }
        else if (facingRight == true && moveInput < 0)
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
        //More player 1 controls 
        if (Input.GetKeyDown(KeyCode.W) && extraJumps > 0)
        {//when up arrow is pressed
            rb2D.velocity = Vector2.up * jumpForce;
            extraJumps--;
            //jump amount goes down by one
        }
        else if (Input.GetKeyDown(KeyCode.W) && extraJumps == 0 && isGrounded == true)
        {//no extrajumps left but would still jump
            rb2D.velocity = Vector2.up * jumpForce;
        }

        if (moveInput == 0)
        {//character not moving
            animator.SetBool("AnimationState", false);
        }
        else
        {//character moving
            animator.SetBool("AnimationState", true);
        }
        //Shoot weapon
         if (Input.GetKeyDown (KeyCode.S)&&Time.time >nextFire)
        {
            nextFire = Time.time + fireRate;
            animator.SetTrigger("Attack");
            Shoot();
        }
    }
    //This method is called whenever player 1 shoots.
    void Shoot()
    {
        Instantiate(weapon, firePoint.position, firePoint.rotation);
    }
    void Flip()
    {
        facingRight = !facingRight;
        //will face other way
        transform.Rotate(0f, 180f, 0f);
    }
    
    //This method manipulates player 1's health system.
    public void TakeDamage(int amount)
    {
        damaged = true;
        

        currentHealth -= amount; //Decrease health


        healthSlider.value = currentHealth; //Change health bar UI

        if (currentHealth <= 0 && !isDead) //Kill player
        {
            Death();
        }

       
    }

    //This method is called whenever player 1's health drops below 0
    void Death()
    {
        Destroy(gameObject); //Kill player

        isDead = true;

        isGrounded = false;

        GlobalControl.Instance.Win = 2;//Signal to end screen winning player

        SceneManager.LoadScene(5);//Transition to end screen

    }

    //Player 1 animates on contact with a weapon. 
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
