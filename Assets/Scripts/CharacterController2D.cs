using UnityEngine;
using Spine.Unity;

[RequireComponent(typeof(BoxCollider2D))]
public class CharacterController2D : MonoBehaviour
{
    [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
    float speed = 9;

    [SerializeField, Tooltip("Acceleration while grounded.")]
    float walkAcceleration = 75;

    [SerializeField, Tooltip("Acceleration while in the air.")]
    float airAcceleration = 30;

    [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
    float groundDeceleration = 70;

    [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
    float jumpHeight = 4;


    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 2f;

    private CapsuleCollider2D boxCollider;

    public Vector2 velocity;

    public static CharacterController2D self;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    public bool grounded;
    public bool isRunning = false;
    public bool isDead = false;
    public float bounceVelocity = 10.00F;
    TimeController myTime;
    public SkeletonAnimation skeletonAnimation;
    public GameObject DeathObject;


    public void Die()
    {
        if (isDead != true)
        {
            isDead = true;
            skeletonAnimation.gameObject.SetActive(false);
            DeathObject.SetActive(true);
            var deathObject = DeathObject.GetComponent<SkeletonAnimation>();
            var deathAnim = deathObject.AnimationState.SetAnimation(0, "Splat", false);
            deathAnim.TrackTime = 0;
        }
    }

    private void Awake()
    {
        myTime = GetComponent<TimeController>();
        boxCollider = GetComponent<CapsuleCollider2D>();
        self = this;
    }

    public void OnCollisionEnter2D(Collision2D hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("KillsPlayer") && !myTime.isRewinding)
        {
            Die();
        }
        else
        {
            //Ceiling Check
            ColliderDistance2D colliderDistance = hit.collider.Distance(boxCollider);
            if (Vector2.Angle(colliderDistance.normal, Vector2.up) > 90)
            {
                velocity.y = 0;
            }
        }
    }

    private void Update()
    {
        if (isRunning)
        {
            if (isDead && (Input.GetButton("Rewind")))
            {
                isDead = false;
                skeletonAnimation.gameObject.SetActive(true);
                DeathObject.SetActive(false);
            }
            else if (!isDead)
            {
                // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
                float moveInput = Input.GetAxis("Horizontal");
                if (velocity.x > 0)
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = -1;
                    transform.localScale = newScale;
                }
                else if (velocity.x < 0)
                {
                    Vector3 newScale = transform.localScale;
                    newScale.x = 1;
                    transform.localScale = newScale;
                }

                //If we're on the ground
                if (grounded)
                {
                    //We should not have a y velocity
                    velocity.y = 0;

                    string curAnim = skeletonAnimation.AnimationName;

                    //If we're not moving and we're on the ground, we're Idling
                    if (velocity.x == 0)
                    {
                        if (curAnim != "Idle")
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
                        }
                    }
                    else
                    {
                        //Otherwise, we're running
                        if (curAnim != "Run2")
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0, "Run2", true);
                        }
                    }

                    if (Input.GetButtonDown("Jump"))
                    {
                        grounded = false;
                        if (skeletonAnimation.AnimationName != "Jump")
                        {
                            skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
                        }
                        // Calculate the velocity required to achieve the target jump height.
                        velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y * 2));

                    }
                }
                else
                {
                    if (skeletonAnimation.AnimationName != "Jump")
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
                    }
                }

                if (Input.GetButtonUp("Jump"))
                {
                    velocity.y = velocity.y * .75f;
                }

                if (velocity.y < 0)
                {
                    velocity += Vector2.up * Physics2D.gravity.y * 2 * (fallMultiplier - 1) * Time.deltaTime;
                }
                else if (velocity.y > 0 && !Input.GetButton("Jump"))
                {
                    velocity += Vector2.up * Physics2D.gravity.y * 2 * (lowJumpMultiplier - 1) * Time.deltaTime;
                }

                float acceleration = grounded ? walkAcceleration : airAcceleration;
                float deceleration = grounded ? groundDeceleration : 0;

                if (moveInput != 0)
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
                }
                else
                {
                    velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
                }
                if (!grounded)
                {
                    velocity.y += Physics2D.gravity.y * 2 * Time.deltaTime;
                }

                transform.Translate(velocity * Time.deltaTime);

                // Retrieve all colliders we have intersected after velocity has been applied.
                Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size, 0);

                foreach (Collider2D hit in hits)
                {
                    // Ignore our own collider.
                    if (hit == boxCollider)
                        continue;

                    ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                    // Ensure that we are still overlapping this collider.
                    // The overlap may no longer exist due to another intersected collider
                    // pushing us out of this one.
                    if (colliderDistance.isOverlapped)
                    {
                        transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                        // If we intersect an object beneath us, set grounded to true. 
                        if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0)
                        {
                            grounded = true;
                        }
                    }


                    // Change the y axis velocity when colliding with objects which have the name of Bounce
                    // The law of gravity still applies.
                    if (hit.name == "Bounce")
                    {
                        velocity.y = bounceVelocity;
                    }

                }
                if (hits.Length == 0)
                {
                    grounded = false;
                }

            }
        }

    }
}
