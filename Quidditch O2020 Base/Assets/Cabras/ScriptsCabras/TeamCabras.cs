using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamCabras : Team
{
    public string TeamName = "Cabras";

    private int MyTeamNumber;

    public int GetTeamNumer()
    {
        return MyTeamNumber;
    }
    public List<Transform> LasCabras;
    public List<Transform> LosRivales;

    private GameObject QuaffleOwner;
    private Transform ClosestTeammateToQuaffle;


    public List<Transform> rivalGoals;
    private List<Transform> ownGoals;

    public List<Transform> MyStartingPositions;

    public List<Transform> myStartingPositions; // Saber donde inician mis jugadores
    public Transform mySeekerStartingPosition;

    public Color MyTeamColor;

    // Start is called before the first frame update
    protected override void Start()
    {
        LasCabras = new List<Transform>();

        LasCabras.Add(transform.Find("Primer Cazador"));
        LasCabras.Add(transform.Find("Segundo Cazador"));
        LasCabras.Add(transform.Find("Tercer Cazador"));
        LasCabras.Add(transform.Find("Guardian"));
        LasCabras.Add(transform.Find("Golpeador Uno"));
        LasCabras.Add(transform.Find("Golpeador Dos"));
        LasCabras.Add(transform.Find("Buscador"));

        Teammates = LasCabras;
        MyTeamNumber = GameManager.instancia.SetTeamName(TeamName);

        //Ya sabemos la posicion del equipo

        if (MyTeamNumber == 1)
        {
            GameManager.instancia.team1Players = LasCabras;

            rivalGoals = GameManager.instancia.team2Goals;
            ownGoals = GameManager.instancia.team1Goals;
        }

        else if (MyTeamNumber == 2)
        {

            GameManager.instancia.team2Players = LasCabras;

            rivalGoals = GameManager.instancia.team1Goals;
            ownGoals = GameManager.instancia.team2Goals;
        }

        GameManager.instancia.SetTeamColor(MyTeamNumber, MyTeamColor);

        Invoke("FillLateData", 1f);

    }

    void FillLateData()
    {
        if (GetTeamNumer() == 1)
        {
            // Mis rivales
            LosRivales = GameManager.instancia.team2Players;
            Rivals = LosRivales;
            // Mis posiciones iniciales
            myStartingPositions = GameManager.instancia.Team1StartPositions;
            mySeekerStartingPosition = GameManager.instancia.Team1SeekerStartPosition;

        }
        else
        {
            LosRivales = GameManager.instancia.team1Players;
            Rivals = LosRivales;
            myStartingPositions = GameManager.instancia.Team2StartPositions;
            mySeekerStartingPosition = GameManager.instancia.Team2SeekerStartPosition;
        }

        for (int j = 0; j < 6; j++)
        {

            LasCabras[j].GetComponent<Player>().myNumberInTeam = j;
            LasCabras[j].GetComponent<Player>().myStartingPosition = myStartingPositions[j];
        }
        LasCabras[6].GetComponent<Player>().myNumberInTeam = 6;
        LasCabras[6].GetComponent<Player>().myStartingPosition = mySeekerStartingPosition;

    }
}
