using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMCazCabras : MonoBehaviour
{
    //Saber cual es el estado activo
    private FSMStatesCazCabras EstadoActual; //Referencia
    public MonoBehaviour Mono;

    //Constructor que llame a un MonoBehaviour
    public FSMCazCabras(MonoBehaviour Mono)
    {
        this.Mono = Mono;
    }

    public void Iniciar(FSMStatesCazCabras inicial) //Cual es el primer estado en el que va a estar
    {
        EstadoActual = inicial;
        EstadoActual.Enter(); //Tan pronto sabemos el edo, ejecutamos función de enter
    }

    public void Update()
    {
        EstadoActual.UpdateEstado();
    }

    public void CambiarDeEstado(FSMStatesCazCabras siguienteEstado)
    {
        //Que no cambie al edo en el que estoy ahorita
        if (siguienteEstado != EstadoActual)
        {
            //Hacer transición. Activar la flechita, pero ante ejecutar Exit
            EstadoActual.Exit();

            //Activar el siguiente edo. Debe de ejecutar su enter (del siguiente)
            siguienteEstado.Enter();
            EstadoActual = siguienteEstado;
        }
    }
}

