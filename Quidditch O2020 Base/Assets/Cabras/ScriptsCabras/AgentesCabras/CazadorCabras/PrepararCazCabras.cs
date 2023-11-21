using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PrepararCazCabras : GoapAction
{
    private CazadorCabras Cazador;

    // Start is called before the first frame update
    public bool terminado = false;
    private float tiempoInicio = 0f;
    [SerializeField] private float duracionAccion = 0f;


    public PrepararCazCabras()
    {
        //AgregamosPrecondiciones
       // AddPrecondition("tienePelota", false);
        //AddPrecondition("estaEnRango", false);

        //agregamos efectos

        //AddEffect("estaEnRango", true)
       // AddEffect("tienePelota", true);
    }

    public override bool checkPrecondition(GameObject obj)
    {
        Cazador = GetComponent<CazadorCabras>();
        Debug.Log("Valor starting position " + Cazador.myStartingPosition);

        if (Cazador.myStartingPosition != null)
        {
            Cazador.steering.Target = Cazador.myStartingPosition;
            Cazador.steering.arrive = true;
            Cazador.steering.arriveWeight = 1f;
            Target = Cazador.myStartingPosition.gameObject;            
            return true;            
        }

        return false;
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

       // Si el juego no ha comenzado, esperar 5 segundos y volver a verificar
        if (!GameManager.instancia.isGameStarted() && GameManager.instancia.IsRecovering() == 0)
       {
            StartCoroutine(EsperarCincoSegundos());
            return false;
       }
        else
        {
            Cazador.steering.Target = GameObject.Find("Quaffle").transform;
            Target = GameObject.Find("Quaffle");
            StopAllCoroutines();
            return true;
        }
 // El juego ha comenzado, la acción está completa
    }

    private IEnumerator EsperarCincoSegundos()
    {
        yield return new WaitForSeconds(2);
        RevisarInicioJuego();
    }

    private void RevisarInicioJuego()
    {
        // Esperar un breve momento antes de volver a verificar

        if (!GameManager.instancia.isGameStarted())
        {
            // Si el juego aún no ha comenzado, esperar nuevamente
            StartCoroutine(EsperarCincoSegundos());
        }
        else
        {
            // El juego ha comenzado, la acción está completa
            terminado = true;
        }
    }

    public override bool isDone()
    {

        Debug.Log("Ya lo terminé");

        return terminado;
    }

}
