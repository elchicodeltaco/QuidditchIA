using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabrasSteeringBlender : MonoBehaviour
{
    public float maxSpeed;
    public float maxForce;
    private Rigidbody rb;
    private Player player;
    

    Vector3 Velocity;
    Vector3 SteeringForce;

    public List<GameObject> NearPlayers;
    public List<GameObject> NearTeammates;
    public List<GameObject> NearRivals;
    public float nearPlayersSensorRadius;
    public SphereCollider NearPlayersSensor;

    void Start()
    {
        // Soy pelota o jugador
        player = GetComponent<Player>();

        rb = GetComponent<Rigidbody>();

        NearPlayers = new List<GameObject>();
        NearTeammates = new List<GameObject>();
        NearRivals = new List<GameObject>();
        NearPlayersSensor = transform.Find("Player Sensor").GetComponent<SphereCollider>();

        //obstacleDetector = transform.Find("Obstacle Detector").GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ajustar el tamaño del sensor de vecinos
        //NearPlayersSensor.radius = nearPlayersSensorRadius;
        //print(SteeringForce);
        SteeringForce = Vector3.ClampMagnitude(SteeringForce, maxForce);

        // Aceleracion = Fuerza / masa
        Vector3 acceleration = SteeringForce / rb.mass;

        // Actualizamos la velocidad
        Velocity += acceleration * Time.deltaTime;

        // Que el agente no supere la velocidad maxima
        Velocity = Vector3.ClampMagnitude(Velocity, maxSpeed);

        // Actualizo la posicion
        //Vector3 newVelocity = new Vector3(Velocity.x, Velocity.y, 0f);
        rb.velocity = Velocity;

        // Girar al agente si lleva una velocidad significativa
        if (rb.velocity.magnitude > 0.01f)
        {
            transform.LookAt(transform.position + Velocity);
        }

    }
    void OnTriggerEnter(Collider colisionador)
    {
        // Si es un jugador lo agrego a mi lista 
        //if (GameManager.instancia.team1Players.Contains(colisionador.transform) ||
        //    GameManager.instancia.team2Players.Contains(colisionador.transform))
        //{
        //    if (!NearPlayers.Contains(colisionador.gameObject))
        //    {
        //        NearPlayers.Add(colisionador.gameObject);
        //    }
        //    // soy una pelota o un jugador
        //    if (player != null)
        //    {// Si es un compañero
        //        if (player.myTeam.isTeammate(colisionador.gameObject) && !NearTeammates.Contains(colisionador.gameObject))
        //            NearTeammates.Add(colisionador.gameObject);

        //        // Si es un rival
        //        else if (!NearRivals.Contains(colisionador.gameObject))
        //            NearRivals.Add(colisionador.gameObject);
        //    }
        //}

    }

    void OnTriggerExit(Collider col)
    {
        //// Deja de estar en mi rango de sensor de vecinos
        //if (NearPlayers.Contains(col.gameObject))
        //    NearPlayers.Remove(col.gameObject);

        //if (player != null)
        //{
        //    if (NearTeammates.Contains(col.gameObject))
        //        NearTeammates.Remove(col.gameObject);

        //    if (NearRivals.Contains(col.gameObject))
        //        NearRivals.Remove(col.gameObject);
        //}
    }

    public void Seek(Vector3 target, float weight)
    {
        point = target;
        Vector3 DesiredVelocity = 
            (target - transform.position).normalized * maxSpeed;
        SteeringForce += (DesiredVelocity - Velocity) * weight;
    }

    public float radius = 100;
    public void Flee(Vector3 target, float weight)
    {
        Vector3 direction = transform.position - target;
        if (direction.magnitude < radius)
        {
            direction.Normalize();
            Vector3 velocidad = direction * maxSpeed;
            Vector3 vectorSteering = velocidad - Velocity;
            SteeringForce += vectorSteering * weight;
        }
        SteeringForce += Vector3.zero;
    }

    public enum decelerationVel
    {
        fast = 1, mid, slow
    }
    public void Arrive(Vector3 target, decelerationVel deceleration, float weigth)
    {
        //maxSpeed *= 10;
        point = target;
        Vector3 direction = target - transform.position;
        float distance = direction.magnitude;
        if (distance > 5) // si no ha llegado al destino
        {
            float decelerationTweeker = 0.3f;
            float speed = distance / ((int)deceleration * decelerationTweeker);
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            Vector3 desireVelocity = direction * speed / distance;
            SteeringForce += (desireVelocity - Velocity) * weigth;
            return;
        }
        //si ya llego al destino se detiene
        rb.velocity = Vector3.zero;
        Velocity = Vector3.zero;
        //transform.position = target;
        SteeringForce += Vector3.zero;
    }

    public void Persuit(Transform evader, float weigth)
    {
        Vector3 direction = evader.position - transform.position;
        //producto punto (hacia a donde mira)
        float relativeHeading = Vector3.Dot(evader.position, transform.position);

        if (Vector3.Dot(direction, transform.position) > 0 && relativeHeading < -0.95f)
        {
            Seek(evader.position, weigth);
            return;
        }

        float lookAheadTime = direction.magnitude / (maxSpeed + evader.GetComponent<Rigidbody>().velocity.magnitude);
        Seek(evader.position + evader.GetComponent<Rigidbody>().velocity * lookAheadTime, weigth);
    }

    Vector3 point = new Vector3();
    public void Interpose(Transform targetA, Transform targetB, float weigth)
    {
        //calcula el punto medio de las posiciones de los agentes
        Vector3 midPoint = (targetA.position + targetB.position) / 2;
        //hace una prediccion de cuanto tiempo le tomara llegar
        float timeToMidPoint = (transform.position - midPoint).magnitude / maxSpeed;
        //basado en la prediccion del tiempo, hace una prediccion de donde terminaran estando los agentes
        //Vector3 PredictionA = targetA.position + targetA.GetComponent<Rigidbody>().velocity * timeToMidPoint;
        Vector3 PredictionB = targetB.position * timeToMidPoint;
        //gracias a estos calculos podemos predecir el punto medio de la prediccion recien hecha
        //midPoint = (PredictionA + PredictionB) / 2;
        //midPoint.z += 35;
        point = midPoint;
        //return GetComponent<Seek>().SeekMethod(midPoint, speed);
        //Arrive(midPoint, decelerationVel.fast, weigth);//y hacia ahi nos dirigimos
        Seek(midPoint, weigth);//y hacia ahi nos dirigimos
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(point, 2);
    }
}
