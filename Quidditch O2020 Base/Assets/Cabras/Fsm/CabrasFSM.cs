using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CabrasFSM
{
    public MonoBehaviour myMono;
    public GameObject gameObject;

    // El estado que esta ejecutando actualmente
    private CabrasEstado currentState;
    // El estado blip
    private CabrasEstado blipPreviousState;

    // Estado global, actúa como un controlador
    private CabrasEstado globalState;

    // Si la fsm se esta ejecutando
    private bool isActive;
    public bool _isActive
    {
        get { return isActive; }
    }
    // Una lista de los estados que tiene esta maquina
    private Dictionary<Enum, CabrasEstado> states;

    public CabrasFSM(GameObject _object, MonoBehaviour _mono)
    {
        myMono = _mono;
        states = new Dictionary<Enum, CabrasEstado>();
        isActive = false;
        currentState = null;
        gameObject = _object;
    }

    public void Activate()
    {
        // Primero verifico que haya estado en la fsm
        if (states.Count == 0)
        {
            Debug.LogError("No se puede activar FSM, no tiene estados");
            return;
        }
        if (currentState == null)
        {
            Debug.LogError("No puedo activar FSM, no hay un estado inicial");
            return;
        }

        isActive = true;
    }
    public void AddState(Enum stateID, CabrasEstado state)
    {
        // Verificar que el estado que quiero agregar no esté ya presente
        if (states.ContainsKey(stateID) || states.ContainsValue(state))
        {
            Debug.LogError("No se puede agregar estado, ya existe");
            return;
        }

        state.SetFSM(this);
        states.Add(stateID, state);
    }

    public void UpdateFSM()
    {
        if (isActive)
        {
            if (globalState != null)
            {
                globalState.Act(gameObject);
                globalState.Reason(gameObject);
            }
            currentState.Act(gameObject); // cumple las acciones del estado
            currentState.Reason(gameObject); // verifica si alguna transicion se activa
        }
        else
        {
            Debug.Log("La maquina de estados no esta activa");
        }
    }
    public void ChangeState(Enum stateID)
    {
        // Ejecutamos acciones de salida del estado
        if (currentState != null)
            currentState.OnExit(gameObject);

        // Obtenemos el nuevo estado al que hay que cambiar
        currentState = GetStateFromEnum(stateID);
        // Como ya cambié de estado, ejecuto las acciones de entrada
        currentState.OnEnter(gameObject);
    }
    CabrasEstado GetStateFromEnum(Enum stateID)
    {
        if (states[stateID] == null)
        {
            Debug.LogError("No econtró el estado con ese ID");
            return null;
        }
        return states[stateID];
    }
    public bool isCurrentState(Enum stateId)
    {
        return currentState == GetStateFromEnum(stateId);
    }
}
