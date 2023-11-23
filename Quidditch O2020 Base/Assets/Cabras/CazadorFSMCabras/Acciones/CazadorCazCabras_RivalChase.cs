using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorCazCabras_RivalChase : FSMStatesCazCabras
{
    private elCazadorCabras Cazador;

    public CazadorCazCabras_RivalChase(FSMCazCabras fsm, Animator animator, elCazadorCabras Cazador) : base(fsm, animator)
    {
        this.Cazador = Cazador; 
    }



    public override void Enter()
    {
        base.Enter();

        //Buscamos la Quaffle
        //Cazador.steering.Target = GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner().transform;

        //Cazador.steering.pursuit = true;
        //Cazador.steering.pursuitWeight = 1f;
    }


    public override void UpdateEstado()
    {
        //Si la Quaffle ya no tiene dueño
        if (!GameManager.instancia.isQuaffleControlled())
        {
           // FSM.CambiarDeEstado(Cazador.PerseguirPelota);        
        }
    }

    public override void Exit()
    {
       // Cazador.steering.pursuit = false;
    }
}