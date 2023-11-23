using MyKeeperStates;
using PecesPlayerEstados;
using System.Collections.Generic;
using UnityEngine;

public class CabrasKeeper : PecesPlayer
{
    [Header("Keeper")]
    public float ThrowStrength = 50;
    public float DistanceToShoot = 50;
    public float DistanciaMinQuaffle = 9;
    public string estadoActual;
    [Header("Positions")]
    public float ringOffset;
    [HideInInspector] public float danger= 48;
    [HideInInspector] public float closenumber = 120;
    [HideInInspector] public float far = 160;
    [HideInInspector] public Vector3 keeperPosition;
    [Header("Fuzzy")]
    public float magnitudePercent;
    public float HowFarBall;
    public EstadoDeAlerta currentAlertState;
    public AnimationCurve toClose;
    public AnimationCurve close;
    public AnimationCurve antcipate;
    [Header("Variables")]
    public float checkRadius;
    public float throwStrength = 50;
    public float distanceToShoot = 10;
    public List<Transform> rings;
    private bool empezar = false;

    public enum EstadoDeAlerta
    {
        Peligro,
        Cerca,
        Lejos,
        FueraDeRango
    }
    //public AnimationCurve 
    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        rings = new List<Transform>();
        Invoke("SetTeamRings", 1);
        // Agregar los estados de este agente, chaser
        GoToPosition goToPos = new GoToPosition(this);
        Persuit persuit = new Persuit(this);
        Interpose interpose = new Interpose(this);
        Block seekTarget = new Block(this);
        ChaseBall perseguirBola = new ChaseBall(this);
        Throw pasarBola = new Throw(this);
        //estos son los ultimos en declararse pero los primeros en usarse
        EstadoGlobal global = new EstadoGlobal(this);
        Prepararse preparar = new Prepararse(this, goToPos);
        RecibirGolpe golpe = new RecibirGolpe(this);
        JuegoAcabado acabo = new JuegoAcabado(this);

        fsm.SetGlobalState(global);

        fsm.AddState(StateID.EstadoGlobal, global);
        fsm.AddState(StateID.Prepararse, preparar);
        fsm.AddState(StateID.RecibirGolpe, golpe);
        fsm.AddState(StateID.JuegoAcabado, acabo);
        fsm.AddState(KeeperStateID.GoToPosition, goToPos);
        fsm.AddState(KeeperStateID.Persuit, persuit);
        fsm.AddState(KeeperStateID.Interpose, interpose);
        fsm.AddState(KeeperStateID.Block, seekTarget);
        fsm.AddState(KeeperStateID.Chase, perseguirBola);
        fsm.AddState(KeeperStateID.Throw, pasarBola);

        fsm.ChangeState(StateID.Prepararse);
        fsm.Activate();


    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if (empezar)
        {
            HowCloseIsTheBall();
            if (HowFarBall > checkRadius)
            {
                currentAlertState = EstadoDeAlerta.FueraDeRango;
                return;
            }
            EvaluateDistanceFromQuaffle();
        }
    }

    public void EvaluateDistanceFromQuaffle()
    {
        AnimationCurve[] curvas = new AnimationCurve[3] { toClose, close, antcipate };
        int estadoDef = -1;
        float estado = -1;
        for (int i = 0; i < 3; i++)
        {
            float cantidad = curvas[i].Evaluate(HowFarBall);
            if (cantidad > estado)
            {
                estado = cantidad;
                estadoDef = i;
            }
        }
        switch (estadoDef)
        {
            case 0: this.currentAlertState = EstadoDeAlerta.Peligro; break;
            case 1: this.currentAlertState = EstadoDeAlerta.Cerca; break;
            case 2: this.currentAlertState = EstadoDeAlerta.Lejos; break;
            default: this.currentAlertState = EstadoDeAlerta.FueraDeRango; break;
        }

    }

    public Transform NearestRingToQuaffle()
    {
        //aqui se podria agregar logica difusa
        float magnitud = int.MaxValue;
        Transform aroCercano = null;
        //agarar la quaffle y compararla con el aro mas cercano
        foreach (Transform aro in rings)
        {
            Vector3 direccion = aro.position - quaffle.position;

            if (direccion.magnitude < magnitud)
            {
                magnitud = direccion.magnitude;
                aroCercano = aro;
            }
        }
        if (HowFarBall < 100f)
            Debug.DrawLine(quaffle.GetComponent<Ball>().transform.position, aroCercano.transform.position, Color.magenta);
        return aroCercano;

    }

    public Vector3 BlockFunction()
    {
        Transform aroCercano = NearestRingToQuaffle();

        //int suma = miEquipo.cabrasTeamNumber == 2 ? 1 : -1;
        int suma = quaffle.position.x >= aroCercano.position.x ? 1 : -1;
        Vector3 direccion = new Vector3(aroCercano.position.x + (ringOffset * suma),
            aroCercano.position.y, aroCercano.position.z);
        return direccion;
    }

    public void WhichRingIsGoingTo()
    {
        //Debug.DrawRay(quaffleBall.GetComponent<Ball>().CurrentBallOwner().transform.position, velEnemy * 10, Color.green);
        //RaycastHit hit;
        //Physics.SphereCast(quaffleBall.GetComponent<Ball>().CurrentBallOwner().transform.position, 80, velEnemy, out hit, velEnemy.magnitude * 100);

        //if (hit.transform == aro)
        //{
        //    magSobreCien += 100;
        //    print(aro.name);
        //}
        //magSobreCien /= 2;
    }

    public void HowCloseIsTheBall()
    {
        Vector3 distancia = quaffle.transform.position - NearestRingToQuaffle().position;
        float retornar = (distancia.magnitude * 100) / magnitudePercent;
        HowFarBall = retornar;
    }
    public Transform NearestChaser()
    {
        List<elCazadorCabras> cazadores = new List<elCazadorCabras>();
        foreach (Transform jugadores in miEquipo.cabras)
        {
            if (jugadores.TryGetComponent<elCazadorCabras>(out elCazadorCabras cheaser))
            {
                cazadores.Add(cheaser);
            }
        }
        float magnitud = int.MaxValue;
        Transform cazadorCercano = null;
        foreach (elCazadorCabras caz in cazadores)
        {
            Vector3 direccion = caz.transform.position - transform.position;

            if (direccion.magnitude < magnitud)
            {
                magnitud = direccion.magnitude;
                cazadorCercano = caz.transform;
            }
        }
        Debug.DrawLine(transform.position, cazadorCercano.position, Color.green);
        return cazadorCercano;
    }

    public Collider[] OverlapCheckSphere()
    {
        return Physics.OverlapSphere(keeperPosition, checkRadius);
    }

    private void SetTeamRings()
    {
        foreach (Transform aro in miEquipo.arosEquipo)
        {
            rings.Add(aro);
        }

        int suma = miEquipo.cabrasTeamNumber == 2 ? 10 : -10;
        Vector3 pos = new Vector3(rings[0].position.x + suma, rings[0].position.y - 10f,
            rings[0].position.z);
        keeperPosition = pos;

        Vector3 maxPosition = new Vector3(keeperPosition.x + checkRadius, keeperPosition.y, keeperPosition.z);
        Vector3 distance = maxPosition - keeperPosition;
        magnitudePercent = distance.magnitude;


        empezar = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(keeperPosition, checkRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(keeperPosition, far);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(keeperPosition, closenumber);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(keeperPosition, danger);
    }
}

public class RingToProtect
{
    public string name;
    public Transform transform;
    public float nearProbability;
    public float directionProbability;
    public RingToProtect(string name, Transform transform)
    {
        this.name = name;
        this.transform = transform;
    }
}