using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;
using BludgerStates;


namespace BeaterGlittersStates
{
    public enum BeaterStatesID
    {
        BeaterChaseBludger,
        BeaterThrowBludger, //Primero buscamos objetivo de lanzamiento y luego lanzamos
        BeaterEscortPlayer,
        BeaterEsperar
    }

    //=============================================================
    //================== BEATER CHASE BLUDGER =====================
    //=============================================================

    public class BeaterChaseBludger: State
    {
        public PecesPlayer player;       
        public CabrasBeater beater;       

        public GameObject[] bludgers;
 
        public Transform objetivoBludger;
        GameObject bludgerMasCercana = null;

        public BeaterChaseBludger(CabrasBeater beater)
        {
            this.player = beater;
            this.beater = beater;
        }

        public override void OnEnter(GameObject _object)
        {
            beater.estado = "ChaseBludger";
        }
        public override void Act(GameObject _object)
        {
            //ebug.Log("Buscando bludger");

            float distanciaMenorBludger = float.MaxValue;


            foreach (GameObject bludger in GameManager.instancia.Bludger)
            {
                if (bludger != null)
                {
                   // Debug.Log("Entro");
                    Transform transformBludger = bludger.transform;
                    Vector3 posicionBludger = transformBludger.position;


                    //Debug.Log("Encontré bludger: " + bludger.name + "su posicion es: " + posicionBludger);

                    if (bludgerMasCercana == null)
                    {
                        bludgerMasCercana = bludger;
                        distanciaMenorBludger = Vector3.Distance(posicionBludger, beater.transform.position);
                    }
                    else
                    {
                        float distancia = Vector3.Distance(posicionBludger, beater.transform.position);
                        if (distancia < distanciaMenorBludger)
                        {
                            distanciaMenorBludger = distancia;
                            bludgerMasCercana = bludger;

                        }
                    }
                    //Debug.Log("La bludger más cercana es: " + bludgerMasCercana.name + distanciaMenorBludger);
                    //  float distanciaBludger = Vector3.Distance(bludger.transform.position, beater.transform.position);

                    //beater.steering.Target = bludgerMasCercana.transform;
                    //beater.steering.pursuit = true;
                    //beater.steering.pursuitWeight = 1;
                    Transform target = bludger.GetComponent<Ball>().steering.Target;
                    if (target == null) return;
                    player.steering.Persuit(target, 1);

                    beater.bludgerEnPosesion = bludgerMasCercana;

                    //Debug.Log("Persiguiendo" + bludger.name);

                }
                else
                {
                    //Debug.Log("No encontré bludgers");
                    ChangeState(BeaterStatesID.BeaterEscortPlayer);
                }
               
            }
        }
        public override void Reason(GameObject _object)
        {
            if(Vector3.Distance(bludgerMasCercana.transform.position, beater.transform.position) < 7f)
            {
                ChangeState(BeaterStatesID.BeaterThrowBludger);
            }
            if (GameManager.instancia.IsRecovering() != 0)
                ChangeState(BeaterStatesID.BeaterEsperar);
        }
        public override void OnExit(GameObject _object)
        {          
        }
        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }

    }


    //=============================================================
    //================== BEATER THROW BLUDGER =====================
    //=============================================================

    public class BeaterThrowBludger: State
    {
        public CabrasBeater beater;
        public PecesPlayer player;

        public Transform[] contrarios;
        Transform objetivoMasCercano;
        public BeaterThrowBludger(CabrasBeater beater)
        {
            this.player = beater;
            this.beater = beater;
        }

        public override void OnEnter(GameObject _object)
        {
            float distanciaMasCercana = float.MaxValue;

            for (int i = 0; i < 7; i++)
            {
                contrarios[i] = beater.miEquipo.rivales[i];
            }
            
            foreach(Transform t in contrarios)
            {
                if(Vector3.Distance(t.transform.position, beater.transform.position) < distanciaMasCercana)
                {
                    distanciaMasCercana = (Vector3.Distance(t.transform.position, beater.transform.position));
                    objetivoMasCercano = t;
                }
            }
            beater.estado = "Lanzar bludg";


        }
        public override void Act(GameObject _object)
        {
            //objeto mas cercano llega vacio
            if (objetivoMasCercano == null) Debug.Log("no hay bludger");
            beater.bludgerEnPosesion.GetComponent<Bludger>().BeaterIntervention(objetivoMasCercano.gameObject);
            beater.bludgerEnPosesion = null;
        }
        public override void Reason(GameObject _object)
        {
        }
        public override void OnExit(GameObject _object)
        {
        }
        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }


    //=============================================================
    //================== BEATER ESCORT PLAYER =====================
    //=============================================================


    //Posiblemente no se necesite esta función, debido a que las pelotas siempre están en movimiento.
    //Podríamos necesitarlo, pero llevo 32 horas sin dormir y queda poco tiempo. Auxilio
    public class BeaterEscortPlayer: State
    {
        public CabrasBeater beater;
        public PecesPlayer player;

        public GameObject compañeroACuidar;

        public BeaterEscortPlayer(CabrasBeater beater)
        {
            this.beater = beater;
            player = beater;
        }
        public override void OnEnter(GameObject _object)
        {
            beater.estado = "Beater escolta";
            compañeroACuidar = beater.miEquipo.cabras[Random.Range(0, 6)].gameObject;
            Debug.Log("Buscamos un compañero");

        }
        public override void Act(GameObject _object)
        {

               
            Debug.Log("Compañero a cuidar: " + compañeroACuidar.name);

            if(compañeroACuidar != null)
            {

                beater.steering.Arrive(compañeroACuidar.transform.position, CabrasSteeringBlender.decelerationVel.mid, 1f);
                beater.steering.Flee(compañeroACuidar.transform.position, 1f);
                //meter offset pursuit para que el beater siga a su compañero de cerca.

                float distanciaACompañero = Vector3.Distance(compañeroACuidar.transform.position, beater.transform.position);

                if (Mathf.Abs(distanciaACompañero) < 10 )
                {
                    beater.steering.Flee(compañeroACuidar.transform.position, 1f);
                }
                else
                {
                    beater.steering.Flee(compañeroACuidar.transform.position, 0.01f);
                }
            }
            else
            {
                compañeroACuidar = beater.miEquipo.cabras[Random.Range(0, 6)].gameObject;
                Debug.Log("Buscamos un compañero");
              
            }

            if(compañeroACuidar.name == ("Golpeador01"))
            {
                compañeroACuidar = beater.miEquipo.cabras[Random.Range(0, 6)].gameObject;
            }


        }
        public override void Reason(GameObject _object)
        {
        }
        public override void OnExit(GameObject _object)
        {
        }
        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }

    //=============================================================
    //================== BeaterWait ===============================
    //=============================================================

    public class BeaterEsperar : State
    {
        PecesPlayer player;
        CabrasBeater beater;

        public BeaterEsperar(CabrasBeater beater)
        {
            this.player = beater;
        }
        public override void OnEnter(GameObject obj)
        {
            Debug.Log("Esperando caz");
            beater.estado = "Beater escolta";
        }

        public override void Act(GameObject obj)
        {

        }
        public override void Reason(GameObject obj)
        {
            if (GameManager.instancia.IsRecovering() == 0)
            {
                ChangeState(BeaterStatesID.BeaterChaseBludger);
            }
        }
        public override void OnExit(GameObject obj)
        {
            Debug.Log("exit esperandoCaz");
        }
    }


}