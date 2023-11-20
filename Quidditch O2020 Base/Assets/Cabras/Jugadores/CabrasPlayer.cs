using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerStates;

[RequireComponent(typeof(CabrasSteeringBlender))]

public class CabrasPlayer : MonoBehaviour
{
    public CabrasSteeringBlender steering;
    FSM fsm;
    public float resistance = 50;
    public Transform myStartingPos = null;

    public PlayerPosition playerPosition;
    public enum PlayerPosition
    {
        Keeper, Beater, Chaser, Seeker
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        steering = GetComponent<CabrasSteeringBlender>();
        fsm = new FSM(gameObject, this);

        // Crear los estados en que puede estar 
        //GlobalMessageState global = new GlobalMessageState(this);
        //Waiting wait = new Waiting(this);
        //ReceiveHit hit = new ReceiveHit(this);

        // Estado global
        //fsm.SetGlobalState(global);
        // Hay que agregarlos a la FSM
        //fsm.AddState(StateID.Waiting, wait);
        //fsm.AddState(StateID.ReceiveHit, hit);

        // Indicar cual es el estado inicial
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
        }
    }
}
