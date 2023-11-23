using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadirCazCabras_Acompaniar : FSMStatesCazCabras
{
    private elCazadorCabras Cazador;

    public CazadirCazCabras_Acompaniar(FSMCazCabras fsm, Animator animator, elCazadorCabras Cazador) : base(fsm, animator)
    {
        this.Cazador = Cazador;
    }



    public override void Enter()
    {
        base.Enter();


        //Buscamos la Quaffle

        if(Cazador.numeroParaSeguir == 1)
        {
           // Cazador.steering.Target = Cazador.GetComponentInParent<LosCazadoresCabras>().quienTieneLaPelota.GetComponent<elCazadorCabras>().posicionUno;

        }
        else if(Cazador.numeroParaSeguir == 2)
        {
            //Cazador.steering.Target = Cazador.GetComponentInParent<LosCazadoresCabras>().quienTieneLaPelota.GetComponent<elCazadorCabras>().posicionDos;

        }

        //Cazador.steering.arrive = true; //Quiero que llegue en chinga
        //Cazador.steering.seekWeight = 1f;
    }

    public override void UpdateEstado()
    {
        //Estoy tras la pelota, hay que ver si la tiene otro jugador
        if (!GameManager.instancia.isQuaffleControlled())
        {
            //FSM.CambiarDeEstado(Cazador.PerseguirPelota);
        }

    }

    public override void Exit()
    {
        //Cazador.steering.arrive = false;
    }
}
