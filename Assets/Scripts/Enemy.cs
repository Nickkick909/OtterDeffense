using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform player; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage()
    {
        StartCoroutine(WaitToDie());
        
    }

    IEnumerator WaitToDie()
    {
        animator.SetTrigger("Death");
        agent.speed = 0;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
