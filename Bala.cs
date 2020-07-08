using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Bala : MonoBehaviour
{
    public int damage = 10;
    public float speed = 2f;
    public Vector2 direccion;
    public float tiempoVida = 3f;
    private Rigidbody2D rigidbody;

    void Start()
    {
        Destroy(this.gameObject, tiempoVida);
        rigidbody = GetComponent<Rigidbody2D>(); 
    }
    
    
    void Update()
    {
        
        
    } 

    private void FixedUpdate()
    {
        Vector2 movement = direccion.normalized * speed;
        rigidbody.velocity = movement;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.CompareTag("Player"))
        {
            //UnityEngine.Debug.Log("Encontré un Player");

            collision.SendMessageUpwards("CambiarVida", damage * (-1)); //Busca el método del objeto que chocó
        }
        else if (collision.CompareTag("Enemigo"))
        {
            collision.SendMessageUpwards("QuitarVida", damage);
        }

        Destroy(this.gameObject);


    }

}
