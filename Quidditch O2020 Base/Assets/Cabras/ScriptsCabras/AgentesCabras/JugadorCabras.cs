using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class JugadorCabras : Player, IGoap
{
    // Start is called before the first frame update
    //public bool tengoLaPelota = false;
    public LasBolas misBolas;
    public CabrasTeam miTeam;
    public abstract Dictionary<string, bool> CreateGoalState();

    public Dictionary<string, bool> GetWorldState()
    {

        Dictionary<string, bool> datos = new Dictionary<string, bool>();

          datos.Add("tienePelosta", true);
      //  datos.Add("rivalTienePelota", miTeam.isRival(GameManager.instancia.Quaffle.GetComponent<Quaffle>().CurrentBallOwner()));
       // datos.Add("estaPelotaEnJuego", GameManager.instancia.isGameStarted() && GameManager.instancia.isQuaffleControlled());

        return datos;
    }
    public void PlanFailed(Dictionary<string, bool> FailedGoal)
    {


    }

    public void PlanFound(
        Dictionary<string, bool> Goal, Queue<GoapAction> actions)
    {


    }

    public void ActionFinished()
    {

    }

    public void PlanAborted(GoapAction abortedAction)
    {

    }

    public bool moveAgent(GoapAction nextAction)
    {
        return false;
    }

    private void Start()
    {
        misBolas = GetComponent<LasBolas>();
    }
    private void Update()
    {
        
    }
}
