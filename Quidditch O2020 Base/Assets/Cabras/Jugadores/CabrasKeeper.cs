
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyKeeperStates;

public class CabrasKeeper : PecesPlayer
{
    [Header("Positions")]
    public Transform keeperPosition;
    public Transform overlapPosition;
    public GameObject ringToPorotect;    
    [Header("Fuzzy")]
    public float magnitudePercent;
    public float HowFarBall;
    public EstadoDeAlerta currentAlertState;
    public AnimationCurve toClose;
    public AnimationCurve close;
    public AnimationCurve antcipate;
    [Header("Variables")]
    public float overlapRadius;
    public float throwStrength = 50;
    public float distanceToShoot = 10;
    public List<RingToProtect> rings;
    public CabrasSteeringBlender steeringCabras;

    public enum EstadoDeAlerta
    {
        Perseguir = 0,
        Anticipar,
        Predecir,
        FueraDeRango
    }
    //public AnimationCurve 
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        
        SetTeamRings();
        // Agregar los estados de este agente, chaser
        PrepareToPlay prepare = new PrepareToPlay(this);
        GoToPosition goToPos = new GoToPosition(this);
        Anticipate anticipate = new Anticipate(this);
        PersuitTarget persuitTarget = new PersuitTarget(this);
        SeekTarget seekTarget = new SeekTarget(this);

        fsm.AddState(KeeperStateID.PrepareToPlay, prepare);
        fsm.AddState(KeeperStateID.GoToPosition, goToPos);
        fsm.AddState(KeeperStateID.Anticipate, anticipate);
        fsm.AddState(KeeperStateID.Persuit, persuitTarget);
        fsm.AddState(KeeperStateID.Seek, seekTarget);

        fsm.ChangeState(KeeperStateID.PrepareToPlay);
        fsm.Activate();

        Vector3 maxPosition = new Vector3(overlapPosition.position.x + overlapRadius, overlapPosition.position.y, overlapPosition.position.z);
        Vector3 distance = maxPosition - ringToPorotect.transform.position;
        magnitudePercent = distance.magnitude;
        steeringCabras = GetComponent<CabrasSteeringBlender>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        HowCloseIsTheBall();
        if (HowFarBall > 100f)
        {
            currentAlertState = EstadoDeAlerta.FueraDeRango;
            return;
        }
        EvaluateDistanceFromQuaffle();
    }

    public void EvaluateDistanceFromQuaffle()
    {
        AnimationCurve[] curvas = new AnimationCurve[3] { toClose, close, antcipate };
        int estadoDef = -1;
        float estado = -1;
        for(int i =0; i<3; i++)
        {
            float cantidad = curvas[i].Evaluate(HowFarBall);
            if(cantidad> estado)
            {
                estado = cantidad;
                estadoDef = i;
            }
        }
        switch (estadoDef)
        {
            case 0: this.currentAlertState = EstadoDeAlerta.Perseguir; break;
            case 1: this.currentAlertState = EstadoDeAlerta.Anticipar; break;
            case 2: this.currentAlertState = EstadoDeAlerta.Predecir; break;
            default: this.currentAlertState = EstadoDeAlerta.FueraDeRango; break;
        }

    }

    public Transform NearestRingToQuaffle()
    {
        //aqui se podria agregar logica difusa
        float magnitud = int.MaxValue;
        RingToProtect aroCercano = null;
        //agarar la quaffle y compararla con el aro mas cercano
        foreach(RingToProtect aro in rings)
        {
            Vector3 direccion = aro.transform.position - quaffle.position;

            if(direccion.magnitude < magnitud)
            {
                magnitud = direccion.magnitude;
                aroCercano = aro;
            }
        }
        if(HowFarBall < 100f)
            Debug.DrawLine(quaffle.GetComponent<Ball>().transform.position, aroCercano.transform.position, Color.magenta);
        return aroCercano.transform;
        
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


    public Collider[] OverlapCheckSphere()
    {
        return Physics.OverlapSphere(overlapPosition.position, overlapRadius);
    }

    private void SetTeamRings()
    {
        rings = new List<RingToProtect>();
        Collider[] ringsToProtect = OverlapCheckSphere();
        foreach (Collider item in ringsToProtect)
        {
            ScoreScript score = item.GetComponent<ScoreScript>();
            if(score != null)
            {
                RingToProtect r = new RingToProtect(item.name, item.transform);
                rings.Add(r);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(overlapPosition.position, overlapRadius);
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