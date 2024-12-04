using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    Animator animator;
    NavMeshAgent agent;
    public Transform player;
    ParticleSystem deathPoof;

    public int health;

    bool chasingPlayer = true;

    public delegate void EnemyDied(GameObject enemy);
    public static EnemyDied enemyDied;

    bool canAttackPlayer = true;
    bool inRangeForAttack = false;

    public Player playerObject;

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        deathPoof = GetComponent<ParticleSystem>();

        animator.SetBool("Running", true);

        playerObject = player.gameObject.GetComponentInParent<Player>();
    }

    void Update()
    {
        agent.SetDestination(player.position);
        TryToAttackPlayer();
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

    void TryToAttackPlayer()
    {
        if (canAttackPlayer && inRangeForAttack)
        {
            canAttackPlayer = false;
            playerObject.TakeDamage(1);
            StartCoroutine(WaitForAttackCoolDown());
        }
    }

    IEnumerator WaitToDie()
    {
        animator.SetBool("Running", false);
        animator.SetTrigger("Death");
        agent.speed = 0;
        deathPoof.Play();

        enemyDied?.Invoke(gameObject);

        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeForAttack = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRangeForAttack = false;
        }
    }

    IEnumerator WaitForAttackCoolDown()
    {
        yield return new WaitForSeconds(1f);
        canAttackPlayer = true;
    }
}
