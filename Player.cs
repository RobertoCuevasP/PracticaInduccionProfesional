using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject balaPrefab;
    public GameObject enemigoRojo;
    public GameObject enemigoAzul;
    public GameObject enemigoAmarillo;

    public float speed = 1.0f;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer renderer;
    private AudioSource audio;

    public RectTransform healthUI;
    public RectTransform gameOverMenu;
    public RectTransform winMenu;

    private Vector2 movimiento;
    
    private bool camino;
    private bool presiono;

    private int direccionX;
    private int direccionY;
    public int vidaInicial = 200;
    private int vida;

    private int enemigosMuertos = 0;


    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        renderer = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        vida = vidaInicial;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        float attackInput = Input.GetAxisRaw("Jump");

        if (horizontalInput != 0)
        {
            animator.SetInteger("Moverse", (int) horizontalInput * 2);
            animator.SetBool("Quieto", false);
            camino = true;
            direccionX = (int) horizontalInput;
            direccionY = 0;
        }
        else if (verticalInput != 0)
        {
            animator.SetInteger("Moverse", (int)verticalInput);
            animator.SetBool("Quieto", false);
            camino = false;
            direccionX = 0;
            direccionY = (int)verticalInput;
        }
        
        movimiento = new Vector2(horizontalInput, verticalInput);

        if(attackInput > 0)
        {
            Attack();
            presiono = true;
        }
        else
        {
            presiono = false;
        }
    }

    void FixedUpdate()
    {
        float horizontalVelocity = movimiento.normalized.x * speed;
        float verticalVelocity = movimiento.normalized.y * speed;
        if (camino)
        {
            rigidbody.velocity = new Vector2(horizontalVelocity, rigidbody.velocity.y);
        } 
        else
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, verticalVelocity);
        }
        
    }


    void LateUpdate()
    {
        animator.SetBool("Quieto", movimiento == Vector2.zero);
    }

    public void CambiarVida(int cantidad)
    {
        if(cantidad < 0)
        {
            StartCoroutine("VisualFeedback");
        }
        vida = vida + cantidad;

        if(vida <= 0)
        {
            vida = 0;
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        } 
        else if(vida > vidaInicial)
        {
            vida = vidaInicial;
        }
        UnityEngine.Debug.Log("Vida Restante: " + vida);

        healthUI.sizeDelta = new Vector2(vida / 2, 20);

    }

    private IEnumerator VisualFeedback()
    {
        renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.color = Color.white;
    }

    void Attack()
    {

        if (balaPrefab != null && !presiono)
        {
            GameObject myBala = Instantiate(balaPrefab, transform.position, Quaternion.identity) as GameObject;
            Bala balaComponente = myBala.GetComponent<Bala>();
            if(direccionX != 0)
            {
                if (direccionX > 0)
                {
                    balaComponente.direccion = Vector2.right;
                }
                else
                {
                    balaComponente.direccion = Vector2.left;
                }
            }
            else
            {
                if(direccionY > 0)
                {
                    balaComponente.direccion = Vector2.up;
                }
                else
                {
                    balaComponente.direccion = Vector2.down;
                }
            }

            audio.Play();

        }

    }

    public void CountEnemigosMuertos()
    {
        enemigosMuertos = enemigosMuertos + 1;
        if(enemigosMuertos == 3)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        vida = vidaInicial;
        healthUI.sizeDelta = new Vector2(vida / 2, 20);
        renderer.color = Color.white;
        enemigosMuertos = 0;


    }

    private void OnDisable()
    {
        this.transform.position = new Vector2(-8.5f, -4.5f);

        if (enemigosMuertos == 3)
        {
            winMenu.gameObject.SetActive(true);
        }
        else
        {
            gameOverMenu.gameObject.SetActive(true);
            enemigoRojo.SetActive(false);
            enemigoAmarillo.SetActive(false);
            enemigoAzul.SetActive(false);
        }
       
    }
}
