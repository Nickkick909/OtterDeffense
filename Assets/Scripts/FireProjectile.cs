using UnityEngine;

public class FireProjectile : ShellProjectile
{
    [SerializeField] GameObject fireAEO;
    private void OnDestroy()
    {
        Instantiate(fireAEO, transform.position, Quaternion.identity);
    }
}
