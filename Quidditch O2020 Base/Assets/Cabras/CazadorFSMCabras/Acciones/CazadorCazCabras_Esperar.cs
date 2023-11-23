using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorCazCabras_Esperar : FSMStatesCazCabras
{
    private elCazadorCabras Cazador;
    private bool espere = false;
    private Coroutine coroutine;
    public CazadorCazCabras_Esperar(FSMCazCabras fsm, Animator animator, elCazadorCabras Cazador) : base(fsm, animator)
    {
        this.Cazador = Cazador; 
    }



    public override void Enter()
    {
        base.Enter();

        espere = false;
        coroutine = FSM.Mono.StartCoroutine(CorutinaEspera());
    }


    public override void UpdateEstado()
    {
        //Estoy tras la pelota, hay que ver si la tiene otro jugador
        if (espere && GameManager.instancia.isGameStarted() && GameManager.instancia.IsRecovering() == 0)
        {
            //FSM.CambiarDeEstado(Cazador.PerseguirPelota);
        }

        else
        {
            if(espere == false)
            {
                coroutine = FSM.Mono.StartCoroutine(CorutinaEspera());

            }
        }
    }

    private IEnumerator CorutinaEspera()
    {
        yield return new WaitForSeconds(2f);
        espere = true;
    }
    public override void Exit()
    {
        espere = true;
        FSM.Mono.StopCoroutine(CorutinaEspera());
    }
}