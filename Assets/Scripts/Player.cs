using JetBrains.Annotations;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector3 startPosition;
    [SerializeField] float movementSpeed = 5;

    [SerializeField] Transform shellSpawn;
    [SerializeField] Transform leftShellSpawn;
    [SerializeField] Transform rightShellSpawn;
    [SerializeField] GameObject shellSpawnPrefab;
    [SerializeField] GameObject fireShellSpawnPrefab;

    bool readyToShootShell = false;
    float basicShellCooldown = 0.25f;
    float shotgunShellCooldown = 0.25f;
    [SerializeField] CharacterController controller;

    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float gravityValue = -9.81f;

    public LayerMask IgnoreEnvironment;

    int currentAttackAmmo = 0;

    public float health = 10;
    public float maxHealth = 10;

    [SerializeField] AttackType currentAttackType = AttackType.Basic;


    Animator animator;

    public delegate void PlayerDied(GameObject player);
    public static PlayerDied playerDied;

    public delegate void UpdatePlayerHealth(float health);
    public static UpdatePlayerHealth playerHealth;

    public delegate void UpdateAttackType(AttackType attackType);
    public static UpdateAttackType updateAttackType;

    public delegate void UpdateAttackAmmo(int attackAmmo);
    public static UpdateAttackAmmo updateAttackAmmo;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        startPosition = transform.position;

        updateAttackType?.Invoke(AttackType.Basic);
        updateAttackAmmo?.Invoke(999);
    }

    private void OnEnable()
    {
        SpawnEnemies.waveStarted += WaveStarted;
        SpawnEnemies.waveCompleted += WaveEnded;
        UIManager.resetGame += ResetPlayer;
    }

    private void OnDisable()
    {
        SpawnEnemies.waveStarted -= WaveStarted;
        SpawnEnemies.waveCompleted -= WaveEnded;
        UIManager.resetGame -= ResetPlayer;
    }

    void Update()
    {
        // Don't need to let player move, attack, look around if we are dead
        if (health > 0)
        {
            HandleMovement();
            HandleLook();
            HandleAttack();
        }

        HandleHealth();
    }
    void HandleHealth()
    {
        // Set healthbar to match health
        float healthPercent = health / maxHealth;
        // Set health UI element
        Debug.Log("Health percent:" + healthPercent);
        playerHealth?.Invoke((healthPercent) * 100);

        if (health <= 0)
        {
            // player died
            animator.SetBool("Death", true);
            playerDied?.Invoke(gameObject);
        }

    }
    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(new Vector3(1,0,0) * horizontalInput + new Vector3(0, 0, 1) * verticalInput);

        Vector3 move = (new Vector3(1, 0, 0) * horizontalInput) + (new Vector3(0, 0, 1) * verticalInput);
        controller.Move(movementSpeed * Time.deltaTime * move);

        if (move.magnitude > 0.1f)
        {
            animator.SetBool("Running", true);
        }
        else
        {
            animator.SetBool("Running", false);
        }

        if (!groundedPlayer)
        {
            playerVelocity.y += gravityValue * Time.deltaTime;
        }


        controller.Move(playerVelocity * Time.deltaTime);
    }

    void HandleLook()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f, ~IgnoreEnvironment))
        {
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }

    void HandleAttack()
    {
        if (Input.GetKey(KeyCode.Mouse0) && readyToShootShell)
        {
            readyToShootShell = false;

            if (currentAttackType == AttackType.Basic)
            {
                BasicAttack();
            } else if (currentAttackType == AttackType.Shotgun)
            {
                ShotgunAttack();
            } else if (currentAttackType == AttackType.FireAOE)
            {
                FireAttack();
            }
            
        }
    }
    IEnumerator ShellCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        readyToShootShell = true;
    }

    void BasicAttack()
    {
        StartCoroutine(ShellCooldown(basicShellCooldown));
        Instantiate(shellSpawnPrefab, shellSpawn.position, transform.rotation);
    }

    void ShotgunAttack()
    {
        currentAttackAmmo--;
        StartCoroutine(ShellCooldown(shotgunShellCooldown));

        // Angle each shot out
        Vector3 leftRotation = transform.eulerAngles + new Vector3(0, -10, 0);
        Vector3 rightRotation = transform.eulerAngles + new Vector3(0, 10, 0);


        Instantiate(shellSpawnPrefab, shellSpawn.position, transform.rotation);

        Instantiate(shellSpawnPrefab, leftShellSpawn.position, Quaternion.Euler(leftRotation));
        Instantiate(shellSpawnPrefab, rightShellSpawn.position, Quaternion.Euler(rightRotation));

        updateAttackAmmo?.Invoke(currentAttackAmmo);

        if (currentAttackAmmo < 1)
        {
            currentAttackType = AttackType.Basic;
            updateAttackType?.Invoke(AttackType.Basic);
            updateAttackAmmo?.Invoke(999);
        }
    }

    void FireAttack()
    {
        currentAttackAmmo--;
        Instantiate(fireShellSpawnPrefab, shellSpawn.position, transform.rotation);

        updateAttackAmmo?.Invoke(currentAttackAmmo);

        if (currentAttackAmmo < 1)
        {
            currentAttackType = AttackType.Basic;
            updateAttackType?.Invoke(AttackType.Basic);
            updateAttackAmmo?.Invoke(999);
        }

        StartCoroutine(ShellCooldown(basicShellCooldown));
    }

    public void SetAttackType(AttackType attackType, int ammoCount)
    {
        currentAttackAmmo = ammoCount;
        Debug.Log("Set attack type");
        if (attackType == AttackType.Shotgun)
        {
            currentAttackType = attackType;
        } else if (attackType == AttackType.FireAOE)
        {
            currentAttackType = attackType;
        }

        updateAttackType?.Invoke(attackType);
        updateAttackAmmo?.Invoke(ammoCount);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Player take damage. New Health: " + health.ToString());
        health -= damage;

        
    }

    public void HandlePlayerDeath(GameObject player)
    {
        animator.SetBool("Death", true);
    }

    public void WaveStarted(int waveNumber)
    {
        readyToShootShell = true;
    }

    public void WaveEnded()
    {
        readyToShootShell = false;
    }

    void ResetPlayer()
    {
        health = maxHealth;
        transform.position = startPosition;
        animator.SetBool("Death", false);
    }

    public void Heal(int healAmount)
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
    }
}

public enum AttackType
{
    Basic,
    Shotgun,
    FireAOE
}
