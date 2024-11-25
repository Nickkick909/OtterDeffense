using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    Transform player;
    ParticleSystem deathPoof;

    public int health;

    bool chasingPlayer = true;

    public delegate void EnemieDied(GameObject enemy);
    public static EnemieDied enemieDied;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        deathPoof = GetComponent<ParticleSystem>();

        animator.SetBool("Running", true);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }

    public void TakeDamage()
    {
        health -= 1;

        if (health < 1)
        {
            StartCoroutine(WaitToDie());
        }
        else
        {
            // Play hit animation
            animator.SetTrigger("Hit");
        }
        
    }

    IEnumerator WaitToDie()
    {
        animator.SetBool("Running", false);
        animator.SetTrigger("Death");
        agent.speed = 0;
        deathPoof.Play();

        enemieDied?.Invoke(gameObject);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
