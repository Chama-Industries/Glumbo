using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jaxMovementScript : MonoBehaviour
{
    // Controls the speed of the player
    private float speed = 10f;
    private float rotateSpeed = 180f;
    private Vector3 jumpPower = new Vector3(0, 8.75f, 0);
    private Vector3 tempDiagonalBoost = new Vector3(0, 7.5f, 0);
    private Vector3 tempVerticalBoost = new Vector3(0, 12.5f, 0);

    // Rigidbody
    private Rigidbody rb;

    // Reference to the player (if this script is on the player, can be removed)
    public GameObject player;

    // Projectile prefab (RigidbodyExtensions component)
    public RigidbodyExtensions glumboAttackPrefab;

    // Cooldowns
    private int tempAttackCooldown;
    private int tempJumpCooldown;

    // Projectile spawn point
    public Transform attackOrigin;

    // 3D movement variable
    Vector3 movementD;

    // Controls
    public KeyCode jump = KeyCode.Space;
    public KeyCode dance = KeyCode.Q;

    [Header("Projectile Orbit Settings")]
    [Tooltip("Maximum number of orbiting projectiles allowed.")]
    public int maxOrbitProjectiles = 1;

    // List to keep track of active orbiting projectiles
    private List<RigidbodyExtensions> activeOrbitProjectiles = new List<RigidbodyExtensions>();

    // Start is called before the first frame update
    void Start()
    {
        // Assigns player speed
        speed = 25f;
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();

        // Ensure player reference is set
        if (player == null)
        {
            player = this.gameObject; // If this script is on the player
        }

        // Ensure glumboAttackPrefab is assigned
        if (glumboAttackPrefab == null)
        {
            Debug.LogError("jaxMovementScript: glumboAttackPrefab not assigned in the Inspector.");
        }

        // Ensure attackOrigin is assigned
        if (attackOrigin == null)
        {
            Debug.LogError("jaxMovementScript: Attack Origin Transform not assigned in the Inspector.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        advancedPlayerActions();
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        tempAttackCooldown++;
        tempJumpCooldown++;
        additionalPlayerActions();
    }

    // Basic WASD movement controls
    void playerMove()
    {
        float hIn = Input.GetAxis("Horizontal");
        float vIn = Input.GetAxis("Vertical");

        // Adjusted movement direction based on camera or player orientation
        movementD = new Vector3(vIn, 0, -hIn);
        movementD.Normalize();

        transform.Translate(movementD * speed * Time.deltaTime, Space.World);

        // Rotate the player in the direction of movement
        if (movementD != Vector3.zero)
        {
            Quaternion rotationD = Quaternion.LookRotation(movementD, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationD, rotateSpeed * Time.deltaTime);
        }
    }

    // Additional controls: Jumping, Dancing, and Secondary Actions
    void additionalPlayerActions()
    {
        // Jump
        if (Input.GetKey(jump) && tempJumpCooldown > 75)
        {
            rb.AddForce(jumpPower, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Dance
        if (Input.GetKey(dance))
        {
            player.transform.Rotate(0.0f, 1.25f, 0.0f);
        }

        // Attack
        if (Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            LaunchProjectile();
            tempAttackCooldown = 0;
        }
    }

    // Advanced player actions: Combo Moves
    void advancedPlayerActions()
    {
        // Diagonal Boost
        if (Input.GetMouseButton(0) && Input.GetKey(jump) && tempJumpCooldown > 70)
        {
            rb.AddForce(tempDiagonalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Bigger Jump
        if (Input.GetKey(jump) && Input.GetKey(dance) && tempJumpCooldown > 70)
        {
            rb.AddForce(tempVerticalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Orbit Attack
        if (Input.GetKey(dance) && Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            if (activeOrbitProjectiles.Count < maxOrbitProjectiles)
            {
                SpawnOrbitProjectile();
                tempAttackCooldown = 0;
            }
        }
    }

    // Launches a single projectile
    void LaunchProjectile()
    {
        if (glumboAttackPrefab == null)
        {
            Debug.LogWarning("jaxMovementScript: glumboAttackPrefab not assigned.");
            return;
        }

        if (attackOrigin == null)
        {
            Debug.LogWarning("jaxMovementScript: Attack Origin Transform not assigned.");
            return;
        }

        // Instantiate the projectile
        RigidbodyExtensions newProjectile = Instantiate(glumboAttackPrefab, attackOrigin.position, attackOrigin.rotation);

        if (newProjectile != null)
        {
            // Assign the player as the target for the projectile
            newProjectile.LaunchForce = speed;
            newProjectile.MaxDistance = 30f;
            newProjectile.ReturnSpeed = 15f;

            // Launch the projectile forward
            Vector3 launchDirection = transform.forward;
            newProjectile.Launch(launchDirection, newProjectile.LaunchForce);
        }
        else
        {
            Debug.LogError("jaxMovementScript: Failed to instantiate glumboAttackPrefab.");
        }
    }

    // Spawns an orbiting projectile
    void SpawnOrbitProjectile()
    {
        if (glumboAttackPrefab == null)
        {
            Debug.LogWarning("jaxMovementScript: glumboAttackPrefab not assigned.");
            return;
        }

        if (attackOrigin == null)
        {
            Debug.LogWarning("jaxMovementScript: Attack Origin Transform not assigned.");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("jaxMovementScript: Player GameObject not assigned.");
            return;
        }

        // Instantiate the orbiting projectile at the attackOrigin position
        RigidbodyExtensions newProjectile = Instantiate(glumboAttackPrefab, attackOrigin.position, attackOrigin.rotation);

        if (newProjectile != null)
        {
            // Assign the player as the target for the projectile
            newProjectile.LaunchForce = speed;
            newProjectile.MaxDistance = 30f;
            newProjectile.ReturnSpeed = 15f;

            // Launch the projectile forward
            Vector3 launchDirection = transform.forward;
            newProjectile.Launch(launchDirection, newProjectile.LaunchForce);

            // Add to the active projectiles list
            activeOrbitProjectiles.Add(newProjectile);
        }
        else
        {
            Debug.LogError("jaxMovementScript: Failed to instantiate glumboAttackPrefab.");
        }
    }
}
