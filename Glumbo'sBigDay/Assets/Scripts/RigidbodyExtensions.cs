using System.Collections;
using UnityEngine;

public class RigidbodyExtensions : MonoBehaviour
{
    // Public properties for external access
    public bool IsLaunched { get; private set; } = false;
    public float LaunchForce = 20f;
    public float MaxDistance = 30f;
    public float ReturnSpeed = 15f;
    public float Lifetime = 10f; // Time before the projectile is destroyed automatically

    // Reference to the player
    [SerializeField]
    private Transform playerTransform;

    // Internal variables
    private Rigidbody rb;
    private Animator animator;
    private Vector3 initialPosition;
    private bool isReturning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (playerTransform == null)
        {
            // Attempt to find the player by tag
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player GameObject with tag 'Player' not found!");
            }
        }

        // Start coroutine to destroy the projectile after its lifetime
        StartCoroutine(DestroyAfterTime());

        // Ensure the projectile starts in the Idle state
        if (animator != null)
        {
            animator.Play("Idle"); // Replace "Idle" with your actual idle state name
        }
    }

    void FixedUpdate()
    {
        if (IsLaunched && !isReturning)
        {
            float distance = Vector3.Distance(transform.position, initialPosition);
            if (distance >= MaxDistance)
            {
                StartReturn();
            }
        }

        if (isReturning)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer <= 1f) // Threshold to consider it has returned
            {
                StopProjectile();
            }
            else
            {
                Vector3 direction = (playerTransform.position - transform.position).normalized;
                rb.velocity = direction * ReturnSpeed;
            }
        }
    }

    /// <summary>
    /// Launches the projectile in the specified direction with the given force.
    /// </summary>
    /// <param name="direction">Direction to launch the projectile.</param>
    /// <param name="force">Force to apply.</param>
    public void Launch(Vector3 direction, float force)
    {
        if (IsLaunched)
        {
            Debug.LogWarning("Projectile is already launched!");
            return;
        }

        rb.velocity = Vector3.zero; // Reset any existing velocity
        rb.AddForce(direction.normalized * force, ForceMode.VelocityChange);
        IsLaunched = true;
        initialPosition = playerTransform.position;
        isReturning = false;

        // Trigger the launch animation
        if (animator != null)
        {
            animator.SetTrigger("Launch"); // Ensure you have a trigger named "Launch" in your Animator
        }

        // Optional: Unparent the projectile if it was parented to the player
        transform.parent = null;
    }

    /// <summary>
    /// Initiates the return sequence of the projectile.
    /// </summary>
    private void StartReturn()
    {
        isReturning = true;

        // Trigger the return animation
        if (animator != null)
        {
            animator.SetTrigger("Return"); // Ensure you have a trigger named "Return" in your Animator
        }
    }

    /// <summary>
    /// Stops the projectile and resets its state.
    /// </summary>
    private void StopProjectile()
    {
        rb.velocity = Vector3.zero;
        rb.position = playerTransform.position; // Snap to player position
        IsLaunched = false;
        isReturning = false;

        // Trigger the idle animation upon return
        if (animator != null)
        {
            animator.Play("Idle"); // Replace "Idle" with your actual idle state name
        }

        // Optional: Re-parent the projectile to the player
        transform.parent = playerTransform;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (IsLaunched && !isReturning)
        {
            // Start returning upon collision
            StartReturn();

            // Optionally, trigger impact animations or effects here
            if (animator != null)
            {
                animator.SetTrigger("Impact"); // Ensure you have a trigger named "Impact" in your Animator
            }
        }
    }

    IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(Lifetime);
        Destroy(gameObject);
    }
}
