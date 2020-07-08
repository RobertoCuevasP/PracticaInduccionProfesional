using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corazon : MonoBehaviour
{
    public int cantidad = 10;

    
    private SpriteRenderer renderer;
    private Collider2D collider;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.SendMessageUpwards("CambiarVida", cantidad);

            renderer.enabled = false;

            gameObject.SetActive(false);
            //Destroy(gameObject, 2f);
        }
    }

    private void OnEnable()
    {
        renderer.enabled = true;
        gameObject.SetActive(true);
    }


}
