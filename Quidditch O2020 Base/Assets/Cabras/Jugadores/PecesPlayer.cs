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

    protected virtual void OnCollisionEnter(Collision collision)
    {
        // Si me pega un rival o una pelota
        if (miEquipo.isRival(collision.gameObject) || collision.gameObject.tag.Equals("Ball Bludger"))
        {
            // Me pegaron con suficiente fuerza
            if (collision.relativeVelocity.magnitude > 2) //calibrar
            {
                print("PEGO" + name.ToString());
                hitted = true;
            }
        }
    }
}
