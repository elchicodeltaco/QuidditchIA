using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyKeeperStates
{
    public enum KeeperStateID // Aqui agreguen las claves de cada estado que quieran
    {
        GoToPosition = 4,
        Persuit,
        Interpose,
        Block,
        Chase,
        Throw
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
            keeper.estadoActual = "Llendo a pos";
        }
        public override void Act(GameObject objeto)
        {
            if (keeper.keeperPosition != null)
            {
                // El jugador se pone en posición de inicio de juego
                player.steering.Seek(keeper.keeperPosition, 1);
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Lejos
                && GameManager.instancia.isQuaffleControlled())
            {
                    fsm.ChangeState(KeeperStateID.Persuit);
            }
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Cerca)
            {
                    fsm.ChangeState(KeeperStateID.Interpose);
            }
        }
        public override void OnExit(GameObject objeto)
        {
           // Debug.Log("exit IDLE");
        }

    }
    public class Persuit : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;

        // Variables del estado

        public Persuit(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("perseguir");
            keeper.estadoActual = "Persuit";
        }
        public override void Act(GameObject objeto)
        {
            //GameObject jugador = keeper.quaffle.GetComponent<Ball>().CurrentBallOwner();
            //if (jugador != null)
            //{
            //    player.steering.Persuit(jugador.transform, 1);
            //}
            //else keeper.GetComponent<Rigidbody>().velocity = Vector3.zero;
            GameObject jugador = keeper.quaffle.GetComponent<Ball>().CurrentBallOwner();
            if (jugador != null)
            {
                player.steering.Interpose(jugador.transform, keeper.NearestRingToQuaffle(), 0.1f);
            }
            if (!GameManager.instancia.isQuaffleControlled())
            {
                player.steering.Interpose(keeper.quaffle, keeper.NearestRingToQuaffle(), 0.1f);
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Cerca)
            {
                fsm.ChangeState(KeeperStateID.Interpose);
            }
            //regresa
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Lejos &&
                !GameManager.instancia.isQuaffleControlled())
            {
                fsm.ChangeState(KeeperStateID.GoToPosition);
            }
            if(keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.FueraDeRango)
            {
                fsm.ChangeState(KeeperStateID.GoToPosition);

            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Perseguir");
            //player.steering.pursuit = false;
        }

    }
    public class Interpose : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;
        // Variables del estado

        public Interpose(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("Anticipar");
            keeper.estadoActual = "Anticipando";

        }
        public override void Act(GameObject objeto)
        {
            //si un jugador la tiene
            GameObject jugador = keeper.quaffle.GetComponent<Ball>().CurrentBallOwner();
            if (jugador != null)
            {
                player.steering.Interpose(jugador.transform, keeper.NearestRingToQuaffle(), 0.1f);
            }
            if (!GameManager.instancia.isQuaffleControlled())
            {
                player.steering.Interpose(keeper.quaffle, keeper.NearestRingToQuaffle(), 0.1f);
            }

        }
        public override void Reason(GameObject objeto)
        {
            //regresa
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Lejos)
            {
                if(GameManager.instancia.isQuaffleControlled())
                fsm.ChangeState(KeeperStateID.Persuit);
                else
                fsm.ChangeState(KeeperStateID.GoToPosition);
            }
            
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Peligro)
            {
                if(GameManager.instancia.isQuaffleControlled())
                fsm.ChangeState(KeeperStateID.Block);
                else
                fsm.ChangeState(KeeperStateID.Chase);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Anticipar");
            //player.steering.interpose = false;
        }

    }
    public class Block : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;
        Vector3 quaffle;
        // Variables del estado

        public Block(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("cazar");
            keeper.estadoActual = "Bloquear";
            quaffle = keeper.BlockFunction();
        }
        public override void Act(GameObject objeto)
        {
            //player.steering.Seek(quaffle, 1);
            player.steering.Arrive(keeper.BlockFunction(), CabrasSteeringBlender.decelerationVel.fast,1);
        }
        public override void Reason(GameObject objeto)
        {
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Cerca)
            {
                fsm.ChangeState(KeeperStateID.Interpose);
            }
            if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Peligro
                && !GameManager.instancia.isQuaffleControlled())
            {
                ChangeState(KeeperStateID.Chase);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Cazar");
            //player.steering.seek = false;
        }

    }
    public class ChaseBall : State
    {
        PecesPlayer player;
        CabrasKeeper keeper;
        Transform quaffle;
        public ChaseBall(CabrasKeeper keeper)
        {
            this.keeper = keeper;
            this.player = keeper;
        }
        public override void OnEnter(GameObject obj)
        {
            keeper.estadoActual = "perseguirPelota";
            //Debug.Log("Perseguir Pelota");
            quaffle = GameManager.instancia.Quaffle.transform;
        }

        public override void Act(GameObject obj)
        {
            player.steering.Persuit(quaffle, 1);
        }
        public override void Reason(GameObject obj)
        {
            //si nadiw tiene ka quaffle
            if (!GameManager.instancia.isQuaffleControlled())
            {
                        Debug.Log("se la agarro");
                //Llego a cierta distancia de la pelota y la intento controlar
                if (Vector3.Distance(keeper.transform.position, quaffle.position)
                    < keeper.DistanciaMinQuaffle)
                {
                    //No está controlada y estoy cerca, puedo tomarla
                    if (GameManager.instancia.ControlQuaffle(keeper.gameObject))//esta es una accion y aqui la toma
                    {
                        //Ya que tengo la pelota busco otro jugador para pasarla
                        ChangeState(KeeperStateID.Throw);
                    }
                }
            }
            else
            {
                //Si la Quaffle fue tomada por otro jugador
                //Podemos chacar si la tiene un compañero
                if (keeper.miEquipo.isCompa(GameManager.instancia.Quaffle.
                    GetComponent<Quaffle>().CurrentBallOwner()))
                {
                    if(keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Peligro)
                    ChangeState(KeeperStateID.Block);

                }
            }
                if(keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Cerca)
                {
                    ChangeState(KeeperStateID.Interpose);
                }
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit perseguirPelota");
        }
    }
    public class Throw : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;
        private Transform nearestChaser;

        private GameObject aro = new GameObject();
        // Variables del estado

        public Throw(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("Anticipar");
            keeper.estadoActual = "Pasar Pelota";

            nearestChaser = keeper.NearestChaser();
        }
        public override void Act(GameObject objeto)
        {
            //if (GameManager.instancia.ControlQuaffle(keeper.gameObject))
            //{
                Transform aroCercano = keeper.NearestRingToQuaffle();

                Vector3 direccion = keeper.quaffle.position - aroCercano.position;
                keeper.quaffle.GetComponent<Quaffle>().
                        Throw(direccion, keeper.ThrowStrength);
                GameManager.instancia.FreeQuaffle();
           //}
        }
        public override void Reason(GameObject objeto)
        {
            if (!GameManager.instancia.ControlQuaffle(keeper.gameObject))
            {
                if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Peligro)
                {
                    if (GameManager.instancia.isQuaffleControlled())
                        fsm.ChangeState(KeeperStateID.Block);
                    else
                        fsm.ChangeState(KeeperStateID.Chase);
                }
                if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Cerca)
                {
                    ChangeState(KeeperStateID.Interpose);
                }
                //regresa
                if (keeper.currentAlertState == CabrasKeeper.EstadoDeAlerta.Lejos)
                {
                    if (GameManager.instancia.isQuaffleControlled())
                        fsm.ChangeState(KeeperStateID.Persuit);
                    else
                        fsm.ChangeState(KeeperStateID.GoToPosition);
                }
            }
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Anticipar");
            //player.steering.interpose = false;
        }

    }
    

    public class Wait : State
    {
        private PecesPlayer player;
        private CabrasKeeper keeper;
        private bool cambiar;
        private float tiempoParCambiar;
        // Variables del estado

        public Wait(CabrasKeeper _player)
        {
            player = _player;
            keeper = _player;
        }
        public override void OnEnter(GameObject objeto)
        {
            keeper.estadoActual = "esperar";
            tiempoParCambiar = 3;
        }
        public override void Act(GameObject objeto)
        {
            if (keeper.keeperPosition != null)
            {
                // El jugador se pone en posición de inicio de juego
                player.steering.Arrive(keeper.keeperPosition,
                    CabrasSteeringBlender.decelerationVel.fast, 1);
                tiempoParCambiar -= Time.deltaTime;
            }
        }
        public override void Reason(GameObject objeto)
        {
            if (tiempoParCambiar < 0)
            {
                ChangeState(KeeperStateID.GoToPosition);
            }
            if (player.miEquipo.isRival(GameManager.instancia.
                Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
                ChangeState(KeeperStateID.GoToPosition);
        }
        public override void OnExit(GameObject objeto)
        {
            //Debug.Log("exit Cazar");
            //player.steering.seek = false;
        }

    }
}
