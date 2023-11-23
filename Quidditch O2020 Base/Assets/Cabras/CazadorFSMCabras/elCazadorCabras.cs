using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CazadorAcciones;

public class elCazadorCabras : PecesPlayer
{
    [Header("Cazador")]
    public float ThrowStrength = 50;
    public float DistanceToShoot = 50;
    public float DistanciaMinQuaffle = 9;

    [Header("Posiciones escolta")]
    public Transform posicionUno;
    public Transform posicionDos;

    [HideInInspector] public int numeroParaSeguir;

    public string estadoActual;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //Agregar edos del agente
        //Buscar pelota, buscar portería (acción: lanzar la pelota)

        Prepararse preparar = new Prepararse(this);
        Esperar esperar = new Esperar(this);
        PerseguirPelota perseguirPelota = new PerseguirPelota(this);
        BuscarAro buscarAro = new BuscarAro(this);
        Acompaniar acompaniar = new Acompaniar(this);
        PerseguirRival rival = new PerseguirRival(this);

        fsm.AddState(CazadorStateID.Prepararse, preparar);
        fsm.AddState(CazadorStateID.Esperar, esperar);
        fsm.AddState(CazadorStateID.PerseguirPelota, perseguirPelota);
        fsm.AddState(CazadorStateID.BuscarAro, buscarAro);
        fsm.AddState(CazadorStateID.Acompaniar, acompaniar);
        fsm.AddState(CazadorStateID.PersiguirRival, rival);

        fsm.ChangeState(CazadorStateID.Prepararse);
        fsm.Activate();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
        //fsmCabras.Update();
    }
}
