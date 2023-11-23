using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PecesPlayerEstados
{
    public enum StateID // Aqui agreguen las claves de cada estado que quieran
    {
        Prepare,
        ReceiveHit
    }

    public class Prepararse : State
    {
        PecesPlayer player;

        public Prepararse(PecesPlayer player)
        {
            this.player = player;
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
                //hangeState();
            }
        }
        public override void OnExit(GameObject obj)
        {
        }
    }

    //=============================================================
    //=================================================== ReceiveHit
    public class ReceiveHit : State
    {
        private PecesPlayer player;

        // Variables del estado
        bool stunEnds;
        bool loseControl;   // El jugador tenia la pelota, la puede perder

        public ReceiveHit(PecesPlayer _player)
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
}
