using System.Collections;
using UnityEngine;

public class ShellProjectile : MonoBehaviour
{
    [SerializeField] float shellSpeed = 10;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(shellSpeed * Time.deltaTime * transform.forward, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( !collision.gameObject.CompareTag("Shell"))
        {
            Destroy(gameObject);
        }
        
    }
}
