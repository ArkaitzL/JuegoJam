using BaboOnLite;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controles : MonoBehaviour
{
    [SerializeField] public GameObject sangre;

    [Header("CLICK IZQUIERDO")]
    [SerializeField] Levantar levantar; 
    [Header("CLICK DERECHO")]
    [SerializeField] Golpear golpear;


    //IZQUIERDO
    [HideInInspector] public Transform personaje;
    bool arrastrando = false;
    Personaje script = null;

    //DERECHO
    Vector3 posicion_original;

    private void Awake()
    {
        Instanciar<Controles>.Añadir(this);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // CLICK DERECHO
            if (Input.GetMouseButtonDown(1))
            {
                if (hit.collider.CompareTag("Casa")) return;

                Transform puño = golpear.objeto;
                puño.position = hit.point.Y(golpear.altura);
                posicion_original = hit.point.Y(golpear.altura);

                puño.gameObject.SetActive(true);

                ControladorBG.Mover(puño,
                    new Movimiento(golpear.duracion, new Vector3(puño.position.x, (puño.localScale.y / 2), puño.position.z), golpear.animacion_golpe)
                );

                ControladorBG.Rutina(golpear.duracion, () =>
                {
                    ControladorBG.Mover(puño,
                        new Movimiento(golpear.duracion, posicion_original, golpear.animacion_regreso)
                    );
                });

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
                    script = hit.transform.GetComponent<Personaje>();

                    if (script != null)
                    {
                        script.moviendose = false;
                    }

                    ControladorBG.Mover(personaje, new Movimiento(
                        levantar.duracion,
                        new Vector3(personaje.position.x, personaje.position.y + levantar.altura, personaje.position.z),
                        levantar.animacion
                        )
                    );
                }
            }
        }

        // CLICK IZQUIERDO

        // Mantener arrastre
        if (arrastrando && Input.GetMouseButton(0))
        {
            if (!Physics.Raycast(ray, out hit)) {
                Detener();
                return;
            }
            personaje.position = new Vector3(hit.point.x, personaje.position.y, hit.point.z);
        }

        // Detener arrastre
        if (arrastrando && Input.GetMouseButtonUp(0))
        {
            Detener();
        }

    }

    public void Detener() {
        if(personaje != null) personaje.GetComponent<Rigidbody>().useGravity = true;
        arrastrando = false;
        personaje = null;

        if (script != null)
        {
            script.moviendose = true;
            script?.Cambiar();
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

    [Serializable]
    class Golpear
    {
        public Transform objeto;
        public float altura;
        public float duracion;
        public AnimationCurve animacion_golpe;
        public AnimationCurve animacion_regreso;

        public Golpear(Transform objeto, float altura, float duracion, AnimationCurve animacion_golpe, AnimationCurve animacion_regreso)
        {
            this.objeto = objeto;
            this.duracion = duracion;
            this.altura = altura;
            this.animacion_golpe = animacion_golpe;
            this.animacion_regreso = animacion_regreso;

        }
    }
}
