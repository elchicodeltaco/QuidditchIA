using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AcompaniarCazCabras : GoapAction
{

    private CazadorCabras Cazador;
    public Vector3 offset = new Vector3(0, 0, 30);

    // Start is called before the first frame update
    public bool terminado = false;
    private float tiempoInicio = 0f;
    [SerializeField] private float duracionAccion = 0f;
    float distanciaDeseada = 5.0f; // Ajusta este valor según sea necesario


    public AcompaniarCazCabras()
    {
        //AgregamosPrecondiciones
      //  AddPrecondition("tienePelota", false);
       // AddPrecondition("rivalTienePelota", false);
       // AddPrecondition("estaPelotaEnJuego", false);

        //AddPrecondition("estaEnRango", false);

    }

    public override bool checkPrecondition(GameObject obj)
    {
        Cazador = GetComponent<CazadorCabras>();
        Target = GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner();
        if (GameManager.instancia.isQuaffleControlled())
        {

            Cazador.steering.teamCohesion = true;
            Cazador.steering.cohesionWeight = 1f;

            Cazador.steering.teamAlignment = true;
            Cazador.steering.alignmentWeight = 1f;

            Cazador.steering.teamSeparation = true;
            Cazador.steering.separationWeight = 1f;
            return true;
        }
        else
        {
            Cazador.steering.teamCohesion = false;
            Cazador.steering.cohesionWeight = 0f;

            Cazador.steering.teamAlignment = false;
            Cazador.steering.alignmentWeight = 0f;

            Cazador.steering.teamSeparation = false;
            Cazador.steering.separationWeight = 0f;
            return false;

        }
        //Asignamos lo que encontramos
    }


    public override bool requiresInRange()
    {
        return false;
    }

    //para que cada accion resetee sus variables
    public override void mReset()
    {

        terminado = false;
        tiempoInicio = 0f;
    }

    public override bool Perform(GameObject obj)
    {

        return true;
    }

    public override bool isDone()
    {

        Debug.Log("Ya lo terminé");

        return terminado;
    }

}
