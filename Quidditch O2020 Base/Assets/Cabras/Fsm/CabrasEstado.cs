using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CabrasEstado
{
    public CabrasFSM fsm;

    public CabrasEstado(CabrasFSM fsm)
    {
        this.fsm = fsm;
    }
    public void SetFSM(CabrasFSM _fsm)
    {
        fsm = _fsm;
    }
    public void ChangeState(Enum id)
    {
        fsm.ChangeState(id);
    }

    public bool isCurrentState(Enum stateId)
    {
        return fsm.isCurrentState(stateId);
    }

    public virtual void Act(GameObject _object) { }
    public virtual void Reason(GameObject _object) { }

    public virtual void OnEnter(GameObject _object) { }
    public virtual void OnExit(GameObject _object) { }
}
