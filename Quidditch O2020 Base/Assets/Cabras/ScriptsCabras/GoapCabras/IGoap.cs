using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Cualquier agente que quiera usar GOAP tiene que implmenetar
 * esta interfaz. Ayudará al Planeador a decidir que acciones tomar.
 * 
 * La usamos también para comunicarnos con el agente y hacerle saber
 * si una acción falla o se cumple.
 * */

public interface IGoap 
{
    // La información del estado del juego o mundo
    Dictionary<string, bool> GetWorldState();

    // Proporcionar al planeador una meta para construir la
    // secuencia de acciones
    Dictionary<string, bool> CreateGoalState();

    void PlanFailed(Dictionary<string, bool> FailedGoal);

    void PlanFound(
        Dictionary<string, bool> Goal, Queue<GoapAction> actions);

    void ActionFinished();

    void PlanAborted(GoapAction abortedAction);

    bool moveAgent(GoapAction nextAction);
}
