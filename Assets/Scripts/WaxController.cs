using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaxController : MonoBehaviour
{
    
    private Rigidbody2D rb;
    private bool facingRight = true;
    private Animator anim;

    [Space]
    [Header("Movilidad")]

    public float speed;

    //SALTO
    private bool canJump = true;
    private bool jump;
    public float jumpForce;
    public float jumpCounter;

    //DASH
    private bool canDash = true;
    private bool dash;
    public float dashForce;
    private bool isDashing;

    private float moveInput;

    [Space]
    [Header("Ground control")]
    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Space]
    [Header("Mecanicas")]

    private Vector2 FuerzasMetales; 
    public Transform metal;
    
    public float steelforce;

    public float distanciamaxima;
    public float distanciaminima;
    private float factordistancia;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }



    void Update()
    {
        
        moveInput = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("runningSpeed", Mathf.Abs(moveInput));

        if (moveInput > 0 && !facingRight)
        {
            Flip();
        } else if (moveInput < 0 && facingRight)
        {
            Flip();
        }

        if (moveInput == 0)
        {
            anim.SetBool("isRunning", false);
        } 
        else
        {
            anim.SetBool("isRunning", true);
        }

        if(Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        

        if(Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
        
    }

    void FixedUpdate()
    { 
   
        //Chequeo suelo
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded == true)
        {
            jumpCounter = 2;
            anim.SetBool("isJumping", false);
        }
        else
        {
            if (rb.velocity.y > 0)
            {
                anim.SetBool("isJumping", true);
            }
        }
        /*
        //Cooldowns
        if(dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        } 
        */
        Move(moveInput, jump, dash);
        jump = false;
        dash = false;


        /*



        // MECANICAS ACERO
        

        
        //distancia al metal
        if(distanciaminima > (transform.position - metal.position).magnitude){

            factordistancia = 1 / distanciaminima;

        } else if((transform.position - metal.position).magnitude > distanciamaxima){

            factordistancia = 0;

        } else {

            factordistancia = 1 / (transform.position - metal.position).magnitude;
        }


        //QUEMANDO ACERO
        if((Input.GetAxisRaw("Gatillos") > 0)){
            
            
            if((transform.position - metal.position).magnitude < distanciamaxima){
                
                //Draw metal forces
                Debug.DrawLine(transform.position, metal.position, Color.blue);
                //Steel push
                FuerzasMetales += new Vector2((steelforce * Input.GetAxisRaw("Gatillos") * factordistancia * (transform.position - metal.position).normalized).x, (steelforce * Input.GetAxisRaw("Gatillos") * factordistancia * (transform.position - metal.position).normalized).y);
            } else {
                FuerzasMetales = FuerzasMetales * Time.deltaTime;
            }
            
        }  else
        {
            FuerzasMetales = new Vector2(0,0); 
        }


        */

    }

    private void Move(float moveInput, bool jump, bool dash)
    {
        rb.velocity = new Vector2(FuerzasMetales.x + (moveInput * speed * Time.fixedDeltaTime), FuerzasMetales.y + rb.velocity.y);

        if (jump)
        {
            Jump();
        }

        if (dash && canDash)
        {
            Dash();
        }
    }

    private void Jump()
    {
        if(canJump)
        {
            anim.SetTrigger("takeOff");
            if (jumpCounter == 2)
            {
                //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                rb.AddForce(new Vector2(0f, jumpForce));
                jumpCounter--;
            }
            else if (jumpCounter == 1)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f); //Frena la caida
                rb.AddForce(new Vector2(0f, jumpForce/1.1f)); //Es mas debil al primero
                jumpCounter--; 
            }
        }
    }

    private void Dash()
    {
        StartCoroutine(DashCooldown());
        if (isDashing)
        {
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            //rb.AddForce(new Vector2(dashForce, 0f));
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;        
    }

	IEnumerator DashCooldown()
	{
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		isDashing = false;
		yield return new WaitForSeconds(1f);
		canDash = true;
	}
}


