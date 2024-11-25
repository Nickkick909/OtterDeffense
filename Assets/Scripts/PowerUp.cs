using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpType type;
    public int ammoCount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        Debug.Log(other.gameObject);
        if (other.name == "Player")
        {
            Debug.Log("Trigger entered player");
            if (type == PowerUpType.Shotgun) {
                other.GetComponent<Player>().SetAttackType(AttackType.Shotgun, ammoCount);
            }
            else if (type == PowerUpType.FireAEO)
            {
                other.GetComponent<Player>().SetAttackType(AttackType.FireAOE, ammoCount);
            }

            Destroy(gameObject);

        }
    }
}

public enum PowerUpType
{
    Shotgun,
    FireAEO
}

