using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;

    [SerializeField] Transform shellSpawn;
    [SerializeField] Transform leftShellSpawn;
    [SerializeField] Transform rightShellSpawn;
    [SerializeField] GameObject shellSpawnPrefab;
    [SerializeField] GameObject fireShellSpawnPrefab;

    bool readyToShootShell = true;
    float basicShellCooldown = 0.25f;
    float shotgunShellCooldown = 0.25f;
    [SerializeField] CharacterController controller;

    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float gravityValue = -9.81f;

    int currentAttackAmo = 0;

    int shotgunPowerUpAmo = 10;
    int firePowerUpAmo = 1;

    int health = 10;
    int maxHealth = 10;

    [SerializeField] AttackType currentAttackType = AttackType.Basic;


    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        HandleMovement();
        HandleLook();
        HandleAttack();
        HandleHealth();
    }
    void HandleHealth()
    {
        // Set healthbar to match health
        float healthPercent = health / maxHealth;
        // Set health UI element

        if (health > 0)
        {
            // player died


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

        if (Physics.Raycast(ray, out hit))
        {
            transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
        }
    }

    void HandleAttack()
    {
        if (Input.GetKey(KeyCode.Space) && readyToShootShell)
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
        currentAttackAmo--;
        StartCoroutine(ShellCooldown(shotgunShellCooldown));

        // Angle each shot out
        Vector3 leftRotation = transform.eulerAngles + new Vector3(0, -10, 0);
        Vector3 rightRotation = transform.eulerAngles + new Vector3(0, 10, 0);


        Instantiate(shellSpawnPrefab, shellSpawn.position, transform.rotation);

        Instantiate(shellSpawnPrefab, leftShellSpawn.position, Quaternion.Euler(leftRotation));
        Instantiate(shellSpawnPrefab, rightShellSpawn.position, Quaternion.Euler(rightRotation));

        if (currentAttackAmo < 1)
        {
            currentAttackType = AttackType.Basic;
        }
    }

    void FireAttack()
    {
        currentAttackAmo--;
        Instantiate(fireShellSpawnPrefab, shellSpawn.position, transform.rotation);

        if (currentAttackAmo < 1)
        {
            currentAttackType = AttackType.Basic;
        }

        readyToShootShell = true;
    }

    public void SetAttackType(AttackType attackType)
    {
        Debug.Log("Set attack type");
        if (attackType == AttackType.Shotgun)
        {
            currentAttackType = attackType;
            currentAttackAmo = shotgunPowerUpAmo;
        } else if (attackType == AttackType.FireAOE)
        {
            currentAttackType = attackType;
            currentAttackAmo = shotgunPowerUpAmo;
        }
    }
}

public enum AttackType
{
    Basic,
    Shotgun,
    FireAOE
}
