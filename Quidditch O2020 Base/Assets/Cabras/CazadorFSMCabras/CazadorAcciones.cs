using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CazadorAcciones
{
    public enum CazadorStateID
    {
        Esperar = 4,
        PerseguirPelota,
        BuscarAro,
        Acompaniar,
        PersiguirRival
    }
    public class Esperar : State
    {
        PecesPlayer player;
        elCazadorCabras cazador;

        public Esperar(elCazadorCabras cazador)
        {
            this.cazador = cazador;
            this.player = cazador;
        }
        public override void OnEnter(GameObject obj)
        {
            //Debug.Log("Esperando caz");
            cazador.estadoActual = "esperar";
        }

        public override void Act(GameObject obj)
        {
            
        }
        public override void Reason(GameObject obj)
        {
            if (GameManager.instancia.IsRecovering() == 0)
            {
                ChangeState(CazadorStateID.PerseguirPelota);
            }
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit esperandoCaz");
        }
    }
    public class PerseguirPelota : State
    {
        PecesPlayer player;
        elCazadorCabras cazador;
        Transform quaffle;
        public PerseguirPelota(elCazadorCabras cazador)
        {
            this.cazador = cazador;
            this.player = cazador;
        }
        public override void OnEnter(GameObject obj)
        {
            cazador.estadoActual = "perseguirPelota";
            //Debug.Log("Perseguir Pelota");
            quaffle = GameManager.instancia.Quaffle.transform;
        }

        public override void Act(GameObject obj)
        {
            player.steering.Persuit(quaffle, 1);
        }
        public override void Reason(GameObject obj)
        {
            //ya anoto
            if (GameManager.instancia.IsRecovering() != 0)
                ChangeState(CazadorStateID.Esperar);

            //si nadiw tiene ka quaffle
            if (!GameManager.instancia.isQuaffleControlled())
            {
                //Llego a cierta distancia de la pelota y la intento controlar
                if (Vector3.Distance(cazador.transform.position, quaffle.position) 
                    < cazador.DistanciaMinQuaffle)
                {
                    //No está controlada y estoy cerca, puedo tomarla
                    if (GameManager.instancia.ControlQuaffle(cazador.gameObject))//esta es una accion y aqui la toma
                    {
                        cazador.GetComponentInParent<LosCazadoresCabras>().
                            TenemosLaPelota(cazador.gameObject);
                        //Ya que tengo la pelota busco anotar
                        ChangeState(CazadorStateID.BuscarAro);
                    }
                }
            }
            else
            {
                //Si la Quaffle fue tomada por otro jugador
                //Podemos chacar si la tiene un compañero
                GameObject companiero = quaffle.GetComponent<Quaffle>().CurrentBallOwner();
                if (cazador.miEquipo.isCompa(companiero) && companiero.GetComponent<PecesPlayer>().playerPosition == PecesPlayer.PlayerPosition.Chaser)
                {
                    //Acompañar al jugador
                    ChangeState(CazadorStateID.Acompaniar);
                }
                else ChangeState(CazadorStateID.PerseguirPelota);
                if (cazador.miEquipo.isRival(GameManager.instancia.Quaffle.
                    GetComponent<Quaffle>().CurrentBallOwner()))//Pero si la tiene un rival
                {
                    //Ir tras el rival
                    ChangeState(CazadorStateID.PersiguirRival);
                }
            }
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit perseguirPelota");
        }
    }
    public class BuscarAro : State
    {
        PecesPlayer player;
        elCazadorCabras cazador;
        Transform objetivoMasCercano;

        public BuscarAro(elCazadorCabras cazador)
        {
            cazador.estadoActual = "buscar aro";
            this.cazador = cazador;
            this.player = cazador;
        }
        public override void OnEnter(GameObject obj)
        {
            //Debug.Log("LLendo al aro");
            List<Transform> objetivos = cazador.GetComponentInParent<CabrasTeam>().arosEnemigos;
            float distanciaMenor = 0f;

            objetivoMasCercano = null;
            float distanciaMinima = float.MaxValue;


            foreach (Transform t in objetivos)
            {
                if (distanciaMinima > Vector3.Distance(t.transform.position, cazador.transform.position))
                {
                    distanciaMinima = Vector3.Distance(t.transform.position, cazador.transform.position);
                    objetivoMasCercano = t;
                }
            }
        }

        public override void Act(GameObject obj)
        {
            player.steering.Seek(objetivoMasCercano.position, 1);

            // esta cerca del aro
            if (Vector3.Distance(cazador.transform.position, objetivoMasCercano.position) 
                < cazador.DistanceToShoot)
            {
                GameManager.instancia.Quaffle.GetComponent<Quaffle>().
                    Throw(objetivoMasCercano.position - cazador.transform.position, cazador.ThrowStrength);
                GameManager.instancia.FreeQuaffle();

            }
        }
        public override void Reason(GameObject obj)
        {
            //ya anoto
            if (GameManager.instancia.IsRecovering() != 0)
                ChangeState(CazadorStateID.Esperar);

            //cuando la pelota esta libre
            if (!GameManager.instancia.isQuaffleControlled())
                ChangeState(CazadorStateID.PerseguirPelota);

            //si el rival tiene la pelota
            if(player.miEquipo.isRival(GameManager.instancia.
                Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
                ChangeState(CazadorStateID.PersiguirRival);
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit buscar aro");
        }
    }
    public class Acompaniar : State
    {
        PecesPlayer player;
        elCazadorCabras cazador;
        Transform direccion;
        Transform quaffle;

        public Acompaniar(elCazadorCabras cazador)
        {
            this.cazador = cazador;
            this.player = cazador;
        }
        public override void OnEnter(GameObject obj)
        {
            cazador.estadoActual = "Acompañar";
            quaffle = GameManager.instancia.Quaffle.transform;
            //Debug.Log("Acompañando");
            if (cazador.numeroParaSeguir == 1)
            {
                direccion = cazador.GetComponentInParent<LosCazadoresCabras>().
                    quienTieneLaPelota.GetComponent<elCazadorCabras>().posicionUno;
            }
            else if (cazador.numeroParaSeguir == 2)
            {
                direccion = cazador.GetComponentInParent<LosCazadoresCabras>().
                    quienTieneLaPelota.GetComponent<elCazadorCabras>().posicionDos;
            }
        }
        public override void Act(GameObject obj)
        {
            GameObject esPortero = quaffle.GetComponent<Quaffle>().CurrentBallOwner();
            if (esPortero != null)
            if (esPortero.GetComponent<PecesPlayer>().playerPosition == PecesPlayer.PlayerPosition.Keeper)
            {
                Debug.Log("Presta Portero");
                player.steering.Arrive(esPortero.transform.position, CabrasSteeringBlender.decelerationVel.slow, 0.2f);
            }
            else
            player.steering.Arrive(direccion.position, CabrasSteeringBlender.decelerationVel.fast, 1);
        }
        public override void Reason(GameObject obj)
        {
            GameObject esPortero = quaffle.GetComponent<Quaffle>().CurrentBallOwner();
            if (esPortero != null)
                if (esPortero.GetComponent<PecesPlayer>().playerPosition == PecesPlayer.PlayerPosition.Keeper)
            {
                ChangeState(CazadorStateID.PerseguirPelota);
            }
            //Estoy tras la pelota, hay que ver si la tiene otro jugador
            if (!GameManager.instancia.isQuaffleControlled())
            {
                ChangeState(CazadorStateID.PerseguirPelota);
            }
            //ya anoto
            if (GameManager.instancia.IsRecovering() != 0)
                ChangeState(CazadorStateID.Esperar);
            //si el rival tiene la pelota
            if (player.miEquipo.isRival(GameManager.instancia.
                Quaffle.GetComponent<Quaffle>().CurrentBallOwner()))
                ChangeState(CazadorStateID.PersiguirRival);
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit acompañar");
        }
    }
    public class PerseguirRival : State
    {
        PecesPlayer player;
        elCazadorCabras cazador;
        Transform rival;

        public PerseguirRival(elCazadorCabras cazador)
        {
            this.cazador = cazador;
            this.player = cazador;
        }
        public override void OnEnter(GameObject obj)
        {
            cazador.estadoActual = "perseguir rival";
            rival = player.quaffle.GetComponent<Quaffle>()
                .CurrentBallOwner().transform;
           //Debug.Log("Persiguiendo al rival");
        }

        public override void Act(GameObject obj)
        {
            player.steering.Persuit(rival, 1);
            //hay que hacer que le pegue al rival
        }
        public override void Reason(GameObject obj)
        {
            if (GameManager.instancia.IsRecovering() != 0)
            {
                ChangeState(CazadorStateID.Esperar);
            }
            //Si la Quaffle ya no tiene dueño
            if (!GameManager.instancia.isQuaffleControlled())
            {
               ChangeState(CazadorStateID.PerseguirPelota);        
            }
        }
        public override void OnExit(GameObject obj)
        {
            //Debug.Log("exit perseguir rival");
        }
    }
}