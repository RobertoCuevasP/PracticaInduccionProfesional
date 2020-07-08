using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int vida = 100;
    private SpriteRenderer renderer;
    public Player player;
    

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }


    public void QuitarVida(int cantidad)
    {
        vida = vida - cantidad;
        StartCoroutine("VisualFeedback");
        if (vida <= 0)
        {
            vida = 0;
            player.CountEnemigosMuertos();
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }

        UnityEngine.Debug.Log("Vida actual " + vida);

    }

    private IEnumerator VisualFeedback()
    {
        renderer.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        renderer.color = Color.white;
    }

    private void OnEnable()
    {
        vida = 100;
        renderer.color = Color.white;

    }

}
