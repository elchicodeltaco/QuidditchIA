using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorCazCabras_BuscarAro : FSMStatesCazCabras
{
    private elCazadorCabras Cazador;

    public CazadorCazCabras_BuscarAro(FSMCazCabras fsm, Animator animator, elCazadorCabras Cazador) : base(fsm, animator)
    {
        this.Cazador = Cazador; 
    }



    public override void Enter()
    {
        base.Enter();

        //Buscamos la Quaffle
        List<Transform> objetivos = Cazador.GetComponentInParent<CabrasTeam>().arosEnemigos;
        float distanciaMenor = 0f;

        Transform objetivoMasCercano = null;
        float distanciaMinima = float.MaxValue;


        foreach (Transform t in objetivos)
        {
            if (distanciaMinima > Vector3.Distance(t.transform.position, Cazador.transform.position))
            {
                distanciaMinima = Vector3.Distance(t.transform.position, Cazador.transform.position);
                objetivoMasCercano = t;
            }
        }
        //Cazador.steering.Target = objetivoMasCercano;
        //Cazador.steering.seek = true;
        //Cazador.steering.seekWeight = 1f;
    }


    public override void UpdateEstado()
    {
        //if (Vector3.Distance(Cazador.transform.position, Cazador.steering.Target.position) < 50f)
        //{
        //    GameManager.instancia.Quaffle.GetComponent<Quaffle>().Throw(Cazador.steering.Target.position - Cazador.transform.position, Cazador.ThrowStrength);
        //    GameManager.instancia.FreeQuaffle();

        //}

        //if (!GameManager.instancia.isQuaffleControlled())
        //{
        //    FSM.CambiarDeEstado(Cazador.PerseguirPelota);
        //}
    }

    public override void Exit()
    {
        //Cazador.steering.seek = false;
    }
}