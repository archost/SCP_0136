using UnityEngine;
using UnityEngine.AI;

public class Nextbot : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(player.transform.position);
    }
}