using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class FireDamageAEO : MonoBehaviour
{
    public List<Enemy> enemiesInFire = new List<Enemy>();
    [SerializeField] float lifetime = 10f;
    [SerializeField] float fireTickSpeed = 0.75f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        StartCoroutine(DealFireDamage());
        StartCoroutine(RemoveFireAfterDelay(lifetime));
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInFire.Add(other.gameObject.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInFire.Remove(other.gameObject.GetComponent<Enemy>());
        }
    }

    IEnumerator DealFireDamage()
    {
        List<Enemy> enemiesDead = new List<Enemy>();
        while (true)
        {
            yield return new WaitForSeconds(fireTickSpeed);
            foreach (Enemy e in enemiesInFire)
            {

                if (e != null) 
                {
                    e.TakeDamage();

                    if (e.health < 1)
                    {
                        enemiesDead.Add(e);
                    }
                }


                
            }

            foreach(Enemy e in enemiesDead)
            {
                enemiesInFire.Remove(e);
            }
        }
    }

    IEnumerator RemoveFireAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
