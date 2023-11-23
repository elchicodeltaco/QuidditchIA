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
        Persuit,
        Seek
    }

    public class PrepareToPlay : State
    {
        private CabrasKeeper player;

        // Variables del estado

        public PrepareToPlay(CabrasKeeper _player)
        {
            player = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("Llendo a posicion");
        }
        public override void Act(GameObject objeto)
        {
            if (player.posicionInicial != null)
            {
                // El jugador se pone en posición de inicio de juego
                player.steeringCabras.Arrive(player.posicionInicial.position, CabrasSteeringBlender.decelerationVel.fast, 1);
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
            //Debug.Log("Llegue a pos");
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }
    public class GoToPosition : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;

        // Variables del estado

        public GoToPosition(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("entrendo idle");
        }
        public override void Act(GameObject objeto)
        {
            if (keeper.keeperPosition != null)
            {
                // El jugador se pone en posición de inicio de juego
                keeper.steeringCabras.Arrive(player.posicionInicial.position, 
                    CabrasSteeringBlender.decelerationVel.fast, 1);
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Predecir)
                fsm.ChangeState(KeeperStateID.Anticipate);
                    
        }
        public override void OnExit(GameObject objeto)
        {
           // Debug.Log("exit IDLE");
            //player.steering.arrive = false;
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }
    public class Anticipate : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;

        private GameObject aro = new GameObject();
        // Variables del estado

        public Anticipate(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("Anticipar");

            
        }
        public override void Act(GameObject objeto)
        {
            if (keeper.quaffle.GetComponent<Ball>().CurrentBallOwner() != null)
            {
                //preguntar si es rival
                keeper.steeringCabras.Interpose(keeper.quaffle.GetComponent<Ball>().
                CurrentBallOwner().transform, keeper.NearestRingToQuaffle(), 0.1f);
                keeper.steeringCabras.maxSpeed = 15;
            }
            else
            {
                keeper.steeringCabras.Interpose(keeper.quaffle, keeper.NearestRingToQuaffle(), 0.1f);
                keeper.steeringCabras.maxSpeed = 15;
            }
            
            
        }
        public override void Reason(GameObject objeto)
        {
            //if(keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Anticipar)
            //{
                //fsm.ChangeState(KeeperStateID.Persuit);
            //}
            if(keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Perseguir)
            {
                fsm.ChangeState(KeeperStateID.Seek);
            }
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.FueraDeRango)
                fsm.ChangeState(KeeperStateID.GoToPosition);
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Anticipar");
            //player.steering.interpose = false;
        }

    }
    public class PersuitTarget : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;

        // Variables del estado

        public PersuitTarget(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("perseguir");
        }
        public override void Act(GameObject objeto)
        {
            if(keeper.quaffle.GetComponent<Ball>().CurrentBallOwner() != null) 
            {
                //preguntar si es rival
                keeper.steeringCabras.Persuit(keeper.quaffle.GetComponent<Ball>().CurrentBallOwner().transform, 1);
            }
            else
            {
                //player.steering.Target = keeper.quaffleBall;
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Predecir)
            {
                fsm.ChangeState(KeeperStateID.Anticipate);
            }
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Perseguir)
            {
                fsm.ChangeState(KeeperStateID.Seek);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Perseguir");
            //player.steering.pursuit = false;
        }

    }
    public class SeekTarget : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;

        // Variables del estado

        public SeekTarget(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("cazar");
        }
        public override void Act(GameObject objeto)
        {
            if(keeper.quaffle.GetComponent<Ball>().CurrentBallOwner() != null) 
            {
                //preguntar si es rival
               // keeper.steeringCabras.Seek(keeper.quaffleBall.GetComponent<Ball>()
                    //.CurrentBallOwner().transform.position, 1);
                keeper.steeringCabras.maxSpeed = 30;

                keeper.steeringCabras.Persuit(keeper.quaffle.GetComponent<Ball>().
                    CurrentBallOwner().transform, 1);
            }
            else
            {
                keeper.steeringCabras.Seek(keeper.quaffle.position, 1);
            }
            //player.steering.seek = true;
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Predecir)
            {
                fsm.ChangeState(KeeperStateID.Anticipate);
            }
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Anticipar)
            {
                fsm.ChangeState(KeeperStateID.Persuit);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Cazar");
            //player.steering.seek = false;
        }

    }
}
