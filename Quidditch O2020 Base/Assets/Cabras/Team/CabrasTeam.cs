﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CabrasTeamState;

public class CabrasTeam : MonoBehaviour
{
    public string cabrasName = "CabrasQC";
    public Color cabrasColor;
    public int cabrasTeamNumber;
    public int getTeamNumber
    {
        get { return cabrasTeamNumber; }
    }

    public FSM fsm;
    public List<Transform> cabras;
    public List<Transform> rivales;

    public List<Transform> arosEnemigos;
    public List<Transform> posicionesIniciales;
    public Transform posicionSeeker;

    private GameObject quaffleBall;
    private Transform ClosestTeammateToQuaffle;
    public TeamState estadoEquipo;

    // Start is called before the first frame update
    void Start()
    {
        cabrasTeamNumber =
            GameManager.instancia.SetTeamName(cabrasName);
        cabras = new List<Transform>();

        if (cabrasTeamNumber == 1)
        {
            //le puedo decir quienes son mis jugadores
            GameManager.instancia.team1Players = cabras;
            // Puedo saber hacia donde tiro
            arosEnemigos = GameManager.instancia.team2Goals;
        }
        else if (cabrasTeamNumber == 2)
        {
            GameManager.instancia.team2Players = cabras;

            arosEnemigos = GameManager.instancia.team1Goals;
        }

        GameManager.instancia.SetTeamColor(cabrasTeamNumber, cabrasColor);

        fsm = new FSM(gameObject, this);
        Preparar preparar = new Preparar(this);
        Defender defender = new Defender(this);
        Atacar atacar = new Atacar(this);

        fsm.AddState(TeamState.Preparando, preparar);
        fsm.AddState(TeamState.Defendiendo, defender);
        fsm.AddState(TeamState.Atacando, atacar);
        //fsm.Activate();
        fsm.ChangeState(TeamState.Preparando);
        quaffleBall = GameObject.FindGameObjectWithTag("Ball Quaffle");
        Invoke("FillLateData", 1f);
    }

    void Update()
    {
        if (fsm != null && fsm.IsActive())
        {
            fsm.UpdateFSM();
        }
        if (quaffleBall.GetComponent<Ball>().CurrentBallOwner() == null)
        {
            estadoEquipo = TeamState.BolaLibre;
            return;
        }
        if (isCabra(quaffleBall.GetComponent<Ball>().CurrentBallOwner()))
        {
            estadoEquipo = TeamState.Atacando;
        }
        if (isRival(quaffleBall.GetComponent<Ball>().CurrentBallOwner()))
        {
            estadoEquipo = TeamState.Defendiendo;

        }

    }

    public bool isCabra(GameObject player)
    {
        return cabras.Contains(player.transform);
    }

    public bool isRival(GameObject player)
    {
        return rivales.Contains(player.transform);
    }

    void FillLateData()
    {
        if (getTeamNumber == 1)
        {
            // Mis rivales
            rivales = GameManager.instancia.team2Players;
            // Mis posiciones iniciales
            posicionesIniciales = GameManager.instancia.Team1StartPositions;
            posicionSeeker = GameManager.instancia.Team1SeekerStartPosition;
        }
        else
        {
            rivales = GameManager.instancia.team1Players;
            posicionesIniciales = GameManager.instancia.Team2StartPositions;
            posicionSeeker = GameManager.instancia.Team2SeekerStartPosition;
        }

        for (int j = 0; j < 6; j++)
        {
            cabras[j].GetComponent<Player>().myNumberInTeam = j;
            cabras[j].GetComponent<Player>().myStartingPosition = posicionesIniciales[j];
        }
        cabras[6].GetComponent<Player>().myNumberInTeam = 6;
        cabras[6].GetComponent<Player>().myStartingPosition = posicionSeeker;

    }
    public void FindClosestTeammateToQuaffle()
    {

        float less = float.MaxValue;
        float dist;

        foreach (Transform chido in cabras)
        {
            dist = Vector3.Distance(chido.position, GameManager.instancia.Quaffle.transform.position);
            if (dist < less)
            {
                less = dist;
                ClosestTeammateToQuaffle = chido;
            }
        }
    }
}

