using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CazadorCazCabras_PerseguirBola : FSMStatesCazCabras
{
    private elCazadorCabras Cazador;

    public CazadorCazCabras_PerseguirBola(FSMCazCabras fsm, Animator animator, elCazadorCabras Cazador) : base(fsm, animator)
    {
        this.Cazador = Cazador;
    }



    public override void Enter()
    {
        base.Enter();

        //Buscamos la Quaffle
        //Cazador.steering.Target = GameManager.instancia.Quaffle.transform;

        //Cazador.steering.pursuit = true; //Quiero que llegue en chinga
        //Cazador.steering.pursuitWeight = 1f;
    }

    public override void UpdateEstado()
    {
        //if (GameManager.instancia.IsRecovering() != 0)
        //{
        //    //Esperar ante de tratar de ir por la pelota
        //    FSM.CambiarDeEstado(Cazador.EstadoEsperar);
        //    return; //Cada llamada de edo
        //}

        ////Estoy tras la pelota, hay que ver si la tiene otro jugador
        //if (!GameManager.instancia.isQuaffleControlled()) //!
        //{
        //    //Llego a cierta distancia de la pelota y la intento controlar
        //    if (Vector3.Distance(Cazador.transform.position, Cazador.steering.Target.position) < 8f)
        //    {
        //        //No está controlada y estoy cerca, puedo tomarla
        //        if (GameManager.instancia.ControlQuaffle(Cazador.gameObject))
        //        {
        //            Cazador.GetComponentInParent<LosCazadoresCabras>().TenemosLaPelota(Cazador.gameObject);
        //            //Ya que tengo la pelota busco anotar
        //            FSM.CambiarDeEstado(Cazador.BuscarAro);
        //        }
        //    }
        //}
        //else
        //{
        //    //Si la Quaffle fue tomada por otro jugador
        //    //Podemos chacar si la tiene un compañero
        //    if (Cazador.miEquipo.esCompa(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
        //    {
        //        //Acompañar al jugador
        //        FSM.CambiarDeEstado(Cazador.Acompaniar);

        //    }
        //    else if(!Cazador.miEquipo.esCompa(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))//Pero si la tiene un rival
        //    {
        //        //Ir tras el rival
        //        FSM.CambiarDeEstado(Cazador.SeguirARival);
        //    }
        //}
    }

    public override void Exit()
    {
        //Cazador.steering.pursuit = false;
    }
}