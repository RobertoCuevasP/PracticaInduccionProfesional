using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PrefabInstantaneo : MonoBehaviour
{

    public GameObject prefab;
    public Transform puntoUbicacion;
    public float tiempoVida;

    public void Instantiate()
    {
        GameObject instantiatedObject = Instantiate(prefab, puntoUbicacion.position, Quaternion.identity) as GameObject;

        if(tiempoVida > 0)
        {
            Destroy(instantiatedObject, tiempoVida);
        }
    }
}
