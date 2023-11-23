using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMStatesCazCabras
{
    protected FSMCazCabras FSM; //para que sepa a qué màquina de edos pertenece
    protected Animator MiAnimador;
    protected float DuracionEstado;
    protected float TiempoTranscurrido;
    protected bool AccionConcluida;

    public FSMStatesCazCabras(FSMCazCabras FSM, Animator MiAnimator) //Regresa objeto instanciado 
    {
        this.FSM = FSM;
        this.MiAnimador = MiAnimator;
        TiempoTranscurrido = 0f;
        AccionConcluida = false;
    }

    public virtual void Enter()
    {
        TiempoTranscurrido = 0f;
        AccionConcluida = false;
    }

    public abstract void UpdateEstado();
    public abstract void Exit();

    protected void Esperar(float duracion)
    {
        TiempoTranscurrido += Time.deltaTime;
        if (TiempoTranscurrido >= duracion)
        {
            TiempoTranscurrido = 0f;
            AccionConcluida = true;
        }
    }
}
