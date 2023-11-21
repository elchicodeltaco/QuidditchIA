using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BuscarElAro : GoapAction
{
    private CazadorCabras Cazador;

    // Start is called before the first frame update
    public bool terminado = false;
    private float tiempoInicio = 0f;
    [SerializeField] private float duracionAccion = 0f;


    public BuscarElAro()
    {
        //AgregamosPrecondiciones
        AddPrecondition("aro", true);
        //AddPrecondition("estaEnRango", false);

        //agregamos efectos
        AddEffect("aro", false);
        AddEffect("AnotarUnGol", true);
        //AddEffect("estaEnRango", true)
      //  AddEffect("aro", false);
      //  AddEffect("estaPelotaEnJuego", false);
    }

    public override bool checkPrecondition(GameObject obj)
    {
        Debug.Log("Aro se ejecuta");

        Cazador = GetComponent<CazadorCabras>();
        List<Transform> objetivos = GetComponentInParent<CabrasTeam>().arosEnemigos;
        float distanciaMenor = 0f;

        Transform objetivoMasCercano = null;

        if(Cazador == GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner())
        {
            if (objetivoMasCercano == null)
            {
                float distanciaMinima = float.MaxValue;
                Debug.Log(objetivos.Count);

                foreach (Transform t in objetivos)
                {
                    if (distanciaMinima > Vector3.Distance(t.transform.position, transform.position))
                    {
                        distanciaMinima = Vector3.Distance(t.transform.position, transform.position);
                        objetivoMasCercano = t;
                    }
                }
                return true;
            }
            else
            {
                Cazador.steering.Target = objetivoMasCercano;
                Cazador.steering.seek = true;
                Cazador.steering.seekWeight = 1f;
                Target = objetivoMasCercano.gameObject;
                return true;
            }
        }
        else
        {
            GetComponentInParent<CazadoresCabras>().PelotaEstáSuelta();
            return false;
        }

        //Primer pase


        //Asignamos lo que encontramos

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
        GameManager.instancia.Quaffle.GetComponent<Quaffle>().Throw(
        Cazador.steering.Target.position - Cazador.transform.position,
        Cazador.ThrowStrenght);
        GameManager.instancia.FreeQuaffle();

        GetComponent<LasBolas>().LaPelota = 0;
        GetComponentInParent<CazadoresCabras>().PelotaEstáSuelta();
        //Cazador.steering.seek = false;
        //Cazador.steering.seekWeight = 0f;
        Target = null;
        return true;
    }

    public override bool isDone()
    {

        Debug.Log("Ya lo terminé");

        return terminado;
    }
}
