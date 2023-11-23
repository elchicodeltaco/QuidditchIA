using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PecesPlayerEstados;
using BeaterGlittersStates;

public class CabrasBeater : PecesPlayer
{
    public string estado;
    public float ThrowStrength;
    public float distanceToShoot;
    public int beaterNumber;
    public GameObject bludgerEnPosesion;



    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //Lista de estados del Beater
        BeaterChaseBludger chase = new BeaterChaseBludger(this);
        BeaterThrowBludger throwB = new BeaterThrowBludger(this);
        BeaterEscortPlayer escort = new BeaterEscortPlayer(this);
        BeaterEsperar esperar = new BeaterEsperar(this);

        Prepararse prepare = new Prepararse(this, chase);

        //Inicializar los estados
        fsm.AddState(StateID.Prepararse, prepare);
        fsm.AddState(BeaterStatesID.BeaterChaseBludger, chase);
        fsm.AddState(BeaterStatesID.BeaterThrowBludger, throwB);
        fsm.AddState(BeaterStatesID.BeaterEscortPlayer, escort);
        fsm.AddState(BeaterStatesID.BeaterEsperar, esperar);



        //Indicar a la fsm en que estado inicia
        fsm.ChangeState(StateID.Prepararse);
        fsm.Activate();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
