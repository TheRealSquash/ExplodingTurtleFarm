using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TurtleAI : MonoBehaviour
{
    private enum State
    {
        Hunting,
        Resting,
        Wandering,
        Return
    }

    [SerializeField] private State state;

    private NavMeshAgent navMeshAgent;
    [SerializeField] private Transform targetDestination;
    private SphereCollider range; // Vision range, affected by awareness genetics

    // Genetics
    [SerializeField] private float socialability = 1.0f;
    [SerializeField] private float satiability = 1.0f;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float attractiveness = 1.0f;
    [SerializeField] private float attraction = 1.0f;
    [SerializeField] private float resistance = 1.0f;
    [SerializeField] private float immunity = 1.0f;
    public float awareness = 7.0f;

    // Stats
    [SerializeField] private int fullness = 100;
    [SerializeField] private GameObject food;
    [SerializeField] private bool inRest = false, inWander = false;
    public GameObject home;
    [SerializeField] private DayCycle dayCycle;

    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        range = GetComponent<SphereCollider>();
        navMeshAgent.speed = speed;
        dayCycle = GameObject.FindWithTag("DayCycle").GetComponent<DayCycle>();

        state = State.Wandering;

        StartCoroutine("DepleteFullness");
    }

    public void SetDestination(Vector3 destination) 
    {
        navMeshAgent.destination = destination;
    }

    public void SetFood(GameObject food) 
    {
        this.food = food;
        if (state != State.Return) state = State.Hunting;
    }

    private void Update() 
    {
        switch(state)
        {
            case State.Hunting:
                Debug.Log("Hunting");
                Hunt();
                break;
            case State.Resting:
                Debug.Log("Resting");
                Rest();
                break;
            case State.Wandering:
                Debug.Log("Wandering");
                Wander();
                break;
            case State.Return:
                Debug.Log("Returning");
                ReturnHome();
                break;
            default:
                Debug.Log("Confused");
                Rest();
                break;
        }

        if (dayCycle.timeLeft <= 50)
        {
            state = State.Return;
        }
    }

    private void ReturnHome()
    {
        if (navMeshAgent.destination != home.transform.position)
        {
            navMeshAgent.destination = home.transform.position;
        }

        if (Vector3.Distance(navMeshAgent.destination, transform.position) < 0.5f)
        {
            gameObject.SetActive(false);
        }
    }

    private void Wander()
    {
        if(!inWander)
        {
            navMeshAgent.destination = Random.onUnitSphere * Random.Range(5, 21);
            inWander = true;
        }
        else
        {
            if (Vector3.Distance(navMeshAgent.destination, transform.position) < 0.5f)
            {
                inWander = false;
                navMeshAgent.destination = transform.position;
                state = State.Resting;
            }
        }
    }

    private void Hunt()
    {
        if (food)
        {
            if (navMeshAgent.destination != food.transform.position)
            {
                navMeshAgent.destination = food.transform.position;
            }

            if (Vector3.Distance(transform.position, food.transform.position) < 0.5f)
            {
                Eat(10);
                Destroy(food);
                food = null;
                state = State.Resting;
            }
        }
    }

    private void Rest()
    {
        if(!inRest)
        {
            StartCoroutine(DoRest(Random.Range(2, 5)));
        }
    }

    // every 2 seconds perform the print()
    private IEnumerator DepleteFullness ()
    {
        if (state != State.Resting) fullness--;

        yield return new WaitForSeconds(1);

        StartCoroutine("DepleteFullness");
    }

    private IEnumerator DoRest(int restTime)
    {
        inRest = true;

        yield return new WaitForSeconds(restTime);

        if (state != State.Hunting && state != State.Return) state = State.Wandering; 

        inRest = false;
    }

    private void Eat(float satiation)
    {
        Debug.Log("Eating");
        fullness = (int) Mathf.Clamp(fullness + satiation, 0, 100);

        state = State.Resting;
    }
}
