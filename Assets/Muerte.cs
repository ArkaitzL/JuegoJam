using BaboOnLite;
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
            Instantiate(Instanciar<Controles>.Coger().sangre, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            //Animacion, vidas--, sangre...
        }
    }
}
