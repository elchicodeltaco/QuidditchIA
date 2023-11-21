using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuscarPelCazCabras : GoapAction
{
    private CazadorCabras Cazador;
    // Start is called before the first frame update
    public bool terminado = false;
    private float tiempoInicio = 0f;
    [SerializeField] private float duracionAccion = 0f;


    public BuscarPelCazCabras()
    {
 
        //AgregamosPrecondiciones
        AddPrecondition("tienePelota", true);
        // AddPrecondition("rivalTienePelota", false);
        //  AddPrecondition("estaPelotaEnJuego", false);


        //agregamos efectos
        AddEffect("tienePelota", false);
        AddEffect("aro", true);

    }
    public override bool checkPrecondition(GameObject obj)
    {
       // Cazador = GetComponent<CazadorCabras>();
        Debug.Log("Buscar pelota se ejecuta");
        /*
                if (GameManager.instancia.isQuaffleControlled())
                {
                    if (Cazador.miTeam.isCabra(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
                    {
                        GetComponentInParent<CazadoresCabras>().TenemosLaPelota(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner());
                        return false;
                    }
                    else
                    {
                        GetComponentInParent<CazadoresCabras>().ElRivalTieneLaPelota();
                        return false;
                    }

                }
                else
                {
                    GameObject objetivo = GameObject.Find("Quaffle");
                    //Primer pase
                    if (objetivo != null)
                    {
                        Target = objetivo;
                        Cazador.steering.Target = objetivo.transform;
                        Cazador.steering.pursuit = true;
                        Cazador.steering.pursuitWeight = 1;
                        return true;
                    }
                    else
                    {
                        Target = null;
                        return false;
                    }
                }

                */
        //Asignamos lo que encontramos
        return true;
    }
        

    public override bool requiresInRange()
    {
        return true;
    }

    //para que cada accion resetee sus variables
    public override void mReset()
    {

        terminado = false;
        tiempoInicio = 0f;
    }

    public override bool Perform(GameObject obj)
    {
        /*
        if (!GameManager.instancia.isQuaffleControlled())
        {
            GameManager.instancia.ControlQuaffle(gameObject);
            GetComponent<LasBolas>().LaPelota = 1;
          //  GetComponentInParent<CazadoresCabras>().TenemosLaPelota(this.gameObject);
            terminado = true;
            return true;
        }
        else
        {
            return false;
        }
        //GetComponentInParent<CazadoresCabras>().TenemosLaPelota(this.gameObject);*/
        return true;
    }

    public override bool isDone()
    {
        return terminado;
    }
}
