using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float movementSpeed = 5;

    [SerializeField] Transform shellSpawn;
    [SerializeField] GameObject shellSpawnPrefab;

    bool readyToShootShell = true;
    float shellCooldown = 0.25f;
    [SerializeField] CharacterController controller;

    [SerializeField] private Vector3 playerVelocity;
    [SerializeField] private bool groundedPlayer;
    [SerializeField] private float gravityValue = -9.81f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && readyToShootShell)
        {
            readyToShootShell = false;
            StartCoroutine(ShellCooldown());
            Instantiate(shellSpawnPrefab, shellSpawn.position, transform.localRotation);
        }

        HandleMovement();
        HandleLook();
    }

    void HandleMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        //transform.Translate(new Vector3(1,0,0) * horizontalInput + new Vector3(0, 0, 1) * verticalInput);

        Vector3 move = (new Vector3(1, 0, 0) * horizontalInput) + (new Vector3(0, 0, 1) * verticalInput);
        controller.Move(movementSpeed * Time.deltaTime * move);

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

    IEnumerator ShellCooldown()
    {
        yield return new WaitForSeconds(shellCooldown);
        readyToShootShell = true;
    }
}
