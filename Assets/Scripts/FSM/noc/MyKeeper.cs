using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyKeeperStates;

public class MyKeeper : Player
{
    [Header("Positions")]
    public Transform keeperPosition;
    public Transform overlapPosition;
    public GameObject ringToPorotect;
    public float HowFarBall;
    public string estado;
    public AnimationCurve toClose;
    public AnimationCurve close;
    public AnimationCurve antcipate;
    public Transform quaffleBall;
    public List<RingToProtect> rings;
    private float magnitudePercent;

    public float overlapRadius;
    public float throwStrength = 50;
    public float distanceToShoot = 10;


    //public AnimationCurve 
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        quaffleBall = GameObject.FindGameObjectWithTag("Ball Quaffle").transform;
        SetTeamRings();
        // Agregar los estados de este agente, chaser
        PrepareToPlay prepare = new PrepareToPlay(this);
        GoToPosition goToPos = new GoToPosition(this);
        Anticipate anticipate = new Anticipate(this);

        fsm.AddState(KeeperStateID.PrepareToPlay, prepare);
        fsm.AddState(KeeperStateID.GoToPosition, goToPos);
        fsm.AddState(KeeperStateID.Anticipate, anticipate);
        //fsm.AddState(ChaserStateID.SearchGoal, search);
        //fsm.AddState(ChaserStateID.EscortTeammate, escort);
        //fsm.AddState(ChaserStateID.ChaseRival, rival);

        fsm.ChangeState(KeeperStateID.PrepareToPlay);

        Vector3 maxPosition = new Vector3(overlapPosition.position.x + overlapRadius, overlapPosition.position.y, overlapPosition.position.z);
        Vector3 distance = maxPosition - ringToPorotect.transform.position;
        magnitudePercent = distance.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        
    }

    public void EvaluateDistanceFromQueffle()
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
            case 0: this.estado = "seek"; break;
            case 1: this.estado = "persuit"; break;
            case 2: this.estado = "anticipate"; break;
        }

    }

    public Vector3 NearestRingToQuaffle()
    {
        //aqui se podria agregar logica difusa
        float magnitud = int.MaxValue;
        RingToProtect aroCercano = null;
        HowFarBall = 0;
        //agarar la quaffle y compararla con el aro mas cercano
        foreach(RingToProtect aro in rings)
        {
            Vector3 direccion = aro.transform.position - quaffleBall.position;
            //Vector3 velEnemy = quaffleBall.GetComponent<Ball>().CurrentBallOwner().GetComponent<Rigidbody>().velocity;
            ///float magSobreCien = (direccion.magnitude * 100) / magnitudePercent;
            
            //magSobreCien = (100 - magSobreCien);
            //ringsProbabilities.Add(magSobreCien);
            //total += magSobreCien;
            if(direccion.magnitude < magnitud)
            {
                magnitud = direccion.magnitude;
                aroCercano = aro;
            }
        }
            Debug.DrawLine(quaffleBall.GetComponent<Ball>().CurrentBallOwner().transform.position, aroCercano.transform.position, Color.magenta);
        return aroCercano.transform.position;
        
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
        Vector3 distancia = quaffleBall.transform.position - NearestRingToQuaffle();
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