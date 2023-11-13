using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyKeeperStates
{
    public enum KeeperStateID // Aqui agreguen las claves de cada estado que quieran
    {
        PrepareToPlay,
        GoToPosition,
        Anticipate,
        Attack,
        SeekBall,
        PassBall
    }

    public class PrepareToPlay : State
    {
        private Player player;

        // Variables del estado

        public PrepareToPlay(Player _player)
        {
            player = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            Debug.Log("Llendo a posicion");
        }
        public override void Act(GameObject objeto)
        {
            if (player.myStartingPosition != null)
            {
                // El jugador se pone en posición de inicio de juego
                player.steering.Target = player.myStartingPosition;
                player.steering.arrive = true;
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (GameManager.instancia.isGameStarted())
            {
                ChangeState(KeeperStateID.GoToPosition);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            player.steering.arrive = false;
            Debug.Log("Llegue a pos");
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }
    public class GoToPosition : State
    {
        private Player player;
        private MyKeeper keeper;

        // Variables del estado

        public GoToPosition(MyKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            Debug.Log("entrendo idle");
        }
        public override void Act(GameObject objeto)
        {
            if (keeper.keeperPosition != null)
            {
                // El jugador se pone en posición de inicio de juego
                player.steering.Target = keeper.keeperPosition;
                player.steering.arrive = true;
            }
        }
        public override void Reason(GameObject objeto)
        {
            Collider[] nearColl = keeper.OverlapCheckSphere();
            foreach (Collider col in nearColl)
            {
                if (col.CompareTag("Ball Quaffle"))
                {
                    if (keeper.quaffleBall.GetComponent<Ball>().CurrentBallOwner() != null)
                    {
                        fsm.ChangeState(KeeperStateID.Anticipate);

                    }
                    else
                    {
                        //si la pelota no tiene dueño
                    }
                }
            }
        }
        public override void OnExit(GameObject objeto)
        {
            Debug.Log("exit IDLE");
            player.steering.arrive = false;
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }
    public class Anticipate : State
    {
        private Player player;
        private MyKeeper keeper;

        // Variables del estado

        public Anticipate(MyKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            Debug.Log("Anticipar");
            //player.steering.interpose = true;
        }
        public override void Act(GameObject objeto)
        {
            keeper.NearestRingToQuaffle();
            keeper.HowCloseIsTheBall();
            keeper.EvaluateDistanceFromQueffle();

        }
        public override void Reason(GameObject objeto)
        {
            
        }
        public override void OnExit(GameObject objeto)
        {
            Debug.Log("exit Anticipar");
            player.steering.interpose = false;
        }

        private void Interpose(GameObject enemy, GameObject ring)
        {
            player.steering.agent1 = enemy;
            player.steering.agent2 = ring;

            player.steering.interpose = true;
        }
    }
}
