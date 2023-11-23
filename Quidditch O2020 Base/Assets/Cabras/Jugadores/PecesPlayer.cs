using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PecesPlayerEstados;

[RequireComponent(typeof(CabrasSteeringBlender))]
public class PecesPlayer : MonoBehaviour
{

    [HideInInspector] public CabrasSteeringBlender steering;
    public bool hitted = false;
    public Transform quaffle;
    // team stuff
    [HideInInspector] public CabrasTeam miEquipo;
    public Transform posicionInicial = null;
    [HideInInspector] public int numeroEnElEquipo;
    //fsm
    public FSM fsm;

    [Header("Padre")]
    public PlayerPosition playerPosition;
    public enum PlayerPosition
    {
        Keeper, Beater, Chaser, Seeker
    }
    public float resistencia = 50;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        steering = GetComponent<CabrasSteeringBlender>();
        fsm = new FSM(gameObject, this);

        //team stuff
        miEquipo = GetComponentInParent<CabrasTeam>();

        quaffle = GameManager.instancia.Quaffle.transform;
        //// Crear los estados en que puede estar 
        //GlobalMessageState global = new GlobalMessageState(this);
        //Waiting wait = new Waiting(this);
        //ReceiveHit hit = new ReceiveHit(this);

        //// Estado global
        //fsm.SetGlobalState(global);
        ////Hay que agregarlos a la FSM
        //fsm.AddState(StateID.Waiting, wait);
        //fsm.AddState(StateID.ReceiveHit, hit);

        //// Indicar cual es el estado inicial
        //fsm.ChangeState(StateID.Waiting);

        // Activo la fsm
        //fsm.Activate();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (fsm != null && fsm.IsActive())
        {
            fsm.UpdateFSM();
            //print("andando"+ gameObject.ToString());
        }
    }
}
