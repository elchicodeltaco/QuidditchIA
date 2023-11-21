using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CabrasTeamState
{
    public enum TeamState
    {
        Preparando,
        Defendiendo,
        Atacando,
        BolaLibre
    }
   

    public class Preparar : State
    {
        private CabrasTeam team;

        public Preparar(CabrasTeam team)
        {
            this.team = team;
        }

        public override void OnEnter(GameObject objeto)
        {
            //Debug.Log("preparando Equipo");
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
            if(team.estadoEquipo == TeamState.Atacando)
            {
                team.fsm.ChangeState(TeamState.Atacando);
            }
            if(team.estadoEquipo == TeamState.Defendiendo)
            {
                team.fsm.ChangeState(TeamState.Defendiendo);
            }
            if(team.estadoEquipo == TeamState.BolaLibre)
            {
                team.fsm.ChangeState(TeamState.BolaLibre);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            Debug.Log("Exit preparando Equipo");
        }
    }
    public class Defender : State
    {
        private CabrasTeam team;

        public Defender(CabrasTeam team)
        {
            this.team = team;
        }

        public override void OnEnter(GameObject objeto)
        {
            Debug.Log("Defendiendo en equipo");
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
        }
        public override void OnExit(GameObject objeto)
        {
            Debug.Log("Exit DefendiendoEquipo");
        }
    }
    public class Atacar : State
    {
        private CabrasTeam team;

        public Atacar(CabrasTeam team)
        {
            this.team = team;
        }

        public override void OnEnter(GameObject objeto)
        {
            Debug.Log("Atacando al otro Equipo");

        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
        }
        public override void OnExit(GameObject objeto)
        {
            Debug.Log("Exit Atacando al otro Equipo");
        }
    }
}
