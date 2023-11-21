using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadoresCabras : MonoBehaviour
{
    public List<GameObject> TodosLosCazadores;

    public bool yaSeReviso;
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


    public void PelotaEstáSuelta(){
        for (int i = 0; i < 3; i++)
        {
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("TienePelota", false);
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("rivalTienePelota", false);
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("estaPelotaEnJuego", true);

          //  TodosLosCazadores[i].GetComponent<GoapAgent>().repensar();
        }

    }
    public void ElRivalTieneLaPelota()
    {
        for(int i = 0; i < 3; i++)
        {
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("TienePelota", false);
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("rivalTienePelota", true);
            TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("estaPelotaEnJuego", false);

           // TodosLosCazadores[i].GetComponent<GoapAgent>().repensar();
        }
        yaSeReviso = true;
    }
    public void TenemosLaPelota(GameObject cazador)
    {
        for (int i = 0; i < 3; i++)
        {   
            if(cazador != TodosLosCazadores[i])
            {
                TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("TienePelota", false);
                TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("rivalTienePelota", false);
                TodosLosCazadores[i].GetComponent<GoapAction>().AddEffect("estaPelotaEnJuego", false);


              //  TodosLosCazadores[i].GetComponent<GoapAgent>().repensar();
            }
        }
    }
}