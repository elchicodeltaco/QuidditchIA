using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PecesPlayerEstados
{
    public enum StateID // Aqui agreguen las claves de cada estado que quieran
    {
        EstadoGlobal,
        Prepararse,
        RecibirGolpe,
        JuegoAcabado
    }

    public class EstadoGlobal : State
    {
        private PecesPlayer player;

        // Variables del estado

        public EstadoGlobal(PecesPlayer _player)
        {
            player = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
            // Si el jugador es golpeado
            if (player.hitted && !isCurrentState(StateID.RecibirGolpe))
            {
                InitBlipState(StateID.RecibirGolpe);
            }

            // Alguien acaba de anotar
            int scoringTeam = GameManager.instancia.IsRecovering();
            if (scoringTeam != 0)
            {

            }

            // Si el juego termina
            if (GameManager.instancia.isGameOver())
            {
                ChangeState(StateID.JuegoAcabado);
            }
        }
        public override void OnExit(GameObject objeto)
        {
        }
    }

    public class Prepararse : State
    {
        PecesPlayer player;
        State sigEstado;

        public Prepararse(PecesPlayer player, State sigEstado)
        {
            this.player = player;
            this.sigEstado = sigEstado;
        }
        public override void OnEnter(GameObject obj)
        {
        }

        public override void Act(GameObject obj)
        {
            if (player.posicionInicial != null)
                player.steering.Arrive(player.posicionInicial.position,
                    CabrasSteeringBlender.decelerationVel.fast, 1);
        }
        public override void Reason(GameObject obj)
        {
            if (GameManager.instancia.isGameStarted())
            {
                ChangeState(sigEstado);
            }
        }
        public override void OnExit(GameObject obj)
        {
        }
    }

    //=============================================================
    //=================================================== ReceiveHit
    public class RecibirGolpe : State
    {
        private PecesPlayer player;

        // Variables del estado
        bool stunEnds;
        bool loseControl;   // El jugador tenia la pelota, la puede perder

        public RecibirGolpe(PecesPlayer _player)
        {
            player = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            // En este estado el jugador recibe un golpe

            
            loseControl = false;
            stunEnds = false;
            fsm.myMono.StartCoroutine(StunFunction());
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
            if (stunEnds)
                RevertBlipState();
        }
        public override void OnExit(GameObject objeto)
        {
            // Ya sea que haya sido incapacitado o no, me aseguro de que su steering esté activo
            player.GetComponent<CabrasSteeringBlender>().enabled = true;

            player.hitted = false;
        }

        IEnumerator StunFunction()
        {
            
            // NO siempre que sea golpeado queda incapacitado o pierde el control de la pelota
            float lose = Random.value;
            if (lose > player.resistencia)
            {
                // Si tenia la pelota
                GameObject owner = GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner();
                if (owner != null)
                {
                    if (owner.Equals(player.gameObject))
                    {
                        Debug.Log("stun function");
                        // Pierde control de la pelota
                        GameManager.instancia.FreeQuaffle();
                    }
                }
                // Lo desbalancea un instante
                player.GetComponent<CabrasSteeringBlender>().enabled = false;

                yield return new WaitForSeconds(2f);
            }

            stunEnds = true;
        }
    }

    public class JuegoAcabado : State
    {
        private PecesPlayer player;

        // Variables del estado

        public JuegoAcabado(PecesPlayer _player)
        {
            player = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
        }
        public override void Act(GameObject objeto)
        {
            objeto.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
        public override void Reason(GameObject objeto)
        {

        }
        public override void OnExit(GameObject objeto)
        {
        }
    }
}
