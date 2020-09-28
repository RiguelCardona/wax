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
    private float moveInput;
    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;

    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    [Space]
    [Header("Mecanicas")]

    public Vector2 FuerzasMetales; 
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
        
    }

    void FixedUpdate()
    {
            
        //Chequeo suelo

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);



        //MOVIMIENTO

        moveInput = Input.GetAxisRaw("Horizontal");

        if(isGrounded == true){

            rb.velocity = new Vector2(FuerzasMetales.x + (moveInput * speed * Time.deltaTime), FuerzasMetales.y + rb.velocity.y);

            if(Input.GetButton("Jump")){

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);

            }
        }
        else {
            
            rb.velocity = new Vector2(FuerzasMetales.x + (moveInput * speed / 2 * Time.deltaTime), FuerzasMetales.y + rb.velocity.y);

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
