using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] PowerUpType type;
    public int ammoCount;


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
            } else if (type == PowerUpType.Healh)
            {
                other.GetComponent<Player>().Heal(ammoCount);
            }

            gameObject.SetActive(false);

        }
    }
}

public enum PowerUpType
{
    Shotgun,
    FireAEO,
    Healh
}

