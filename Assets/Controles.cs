using BaboOnLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controles : MonoBehaviour
{

    [Header("CLICK IZQUIERDO")]
    [SerializeField] Levantar levantar_personaje; 

    bool arrastrando = false;
    Transform personaje;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // CLICK DERECHO
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Coordenadas: " + hit.point);
            }

            // CLICK IZQUIERDO

            // Iniciar arrastre
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Personaje"))
                {
                    arrastrando = true;
                    personaje = hit.transform;
                    personaje.GetComponent<Rigidbody>().useGravity = false;

                    ControladorBG.Mover(personaje, new Movimiento(
                        levantar_personaje.duracion,
                        new Vector3(personaje.position.x, personaje.position.y + levantar_personaje.altura, personaje.position.z),
                        levantar_personaje.animacion
                        )
                    );
                }
            }
        }

        // CLICK IZQUIERDO

        // Mantener arrastre
        if (arrastrando && Input.GetMouseButton(0))
        {
            personaje.position = new Vector3(hit.point.x, personaje.position.y, hit.point.z);
        }

        // Detener arrastre
        if (arrastrando && Input.GetMouseButtonUp(0))
        {
            personaje.GetComponent<Rigidbody>().useGravity = true;
            arrastrando = false;
            personaje = null;
        }
    }

    [Serializable]
    class Levantar
    { 

        public float altura;
        public float duracion;
        public AnimationCurve animacion;

        public Levantar(float altura, float duracion, AnimationCurve animacion)
        {
            this.altura = altura;
            this.duracion = duracion;
            this.animacion = animacion;
        }
    }
}
