/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float speed = 4;
    public float jumpForce;
    private Rigidbody2D rb;

    public bool grounded;
    public bool facingLeft;
    public Transform colisor;

    public Transform a;
    public Transform b;
    public LayerMask whatIsGround;

    private float h, v;

    private Animator anim;
    private int idAnimation;

    public bool agachado;

    private bool teste = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        grounded = Physics2D.OverlapArea(a.position, b.position, whatIsGround);
        

        h = Input.GetAxisRaw("Horizontal");
         v = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Velocidade", speed);
        anim.SetBool("grounded", grounded);
        anim.SetInteger("idAnimation", idAnimation);
        anim.SetBool("agachado", agachado);
        anim.SetBool("teste", teste);

        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(new Vector2(0, jumpForce));
        }


        if (h > 0 || h < 0 )
        {
            idAnimation = 1;
        }

        if (h == 0)
        {
            idAnimation = 0;
        }


        if (h < 0 && !facingLeft)
        {
            Flip();
        }
        else if (h > 0 && facingLeft)
        {
            Flip();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 5.5f;
            
            
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 4;
            
        }

        if (Input.GetKey(KeyCode.S))
        {
            agachado = true;
            colisor.transform.localPosition = new Vector2(colisor.localPosition.x, 0.401f);
            
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            agachado = false;
            colisor.transform.localPosition = new Vector2(colisor.localPosition.x, 0.101f);
            
            
        }

        

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(h * speed , rb.velocity.y);
    }


    private void Flip()
    {
        facingLeft = !facingLeft;

        float x = transform.localScale.x;

        x *= -1;

        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}*/