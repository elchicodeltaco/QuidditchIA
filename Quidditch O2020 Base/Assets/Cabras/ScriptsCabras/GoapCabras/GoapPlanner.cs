using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner
{
    public Queue<GoapAction> ElPlan(
        GameObject agente,
        List<GoapAction> AccionesDisponibles,
        Dictionary<string, bool> WorldState,
        Dictionary<string, bool> Goal)
    {
        // Limpiar las acciones
        foreach (GoapAction accion in AccionesDisponibles)
            accion.Reset();

        // De las acciones disponibles checamos cuales se
        // pueden ejecutar con sus precondiciones procedurales
        List<GoapAction> accionesUsables = new List<GoapAction>();
        foreach (GoapAction accion in AccionesDisponibles)
            if (accion.checkPrecondition(agente))
                accionesUsables.Add(accion);

        // Una vez que sabe cuales acciones se pueden llevar a cabo,
        // construimos el arbol de acciones para encontrar la meta
        List<Node> arbol = new List<Node>();
        // Construir el arbol
        Node inicial = new Node(null, 0f, null, WorldState);
        bool exito = ConstruyeGrafo(inicial, arbol, accionesUsables, Goal);

        if (!exito) // No econtro un plan
        {
            Debug.Log("No encontró una solución");
            return null;
        }

        // De lo contrario, encontró una solución al menos. 
        // Hay que buscar la de menor costo
        Node masBarato = null;
        foreach (Node hoja in arbol)
            if (masBarato == null)
                masBarato = hoja;
            else
                if (hoja.costo < masBarato.costo)
                masBarato = hoja;

        // Como ya encontré el nodo más barato, hago un backtracking
        // para regresar la lista de acciones a seguir
        List<GoapAction> resultado = new List<GoapAction>();
        Node temp = masBarato;
        while(temp!=null)
        {
            if (temp.accion != null) // agrega la accion al frente
                resultado.Insert(0, temp.accion);
            temp = temp.padre;
        }
        Queue<GoapAction> colaAcciones = new Queue<GoapAction>();
        foreach (GoapAction accion in resultado)
            colaAcciones.Enqueue(accion);

        // Lo logramos! Ten tu plan!
        return colaAcciones;
    }

    private bool ConstruyeGrafo(
        Node inicial, List<Node> arbol, List<GoapAction> acciones,
        Dictionary<string, bool> goal)
    {
        bool encontroSolucion = false;

        // Ir en cada accion y ver si se puede usar
        foreach(GoapAction accion in acciones)
        {
            // Si en el estado se encuentran las condiciones para
            // satisfacer las precondiciones de la accion
            if(ChecaPrecondicionesEnEstado(
                accion.GetPrecondiciones, inicial.estado))
            {
                // Como si se podría ejecutar la acción
                // los efectos se tienen que realizar en el nodo
                Dictionary<string, bool> estadoActual =
                    ActualizaEstado(inicial.estado, accion.GetEfectos);

                // con el estado actualizado, creamos su nodo
                Node nodo = new Node(
                    inicial, inicial.costo + accion.cost,
                    accion, estadoActual);

                // Verificar si el nuevo nodo es terminal, es decir, la meta
                if(ChecaPrecondicionesEnEstado(goal, estadoActual))
                {
                    // Encontró una soluciónh
                    arbol.Add(nodo);
                    encontroSolucion = true;
                }
                else
                {
                    // Esta no es una solución, tiene que seguir expandiendo
                    // el árbol con las acciones que quedan.
                    List<GoapAction> accionesRestantes = new List<GoapAction>();
                    foreach (GoapAction acc in acciones)
                        if (!acc.Equals(accion))
                            accionesRestantes.Add(acc);

                    bool nuevaSolucion =
                        ConstruyeGrafo(nodo, arbol, accionesRestantes, goal);
                    if (nuevaSolucion)
                        encontroSolucion = true;
                } // llave del else
            } // llave del if
        } // llave del foreach
        return encontroSolucion;
    }

    // Actualizar el estado actual con los efectos que tenga una acción
    private Dictionary<string, bool> ActualizaEstado(
        Dictionary<string, bool> estadoActual,
        Dictionary<string, bool> efectosDeAccion)
    {
        // Primero ponemos la información del estado Acual
        Dictionary<string, bool> unEstado = new Dictionary<string, bool>();

        foreach (KeyValuePair<string, bool> valor in estadoActual)
            unEstado.Add(valor.Key, valor.Value);

        // Agregamos los efectos de la accion si es que no existen en el estado
        foreach(KeyValuePair<string, bool> efecto in efectosDeAccion)
        {
            bool yaExiste = false;
            // Puede que el dato ya exista pero que su valor haya cambiado
            foreach(KeyValuePair<string, bool> dato in unEstado)
            {
                bool val;
                if(dato.Key.Equals(efecto.Key))
                {
                    yaExiste = true; break;
                }
            }
            if (yaExiste) // acualizo el dato
                unEstado[efecto.Key] = efecto.Value;
            else // si no existe, lo agregamos
                unEstado.Add(efecto.Key, efecto.Value);
        }// llave del foreach
        return unEstado;
    } 

    // Verifico que las precondiciones estén en el estado del 
    // mundo en el nodo actual.
    // Basta que una precondición no esté en el estado para 
    // que no se cumpla la condición
    private bool ChecaPrecondicionesEnEstado(
        Dictionary<string, bool> precondiciones,
        Dictionary<string,bool> estado)
    {
        bool sonIguales = true;

        foreach(KeyValuePair<string, bool> precon in precondiciones)
        {
            bool igual = false;
            foreach(KeyValuePair<string, bool> valor in estado)
            {
                if(valor.Equals(precon))
                { // encontré una precondicion en el estado
                    igual = true;
                    break;
                }
            }// llave del 2o foreach
            // si no es igual, no está en el estado, por lo tanto 
            // se cumple la precondición
            if (!igual)
                sonIguales = false;
        }
        return sonIguales;
    }

    private class Node
    {
        public Node padre;
        public float costo;
        public GoapAction accion;
        public Dictionary<string, bool> estado;

        public Node(Node padre, float costo, GoapAction accion,
            Dictionary<string, bool> estado)
        {
            this.padre = padre;
            this.costo = costo;
            this.estado = estado;
            this.accion = accion;
        }
    } // llave de clase Node
} // llave del goapplanner
