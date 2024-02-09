using BaboOnLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : MonoBehaviour
{

    [SerializeField] GameObject[] casas;
    [SerializeField] GameObject personaje, isla, casa_base, enemigo;
    [SerializeField] float margen = 1.0f;
    [SerializeField] int casa_tiempo = 20, enemigo_tiempo = 20;

    List<GameObject> casa_creadas = new();
    Vector2 areaIsla;

    void Start()
    {
        // Obtiene el tamaño de la isla
        Vector3 tamaño = isla.GetComponent<Renderer>().bounds.size;
        areaIsla = new Vector2(tamaño.x - margen, tamaño.z - margen);

        Casa(Vector3.zero, 0);

        ControladorBG.Rutina(casa_tiempo, () =>
        {
            Casa(Ubicacion());
        }, true);

        ControladorBG.Rutina(enemigo_tiempo, () =>
        {
            StartCoroutine(
                Enemigo(Random.Range(3, 10), Ubicacion())
           );
        }, true);
    }

    void Casa(Vector3? posicion, int? index = null)
    {
        if (posicion == null) return;

        //Crea la casa base
        GameObject casa = Instantiate(
            casa_base,
            (Vector3)posicion,
            Quaternion.identity
        );
        casa.transform.SetParent(transform);

        //crea la skin
        Instantiate(
            casas[(index == null) ? Random.Range(0, casas.Length) : (int)index],
            Vector3.zero,
            Quaternion.identity
        ).transform.SetParent(casa.transform, false);

        //Guarda la referencia
        casa_creadas.Add(casa);

        //Crea a los habitantes
        StartCoroutine(
            Personaje(Random.Range(2, 5), casa.transform)
        );
    }


    IEnumerator Personaje(int cantidad, Transform casa)
    {
        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(2.5f);

            // Selecciona una casa aleatoria de las construidas
            Transform spawn = casa.transform.GetChilds(0);
            Instantiate(personaje, spawn.position, Quaternion.identity).transform.SetParent(transform);
        }
    }

    IEnumerator Enemigo(int cantidad, Vector3? ubicacion)
    {
        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(2.5f);

            // Selecciona una casa aleatoria de las construidas
            Instantiate(enemigo, (Vector3)ubicacion, Quaternion.identity).transform.SetParent(transform);
        }
    }

    Vector3? Ubicacion() 
    {
        // Genera un punto aleatorio dentro del área de la isla
        Vector3? ubicacion = null;
        int contador = 0;
        do
        {
            if (contador++ == 100)
            {
                break;
            }

            //Crea un punto random
            ubicacion = new Vector3(
                Random.Range(-areaIsla.x / 2, areaIsla.x / 2),
                0,
                Random.Range(-areaIsla.y / 2, areaIsla.y / 2)
            );

        } while (
            //Comprueba que no este bug con una casa
            casa_creadas.Some((casa) =>
                Vector3.Distance(casa.transform.position, (Vector3)ubicacion) < margen
            )
        );

        contador.Log("Intentos: ");

        return (contador != 100) ? ubicacion : null;
    }

}
