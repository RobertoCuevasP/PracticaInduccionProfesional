using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    /*Muchos de los Atributos se inicializan desde Unity, no desde código*/

    public int numeroRandom;

    public GameObject balaPrefab;
    public GameObject shooter;
    public GameObject target;


    private Rigidbody2D rigidbody;
    public Animator animator;
    private AudioSource audio;

    public float speed;
    public float tiempoEspera;
    
    private RaycastHit2D hitInfo;

    private bool estaAtacando;

    public Vector2 posicionInicial;


    void Awake()
    {
        animator = GetComponent<Animator>(); 
        rigidbody = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();

    }
    void Start()
    {
        animator.SetBool("Quieto", true);
        CreateTarget();
        SetTarget();
        StartCoroutine("DirigirseAlTarget");
    }

    private void CreateTarget()
    {
        if (target == null)
        {
            target = new GameObject("Target");
        }
    }

    private void SetTarget()
    {
        //Este método da un target al enemigo para dirigirse a él
        numeroRandom = UnityEngine.Random.Range(0, 4);
        if (numeroRandom == 0) // arriba
        {
            target.transform.position = new Vector2(transform.position.x, transform.position.y + 1f);            
        }
        else if (numeroRandom == 1) //abajo
        {
            target.transform.position = new Vector2(transform.position.x, transform.position.y - 1f);            
        }
        else if (numeroRandom == 2) //izquierda
        {
            target.transform.position = new Vector2(transform.position.x - 1f, transform.position.y);        
        }         
        else //derecha
        {
            target.transform.position = new Vector2(transform.position.x + 1f, transform.position.y);
        }        
        transform.localScale = new Vector3(1, 1, 1);
        DetectarColliders();
    }

    private void DetectarColliders() //detecta otros objetos por el mapa, ayudará a determinar qué acción realizar
    {
        //Este método verifica si el target es accesible de llegar        
        if (numeroRandom == 0) // arriba
        {
            hitInfo = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y + 0.5f), Vector2.up);
        }
        else if (numeroRandom == 1) //abajo
        {
            hitInfo = Physics2D.Raycast(new Vector2(this.transform.position.x, this.transform.position.y -0.5f), Vector2.down);          

        }
        else if (numeroRandom == 2) //izquierda
        {
            hitInfo = Physics2D.Raycast(new Vector2(this.transform.position.x - 0.5f, this.transform.position.y), Vector2.left);

        }
        else //derecha
        {
            hitInfo = Physics2D.Raycast(new Vector2(this.transform.position.x + 0.5f, this.transform.position.y), Vector2.right);            

        }

        if (hitInfo)
        {
           
            if (hitInfo.collider.tag == "Muro")
            {
                Transform muro = hitInfo.transform;
                if(hitInfo.distance <= 0.0f)
                {
                    SetTarget();
                }
            }
            else if(hitInfo.collider.tag == "Player") 
            {
                if(balaPrefab != null)
                {
                    audio.Play();
                    Shoot();
                }
                
                SetTarget();
            }
        }
        else
        {
            UnityEngine.Debug.Log("Info es null");
        }
    }

    private IEnumerator DirigirseAlTarget()
    {
        while (Vector2.Distance(this.transform.position, target.transform.position) > 0.05)
        {
            animator.SetInteger("Moverse", numeroRandom);
            animator.SetBool("Quieto", false);
            Vector2 direccion = target.transform.position - this.transform.position;

            float xDireccion = direccion.x;
            float yDireccion = direccion.y;

            transform.Translate(direccion.normalized * speed * Time.deltaTime);

            yield return null; //Sirve para romper el método y llamarlo nuevamente
        }

        this.transform.position = new Vector2(target.transform.position.x, target.transform.position.y); // Se asegura de que llegó exactamente al target
        SetTarget();

        animator.SetBool("Quieto", true);

        yield return new WaitForSeconds(tiempoEspera);
           
        StartCoroutine("DirigirseAlTarget");
    }

   
    private void Shoot()
    {
        if(balaPrefab != null && shooter != null)
        {
            GameObject myBala = Instantiate(balaPrefab, transform.position, Quaternion.identity) as GameObject;

            Bala balaComponente = myBala.GetComponent<Bala>();

            if (numeroRandom == 0) // arriba
            {
                balaComponente.direccion = Vector2.up;

            }
            else if (numeroRandom == 1) //abajo
            {
                balaComponente.direccion = Vector2.down;
            }
            else if (numeroRandom == 2) //izquierda
            {
                balaComponente.direccion = Vector2.left;
            }
            else //derecha
            {
                balaComponente.direccion = Vector2.right;
            }
        }
    }

    private void OnEnable() //Qué ocurrirá
    {
        StartCoroutine("DirigirseAlTarget");
    }

    private void OnDisable()
    {
        this.transform.position = posicionInicial;
        if(target == null)
        {
            CreateTarget();
        }
        SetTarget();
        
    }

}
