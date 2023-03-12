using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerBehavior : MonoBehaviour
{
    [Header("References")]
    public FixedJoystick joy;
    public CinemachineConfiner2D camConf;

    private Rigidbody2D rb;
    private TrailRenderer tr;

    [Header("Movement")]
    public float speed = 5;
    public float acceleration;
    public float decceleration;
    public float velocityAmount;
    public float frictionAmount;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCoyoteTime;
    public float jumpBufferTime;
    public float fallgravitymultiplier;
    public int maxJump;

    private int jumpCount;
    private float gravityScale;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    [Range(0f, 1f)]
    public float dashcooldown;

    private bool canDash = true;
    private bool isDashing;
    private Vector2 dashDirection;

    [Header("Checks")]
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;

    [SerializeField] private float lastGroundedTime;
    private float lastJumpTime;

    [Header("Collectible")]
    public GameObject featherUI;
    public TextMeshProUGUI collectibleUI;
    public int featherNumber;

    [Header("Other")]
    public GameObject wings;
    public Sprite goldenwings;
    public Sprite cpOff;
    public Sprite cpOn;

    private bool facingRight = true;
    private Vector2 checkpoint;
    private Collider2D camFailsafe;

    [Header("Animations & Sounds")]
    public AudioClip acJump;
    public AudioClip acLand;
    public AudioClip acDash;
    public AudioClip acCollec;
    public AudioClip acWings;

    private AudioSource aS;
    private Animator Anim;
    private bool isJumping = false;

    void Start()
    {
        checkpoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
        aS = GetComponent<AudioSource>();
        Anim = GetComponent<Animator>();
        gravityScale = rb.gravityScale;
        jumpCount = maxJump;
    }

    void FixedUpdate()
    {
        Anim.SetBool("IsGrounded", isGrounded);
        Anim.SetBool("IsDashing", isDashing);

        lastJumpTime -= Time.deltaTime;
        lastGroundedTime -= Time.deltaTime;

        //GROUNDED
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (isGrounded && !isJumping)
        {
            canDash = true;
            lastGroundedTime = jumpCoyoteTime;
            jumpCount = maxJump;
        }

        //APPEL DE FONCTION
        Run();
        VelocityChecks();
        Flip();
        SpriteAnimation();

        if (isDashing)
        {
            rb.velocity = dashDirection.normalized * dashSpeed;
            rb.gravityScale = 0;
        }
    }

    public void Run()
    {
        float joydir = 0;
        if (joy.Horizontal < 0)
        {
            joydir = -1;
            Anim.SetBool("IsWalking", true);
        }
        else if (joy.Horizontal > 0)
        {
            joydir = 1;
            Anim.SetBool("IsWalking", true);
        }
        else
        {
            Anim.SetBool("IsWalking", false);
        }

        float targetSpeed = joydir * speed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velocityAmount) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
    } //FINI

    public void Jump()
    {  
        if (jumpCount != 0) //lastGroundedTime >= 0f && 
        {
            aS.PlayOneShot(acJump);

            rb.velocity = Vector2.up * jumpForce;
            isJumping = true;
            jumpCount--;
        }
    }

    public void FallGravity()
    {
        rb.gravityScale = gravityScale * fallgravitymultiplier;
    }

    public void Dash()
    {
        if (canDash)
        {
            aS.PlayOneShot(acDash);

            isDashing = true;
            canDash = false;
            tr.emitting = true;
            dashDirection = new Vector2(joy.Horizontal, joy.Vertical);
            wings.SetActive(false);

            if (dashDirection == Vector2.zero)
            {
                dashDirection = new Vector2(transform.localScale.x, 0);
            }

            StartCoroutine(StopDash());
        }
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        canDash = false;
        isDashing = false;
        tr.emitting = false;
        rb.gravityScale = gravityScale;
        rb.velocity *= dashcooldown;

        //FallGravity();
    }

    public void VelocityChecks()
    {
        if (rb.velocity.y < 0)
        {
            FallGravity();
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }

    private void Flip()
    {
        if (facingRight == false && joy.Horizontal > 0)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
        else if (facingRight == true && joy.Horizontal < 0)
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }
    }

    private void SpriteAnimation()
    {      
        if (isDashing)
        {
            Anim.SetBool("IsDashing", true);
        }
        else if (isJumping)
        {
            Anim.SetBool("IsDashing", false);

            if (rb.velocity.y > 0)
            {
                Anim.SetBool("IsJumping", true);
                wings.SetActive(true);
            }
            else
            {
                Anim.SetBool("IsJumping", false);
                wings.SetActive(false);
                isJumping = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Checkpoint"))
        {
            checkpoint = collision.transform.position;
            GameObject[] cps = GameObject.FindGameObjectsWithTag("Checkpoint");
            for (int i = 0; i < cps.Length; i++)
            {
                cps[i].GetComponent<SpriteRenderer>().sprite = cpOff;
            }
            collision.GetComponent<SpriteRenderer>().sprite = cpOn;
        }
        else if (collision.transform.CompareTag("DiaTrigger"))
        {
            collision.GetComponent<DialogueTrigger>().StartDialogue();
            collision.transform.tag = "Untagged";
        }
        else if (collision.transform.CompareTag("KillZone"))
        {
            transform.position = checkpoint;
        }
        else if (collision.transform.CompareTag("Collectible"))
        {

            if (featherNumber < 12 || featherNumber == 0)
            {
                aS.PlayOneShot(acCollec);

                featherUI.SetActive(true);
                featherNumber++;
                collectibleUI.text = ": " + featherNumber + "/12";
            }
            else if(featherNumber == 12)
            {
                aS.PlayOneShot(acWings);

                featherUI.SetActive(false);
                wings.GetComponent<SpriteRenderer>().sprite = goldenwings;
                maxJump = 2;
                jumpForce = 12;
            }

            Destroy(collision.gameObject);
        }
        else if (collision.transform.CompareTag("Vortex"))
        {
            SceneManager.LoadScene(3);
        }
    }
}
