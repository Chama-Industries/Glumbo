using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class jaxMovementScript : MonoBehaviour
{
    // Controls the speed of the player
    private float speed = 10f;
    private float rotateSpeed = 180f;
    private Vector3 jumpPower = new Vector3(0, 10.0f, 0);
    private Vector3 fallingPower = new Vector3(0, -0.4f, 0);
    private Vector3 tempDiagonalBoost = new Vector3(0, 7.5f, 0);
    private Vector3 tempVerticalBoost = new Vector3(0, 15.0f, 0);

    // Rigidbody
    private Rigidbody rb;

    // Reference to the player (if this script is on the player, can be removed)
    public GameObject player;

    // Projectile prefab (GameObject component)
    public GameObject glumboAttack;

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
    public KeyCode secondaryKey = KeyCode.E;

    // Variables to Control & Display the Player's Score
    public TextMeshProUGUI theScore;
    private baselineScore playerScore = new baselineScore();
    private int scoreCheck;


    // Start is called before the first frame update
    void Start()
    {
        // Assigns player speed
        speed = 25f;
        // Get Rigidbody component
        rb = GetComponent<Rigidbody>();
        // assignment allowing us to see if the Displayed Score matched the internal score
        scoreCheck = playerScore.getScore();

        // Ensure player reference is set
        if (player == null)
        {
            player = this.gameObject; // If this script is on the player
        }

        // Ensure glumboAttack is assigned
        if (glumboAttack == null)
        {
            UnityEngine.Debug.LogError("jaxMovementScript: glumboAttack not assigned in the Inspector.");
        }

        // Ensure attackOrigin is assigned
        if (attackOrigin == null)
        {
            UnityEngine.Debug.LogError("jaxMovementScript: Attack Origin Transform not assigned in the Inspector.");
        }

        if (player == null)
        {
            UnityEngine.Debug.LogWarning("jaxMovementScript: Player GameObject not assigned.");
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
        if (Input.GetKey(jump) && tempJumpCooldown > 50)
        {
            rb.AddForce(jumpPower, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Dance
        if (Input.GetKey(dance))
        {
            player.transform.Rotate(0.0f, 1.25f, 0.0f);
        }

        // Fast Falling
        if (Input.GetKey(secondaryKey))
        {
            rb.AddForce(fallingPower, ForceMode.VelocityChange);
        } 

        // Attack
        if (Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            Instantiate(glumboAttack, attackOrigin.position, attackOrigin.rotation);
            tempAttackCooldown = 0;
        }
    }

    // Advanced player actions: Combo Moves
    void advancedPlayerActions()
    {
        // Diagonal Boost
        if (Input.GetMouseButton(0) && Input.GetKey(jump) && tempJumpCooldown > 50)
        {
            rb.AddForce(tempDiagonalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Bigger Jump
        if (Input.GetKey(jump) && Input.GetKey(dance) && tempJumpCooldown > 50)
        {
            rb.AddForce(tempVerticalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        // Orbit Attack
        if (Input.GetKey(dance) && Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            SpawnOrbitProjectile();
            tempAttackCooldown = 0;
        }
    }

    // Spawns an orbiting projectile
    void SpawnOrbitProjectile()
    {
        // Instantiate the orbiting projectile at the attackOrigin position
        GameObject newProjectile = Instantiate(glumboAttack, attackOrigin.position, attackOrigin.rotation);

        // Get the script attached to the projectile
        playerAttack orbitScript = newProjectile.GetComponent<playerAttack>();
        if (orbitScript != null)
        {
            //method call to have the projectile rotate (broken)
            orbitScript.activateOrbit();
        }
        else
        {
            UnityEngine.Debug.LogError("jaxMovementScript: glumboAttack prefab does not have a playerAttack component.");
            Destroy(newProjectile);
        }
    }

    void updateScore()
    {
        if (scoreCheck != playerScore.getScore())
        {
            theScore.text = "Score: " + playerScore.getScore() + "";
        }
    }
}

