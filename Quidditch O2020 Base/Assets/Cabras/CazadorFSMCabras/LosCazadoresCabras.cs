using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosCazadoresCabras : MonoBehaviour
{
    public List<GameObject> TodosLosCazadores;


    public GameObject quienTieneLaPelota = null;
    public int numeroParaQuePersiga = 0;
    // Start is called before the first frame update
    void Start()
    {
        TodosLosCazadores = new List<GameObject>();


        // Obtener el transform del objeto actual
        Transform transformObjetoActual = transform;

        // Verificar si el objeto actual tiene al menos tres hijos
        if (transformObjetoActual.childCount >= 3)
        {
            // Iterar a través de los primeros tres hijos y agregarlos a la lista
            for (int i = 0; i < 3; i++)
            {
                Transform hijo = transformObjetoActual.GetChild(i);
                TodosLosCazadores.Add(hijo.gameObject);
            }
        }
        else
        {
        }
    }


    // Update is called once per frame
    void Update()
    {

    }
    public void TenemosLaPelota(GameObject cazador)
    {
        quienTieneLaPelota = cazador;
        numeroParaQuePersiga = 1;
        for (int i = 0; i < 3; i++)
        {
            if (TodosLosCazadores[i] != cazador)
            {
                TodosLosCazadores[i].GetComponent<elCazadorCabras>().numeroParaSeguir = numeroParaQuePersiga;
                numeroParaQuePersiga++;
            }
        }
    }
}
