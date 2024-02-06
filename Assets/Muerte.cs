using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Muerte : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puño"))
        {
            gameObject.SetActive(false);
            //Animacion, vidas--, sangre...
        }
    }
}
