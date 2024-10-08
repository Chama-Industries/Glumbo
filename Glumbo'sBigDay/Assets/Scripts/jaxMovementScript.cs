using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;

public class jaxMovementScript : MonoBehaviour
{
    //controls the speed of the player
    private float speed = 10f;
    private float rotateSpeed = 180f;
    private Vector3 jumpPower = new Vector3(0, 8.75f, 0);
    private Vector3 tempDiagonalBoost = new Vector3(0, 7.5f, 0);
    private Vector3 tempVerticalBoost = new Vector3(0, 12.5f, 0);
    //rigidbody
    Rigidbody rb;
    //temporary variable to hold a reference to the player
    public GameObject player;
    //variable to reference a projectile for the player to use to attack
    public GameObject glumboAttack;
    //int to give a cooldown so whatever action can't be spammed
    private int tempAttackCooldown;
    private int tempJumpCooldown;
    //variable to reference where the projectile will come from
    public Transform attackOrigin;
    //3D movement variable
    Vector3 movementD;
    //assignments for additional inputs, holds the keys we're using to give the player control
    public KeyCode jump = KeyCode.Space;
    public KeyCode dance = KeyCode.Q;
    //public KeyCode secondaryKey = KeyCode.E;
    [Header("Projectile Orbit Settings")]
    [Tooltip("Maximum number of orbiting projectiles allowed.")]
    public int maxOrbitProjectiles = 1;

    // List to keep track of active orbiting projectiles
    private List<GameObject> activeOrbitProjectiles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //assigns player speed
        speed = 25;
        //staple grab of a physics simulation component
        rb = GetComponent<Rigidbody>();
        //relvenant keys that do things when pressed
    }

    // Update is called once per frame
    void Update()
    {
        playerMove();
        advancedPlayerActions();
    }

    //50 calls per seconds, helps with a little cooldown on jump and attacking (VERY TEMPORARY)
    void FixedUpdate()
    {
        tempAttackCooldown++;
        tempJumpCooldown++;
        additionalPlayerActions();
    }

    //has basic WASD controls for the player
    void playerMove()
    {
        float hIn = Input.GetAxis("Horizontal");
        float vIn = Input.GetAxis("Vertical");

        //for some reason it was flipped (likely due to how i have the camera), so i had to toy with the variables 
        movementD = new Vector3(vIn, 0, -hIn);
        movementD.Normalize();

        transform.Translate(movementD * speed * Time.deltaTime, Space.World);

        //makes the player rotate in the direciton of movement
        if (movementD != Vector3.zero)
        {
            Quaternion rotationD = Quaternion.LookRotation(movementD, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotationD, rotateSpeed * Time.deltaTime);
        }
    }

    //includes additional controls, such as Jumping, Dancing, and a secondary Button for other actions
    void additionalPlayerActions()
    {
        //very temporary code to allow the player to jump
        if(Input.GetKey(jump) && tempJumpCooldown > 75)
        {
            rb.AddForce(jumpPower, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }
        //temporary code to communicate that dance is working (would play an animation)
        if (Input.GetKey(dance))
        {
            player.transform.Rotate(0.0f, 0.25f, 0.0f);
        }
        //another key to use
        /*if (Input.GetKey(secondaryInput))
        {
            
        }*/
        //attacking input
        if(Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            Instantiate(glumboAttack, attackOrigin.position, attackOrigin.rotation);
            tempAttackCooldown = 0;
        }
    }

    //combo moves by using multiple buttons at once, would have a cooldown but this is an alpha build
    void advancedPlayerActions()
    {
        //gives the player a vertical & horizontal movement boost
        if(Input.GetMouseButton(0) && Input.GetKey(jump) && tempJumpCooldown > 70)
        {
            rb.AddForce(tempDiagonalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        //gives the player a bigger jump
        if(Input.GetKey(jump) && Input.GetKey(dance) && tempJumpCooldown > 70)
        {
            rb.AddForce(tempVerticalBoost, ForceMode.VelocityChange);
            tempJumpCooldown = 0;
        }

        //attacking input for orbit attack
        if(Input.GetKey(dance) && Input.GetMouseButton(0) && tempAttackCooldown > 50)
        {
            if (activeOrbitProjectiles.Count < maxOrbitProjectiles){
                SpawnOrbitProjectile();
                tempAttackCooldown = 0;
            }
        }
    }

    void SpawnOrbitProjectile()
    {
        if (glumboAttack == null)
        {
            Debug.LogWarning("jaxMovementScript: glumboAttack prefab not assigned.");
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

        //Quaternion.identity
        // Instantiate the orbiting projectile at the attackOrigin position
        GameObject projectile = Instantiate(glumboAttack, attackOrigin.position, attackOrigin.rotation);

        // Get the orbitingProjectileMovement component
        orbitingProjectileMovement orbitScript = projectile.GetComponent<orbitingProjectileMovement>();
        if (orbitScript != null)
        {
            // Assign the orbit center to the player's Transform
            orbitScript.orbitCenter = player.transform;

            // Optionally, set other parameters like speed and radius if exposed
            // orbitScript.orbitSpeed = 50f;
            // orbitScript.orbitRadius = 5f;

            // Add to the active projectiles list
            activeOrbitProjectiles.Add(projectile);
        }
        else
        {
            Debug.LogError("jaxMovementScript: glumboAttack prefab does not have an orbitingProjectileMovement component.");
            Destroy(projectile);
        }
    }
}
