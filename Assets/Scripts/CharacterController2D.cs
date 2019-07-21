using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;

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

    private BoxCollider2D boxCollider;

    public Vector2 velocity;

    public static CharacterController2D self;

    /// <summary>
    /// Set to true when the character intersects a collider beneath
    /// them in the previous frame.
    /// </summary>
    public bool grounded;
    public bool isDead = false;
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
        boxCollider = GetComponent<BoxCollider2D>();
        self = this;
    }

    public void OnCollisionEnter2D(Collision2D hit)
    {
        SpecialHitTypeCheck(hit.collider);
        AngleCollideCheck(hit.collider);
    }

    private void AngleCollideCheck(Collider2D hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            ColliderDistance2D colliderDistance = hit.Distance(boxCollider);
            float angle = Vector2.Angle(colliderDistance.normal, Vector2.up);
            if (angle == 180)
            {
                velocity.y = 0;
            }
            if (angle == 0)
            {
                velocity.y = 0;
                grounded = true;

            }
        }
    }

    void SpecialHitTypeCheck(Collider2D hit)
    {
        if (!GameController.G.isRewinding)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("KillsPlayer"))
            {
                Die();
            }
            else if (hit.gameObject.layer == LayerMask.NameToLayer("Bouncy"))
            {
                float bounceVelocity = hit.GetComponent<Bounciness>().bounciness;
                Bounce(bounceVelocity);
            }
        }
    }

    void Bounce(float bounceVelocity)
    {

        velocity.y = bounceVelocity;
        grounded = false;
    }

    private void Update()
    {
        if (GameController.G.isPlaying)
        {
            if (isDead && GameController.G.isRewinding)
            {
                isDead = false;
                skeletonAnimation.gameObject.SetActive(true);
                DeathObject.SetActive(false);
            }
            else if (!isDead && !GameController.G.isRewinding)
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
                }
                else
                {
                    if (skeletonAnimation.AnimationName != "Jump")
                    {
                        skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
                    }
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
                List<Collider2D> hitList = new List<Collider2D>(hits);
                hitList.Remove(boxCollider);

                foreach (Collider2D hit in hitList)
                {
                    ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                    // Ensure that we are still overlapping this collider.
                    // The overlap may no longer exist due to another intersected collider
                    // pushing us out of this one.
                    if (colliderDistance.isOverlapped)
                    {
                        if (!GameController.G.isRewinding)
                        {
                            transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                        }
                        AngleCollideCheck(hit);
                        SpecialHitTypeCheck(hit);
                    }
                }
                if (hitList.Count == 0)
                {
                    grounded = false;
                }

            }
        }

    }
}
