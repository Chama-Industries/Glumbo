using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemyscript : MonoBehaviour
{
    [SerializeField] List<Transform> Waypoint = new List<Transform>();
    NavMeshAgent enemymove;
    int waypointinedex = 0;
    // Start is called before the first frame update
    void Start()
    {
        enemymove = GetComponent<NavMeshAgent>();
        enemymove.SetDestination(Waypoint[0].position);
    }
    Vector3 nextmove()
    {
        waypointinedex++;
        waypointinedex %= Waypoint.Count;
        return Waypoint[waypointinedex].position;

    }
    // Update is called once per frame
    void Update()
    {
        if (enemymove.remainingDistance <= enemymove.stoppingDistance)
        {
            enemymove.SetDestination(nextmove());
        }
    }
}