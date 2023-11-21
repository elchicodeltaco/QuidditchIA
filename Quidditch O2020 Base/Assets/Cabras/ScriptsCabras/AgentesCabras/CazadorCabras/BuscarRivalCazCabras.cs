using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuscarRivalCazCabras : GoapAction
{
    private CazadorCabras Cazador;

    // Start is called before the first frame update
    public bool terminado = false;
    private float tiempoInicio = 0f;
    [SerializeField] private float duracionAccion = 0f;


    public BuscarRivalCazCabras()
    {

        //AgregamosPrecondiciones
       // AddPrecondition("tienePelota", false);

        //agregamos efectos
      //  AddEffect("tienePelota", true);
      //  AddEffect("AnotarUnGol", true);

    }
    public override bool checkPrecondition(GameObject obj)
    {
        Cazador = GetComponent<CazadorCabras>();

        if (GameManager.instancia.isQuaffleControlled())
        {
            if (Cazador.myTeam.isTeammate(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
            {
                Cazador.steering.Target = GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner().transform;
                Target = GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner();
                Cazador.steering.seek = true;
                Cazador.steering.seekWeight = 1f;
                return true;
            }
            else
            {
                GetComponentInParent<CazadoresCabras>().TenemosLaPelota(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner());
                return false;
            }
        }
        return false;
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
        terminado = true;
        return true;
    }

    public override bool isDone()
    {
        return terminado;
    }
}
