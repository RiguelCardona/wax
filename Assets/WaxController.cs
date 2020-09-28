using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaxController : MonoBehaviour
{
    
    private Rigidbody2D rb;

    [Space]
    [Header("Movilidad")]

    public float speed;

    public float jumpForce;
    public float jumpCounter;
    private float jumpTime;

    public float dashforce;
    public float dashCooldown;

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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Jump") && jumpCounter > 0){

            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCounter--;

        }
        

        if(Input.GetButtonDown("Dash") && dashCooldown <= 0){

            rb.AddForce(Vector2.right * dashforce, ForceMode2D.Impulse);
            dashCooldown = 3;
            Debug.Log("Dashing");
        }               
    }

    void FixedUpdate()
    {
        //MOVIMIENTO LATERAL
        moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(FuerzasMetales.x + (moveInput * speed * Time.deltaTime), FuerzasMetales.y + rb.velocity.y);

            
        //Chequeo suelo
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if(isGrounded == true){

            jumpCounter = 1;

        }


        //Cooldowns

        if(dashCooldown > 0){
            dashCooldown -= Time.deltaTime;
        } 
        
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
            
        }  else {

            FuerzasMetales = new Vector2(0,0); 
        }

    }
}
