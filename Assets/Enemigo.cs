using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] float velocidad = 1.0f;

    GameObject personaje;

    void Start()
    {
        // Busca al personaje con el tag "Personaje"
        personaje = BuscarPersonajeMasCercano();
    }

    void Update()
    {
        if (personaje != null)
        {
            // Actualiza la posición del enemigo para seguir al personaje
            transform.position = Vector3.MoveTowards(transform.position, personaje.transform.position, Time.deltaTime * velocidad);

            // Si el personaje ha sido destruido, busca al siguiente personaje más cercano
            if (Vector3.Distance(personaje.transform.position, transform.position) < .1f)
            {
                personaje = BuscarPersonajeMasCercano();
            }
        }
    }

    GameObject BuscarPersonajeMasCercano()
    {
        GameObject[] personajes = GameObject.FindGameObjectsWithTag("Personaje");
        GameObject personajeMasCercano = null;
        float distanciaMasCercana = Mathf.Infinity;

        foreach (GameObject personaje in personajes)
        {
            float distancia = Vector3.Distance(transform.position, personaje.transform.position);
            if (distancia < distanciaMasCercana)
            {
                distanciaMasCercana = distancia;
                personajeMasCercano = personaje;
            }
        }

        return personajeMasCercano;
    }
}