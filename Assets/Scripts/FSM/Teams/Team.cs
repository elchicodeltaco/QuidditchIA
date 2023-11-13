using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeamStates;

public class Team : MonoBehaviour
{
    public FSM fsm;
    
    // Variables del equipo
	public List<Transform> Teammates;  // Yo quiero tener a mis jugadores en una lista
	public List<Transform> Rivals;      // rivales
        
    // Use this for initialization
    protected virtual void Start()
    {
        // Hay que hacer la fsm del agente
        fsm = new FSM(gameObject, this);

        // Crear los estados en que puede estar 
       
       
        // Activo la fsm
       // fsm.Activate();
    }

    protected virtual void Update()
    {
        if (fsm != null && fsm.IsActive())
        {
            fsm.UpdateFSM();
        }
    }

	public bool isTeammate(GameObject player)
	{
		return Teammates.Contains(player.transform);
	}

	public bool isRival(GameObject player)
	{
		return Rivals.Contains(player.transform);
	}
}
