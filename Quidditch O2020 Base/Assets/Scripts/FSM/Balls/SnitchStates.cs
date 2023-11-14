using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnitchStates
{
    public enum SnitchStateID // Aqui agreguen las claves de cada estado que quieran
    {
        Waiting,
        Hovering,
        Escaping,
        Wandering,
        Returning
    }

    //=============================================================
    //=================================================== Waiting
    public class Waiting : State
    {
        private Ball ball;

        // Variables del estado

        public Waiting(Ball _ball)
        {
            ball = _ball;
        }
        public override void OnEnter(GameObject objeto)
        {
            ball.steering.ShutDownAll();
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
        }
        public override void OnExit(GameObject objeto)
        {
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }
    //=============================================================
    //=================================================== Hovering
    public class Hovering : State
    {
        private Ball ball;

        // Variables del estado
        float timeHovering = 2f;     // El tiempo que durará en este estado y regresará a wander
        bool waitEnds;
        bool hovering;

        Coroutine Hover;

        public Hovering(Ball _ball)
        {
            ball = _ball;
        }
        public override void OnEnter(GameObject objeto)
        {
            hovering = false;
            waitEnds = false;
        }
        public override void Act(GameObject objeto)
        {
            if(!hovering)
            {
                Hover = fsm.myMono.StartCoroutine(HoverFunction());
            }
        }
        public override void Reason(GameObject objeto)
        {
            if(waitEnds)
            {
                ChangeState(SnitchStateID.Wandering);
            }
            // Si hay jugadores cerca
            if (ball.steering.NearPlayers.Count > 0)
            {
                // Se aleja de ellos
                ChangeState(SnitchStateID.Escaping);
            }
        }
        public override void OnExit(GameObject objeto)
        {
        }

        IEnumerator HoverFunction()
        {
            hovering = true;
            yield return new WaitForSeconds(timeHovering);
            hovering = false;
            waitEnds = true;
        }
    }
    //=============================================================
    //=================================================== Escaping
    public class Escaping : State
    {
        private Ball ball;

        // Variables del estado

        public Escaping(Ball _ball)
        {
            ball = _ball;
        }
        public override void OnEnter(GameObject objeto)
        {
            ball.steering.separation = true;
            ball.steering.wander2 = true;
        }
        public override void Act(GameObject objeto)
        {
        }
        public override void Reason(GameObject objeto)
        {
            // si no tiene jugadores cerca
            if(ball.steering.NearPlayers.Count == 0)
            {
                // Puede deambular por el campo
                ChangeState(SnitchStateID.Wandering);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            ball.steering.separation = false;
            ball.steering.wander2 = false;
        }

        IEnumerator IdleFunction()
        {
            yield return new WaitForSeconds(1f);
        }
    }

    //=============================================================
    //=================================================== Wandering
    public class Wandering : State
    {
        private Ball ball;

        // Variables del estado
        bool calculating;   // evalua si cambia a Hovering
        bool hovers;

        Coroutine Calc;

        public Wandering(Ball _ball)
        {
            ball = _ball;
        }
        public override void OnEnter(GameObject objeto)
        {
            ball.steering.wander2 = true;

            calculating = false;
            hovers = false;
        }
        public override void Act(GameObject objeto)
        {
            if(!calculating)
            {
                Calc = fsm.myMono.StartCoroutine(WanderingFunction());
            }
        }
        public override void Reason(GameObject objeto)
        {
            // Si hay jugadores cerca
            if(ball.steering.NearPlayers.Count > 0)
            {
                // Se aleja de ellos
                ChangeState(SnitchStateID.Escaping);
            }
            // Cada cierto tiempo se quedará en un lugar
            if(hovers)
            {
                ChangeState(SnitchStateID.Hovering);
            }

            // Puede ser que se aleje demasiado de la cancha
            if(Vector3.Distance(ball.transform.position, GameManager.instancia.transform.position) > 1500f)
            {
                ChangeState(SnitchStateID.Returning);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            ball.steering.wander2 = false;
            if(Calc!= null)
                fsm.myMono.StopCoroutine(Calc);
        }

        IEnumerator WanderingFunction()
        {
            calculating = true;
            yield return new WaitForSeconds(5f);
            hovers = Random.value > 0.5f;   // Probabilidad de 50% de que cambie a hover
            calculating = false;
        }
    }

    //=============================================================
    //=================================================== Returning
    public class Returning : State
    {
        private Ball ball;

        public Returning(Ball _ball)
        {
            ball = _ball;
        }
        public override void OnEnter(GameObject objeto)
        {
            ball.steering.arrive = true;
            ball.steering.Target = GameManager.instancia.transform;
        }
        public override void Act(GameObject objeto)
        {
            
        }
        public override void Reason(GameObject objeto)
        {
            // Si hay jugadores cerca
            if (ball.steering.NearPlayers.Count > 0)
            {
                // Se aleja de ellos
                ChangeState(SnitchStateID.Escaping);
            }
            // Cuando haya vuelto lo suficientemente cerca de la cancha
            if (Vector3.Distance(ball.transform.position, ball.steering.Target.position) < 400f)
            {
                ChangeState(SnitchStateID.Wandering);
            }
        }
        public override void OnExit(GameObject objeto)
        {
            ball.steering.arrive = false;
        }

    }
}
