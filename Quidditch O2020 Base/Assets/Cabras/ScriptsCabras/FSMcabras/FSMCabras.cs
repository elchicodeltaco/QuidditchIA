using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMCabras 
{
    private Stack<FSMStateCabras> pilaEstados = new Stack<FSMStateCabras>();

    public void pushState(FSMStateCabras estado)
    {
        pilaEstados.Push(estado);
    }

    public void popState()
    {
        pilaEstados.Pop();
    }

    public void Update(GameObject gameObject)
    {
        if (pilaEstados.Peek() != null)
        {
            // Ejecuta el estado actual
            pilaEstados.Peek().Invoke(this, gameObject);
        }
    }

    public delegate void FSMStateCabras(FSMCabras fsm, GameObject gameObject);
}
