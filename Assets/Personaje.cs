using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Personaje : MonoBehaviour
{
    [SerializeField] float velocidad = 1.0f;

    Controles controles;
    public bool moviendose = true;

    private void Start()
    {
        controles = Instanciar<Controles>.Coger();
    }

    void Update()
    {
        if(moviendose) transform.Translate(Vector3.forward * Time.deltaTime * velocidad, Space.Self);
    }

    public void Cambiar() {
        ControladorBG.Rotar(transform, new Rotacion(
            1f, Quaternion.Euler(new(0, Random.Range(0, 359), 0))
        ));

        moviendose = false;
        ControladorBG.Rutina(1f, () => { moviendose = true; });
    }

    private void OnCollisionStay(Collision collision)
    {
        if (ControladorBG.Esperando("personaje") && !collision.gameObject.CompareTag("Untagged"))
        {
            ControladorBG.IniciarEspera("personaje", 2f);
            Cambiar();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Casa") && controles.personaje == transform) {
            controles.Detener();
        }
    }
}
