using BaboOnLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo : MonoBehaviour
{

    [SerializeField] GameObject[] casas;
    [SerializeField] GameObject personaje, isla, casa_base;
    [SerializeField] float margen = 1.0f;
    [SerializeField] int max_habitantes = 5;

    List<GameObject> casa_creadas = new();


    void Start()
    {
        Casa(Vector3.zero, 0);

        //ControladorBG.Rutina(.5f, () => {
        //    Casa(Ubicacion());
        //}, true);
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
            Personaje(Random.Range(2, max_habitantes))
        );
    }

    IEnumerator Personaje(int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            yield return new WaitForSeconds(2.5f);

            // Selecciona una casa aleatoria de las construidas
            Transform spawn = casa_creadas[Random.Range(0, casa_creadas.Count)].transform.GetChilds(0);
            Instantiate(personaje, spawn.position, Quaternion.identity).transform.SetParent(transform);
        }
    }

    Vector3? Ubicacion() 
    {
        // Obtiene el tamaño de la isla
        Vector3 tamaño = isla.GetComponent<Renderer>().bounds.size;
        Vector2 areaIsla = new Vector2(tamaño.x - margen, tamaño.z - margen);

        // Genera un punto aleatorio dentro del área de la isla
        Vector2 ubicacion;
        int contador = 0;
        do
        {
            ubicacion = new Vector3(
                Random.Range(-areaIsla.x / 2, areaIsla.x / 2),
                0,
                Random.Range(-areaIsla.y / 2, areaIsla.y / 2)
            );

        } while (
            contador++ == 100 ||
            casa_creadas.Every((casa) =>
                Vector3.Distance(casa.transform.position, ubicacion) < margen
            )
        );

        return (contador != 100) ? ubicacion : null;
    }

}
