using Spine.Unity;
using UnityEngine;

public class NewCharacterController : SkeletonAnimator
{
    private const float pad = 0.01f;

    private enum HorizInputs { None, Left, Right }

    private BoxCollider2D _boxCollider;
    public Rigidbody2D _rigidbody;
    private SkeletonAnimator _skeletonAnimator;

    private HorizInputs _currentHorizInput = HorizInputs.None;

    private bool _jumpButtonInput = false;
    private float _jumpButtonInputDuration = 0f;


    public float timeSinceDeath;
    public bool isDead = false;
    public GameObject DeathObject;
    public GameObject DeathCanvas;
    public GameObject PauseCanvas;

    new void Awake()
    {
        PauseCanvas.SetActive(false);
        DeathCanvas.SetActive(false);
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _skeletonAnimator = GetComponent<SkeletonAnimator>();
        base.Awake();
    }

    bool prevPause;
    new void Update()
    {
        if (GameController.G.isPlaying && !GameController.G.isPaused && !prevPause)
        {
            if (isDead && !GameController.G.isRewinding)
            {
                timeSinceDeath += Time.deltaTime;
            }
            if (isDead && GameController.G.isRewinding)
            {
                timeSinceDeath -= Time.deltaTime;
                if (timeSinceDeath < 0)
                {
                    Revive();
                }
            }
            else if (!isDead && !GameController.G.isRewinding)
            {
                // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
                float moveInput = Input.GetAxis("Horizontal");

                if (moveInput > 0.05f)
                {
                    _currentHorizInput = HorizInputs.Right;
                }
                else if(moveInput < -0.05f)
                {
                    _currentHorizInput = HorizInputs.Left;
                }
                else
                {
                    _currentHorizInput = HorizInputs.None;
                }

                if (Input.GetButton("Jump"))
                {
                    _jumpButtonInput = true;
                }
                else
                {
                    _jumpButtonInput = false;
                    _jumpButtonInputDuration = 0f;
                }

                HorizontalFlipGraphics();
                SwitchAnimations();
            }
        }

        prevPause = GameController.G.isPaused;
        base.Update();
    }

    private void HorizontalFlipGraphics()
    {
        if (_currentHorizInput == HorizInputs.Left)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;
        }
        else if (_currentHorizInput == HorizInputs.Right)
        {
            Vector3 newScale = transform.localScale;
            newScale.x = -1;
            transform.localScale = newScale;
        }
    }
    private void SwitchAnimations()
    {
        var skeletonAnimation = _skeletonAnimator.skeletonAnimation;
        bool isGrounded = IsGrounded();

        if (!isGrounded)
        {
            if (skeletonAnimation.AnimationName != "Jump")
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Jump", true);
            }
        }
        else if (_rigidbody.velocity.x == 0)
        {
            if (skeletonAnimation.AnimationName != "Idle")
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "Idle", true);
            }
        }
        else if (skeletonAnimation.AnimationName != "Run2")
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "Run2", true);
        }
    }

    private void FixedUpdate()
    {
        if (GameController.G.isPlaying && !GameController.G.isPaused)
        {
            Vector2 velocity = _rigidbody.velocity;

            velocity.x = AdjustHorizontalVelocity(velocity.x);
            velocity.y = AdjustVerticalVelocity(velocity.y);

            _rigidbody.velocity = velocity;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
        }
    }

    private float AdjustHorizontalVelocity(float inputVelocity)
    {
        const float MaxVelocity = 15f;
        const float Acceleration = MaxVelocity * 20;
        const float AirAcceleration = 80f;

        float desiredVelocity;

        if (_currentHorizInput == HorizInputs.Left)
        {
            desiredVelocity = -MaxVelocity;
        }
        else if (_currentHorizInput == HorizInputs.Right)
        {
            desiredVelocity = MaxVelocity;
        }
        else
        {
            desiredVelocity = 0;
        }
        bool grounded = IsGrounded();
        if (grounded)
        {
            return Mathf.MoveTowards(inputVelocity, desiredVelocity, Acceleration * Time.fixedDeltaTime);
        }
        else
        {
            return Mathf.MoveTowards(inputVelocity, desiredVelocity, AirAcceleration * Time.fixedDeltaTime);
        }
    }

    private float AdjustVerticalVelocity(float inputVelocity)
    {
        const float MaxGravVelocity = 50f;
        const float gravity = 150f;
        const float jumpHeight = 35f;
        bool isGrounded = IsGrounded();
        bool hitUp = _rigidbody.velocity.y < 0;
        bool jumpHasStarted = _jumpButtonInputDuration > 0;
        bool canJumpUpdate = !hitUp && (jumpHasStarted || isGrounded);

        if (CastUp())
        {
            if (isGrounded)
            {
                return 0f;
            }

            _jumpButtonInputDuration = 999f;
            return Mathf.MoveTowards(Mathf.Min(0f, inputVelocity), MaxGravVelocity * -1, gravity * Time.fixedDeltaTime);
        }
        else if (canJumpUpdate && _jumpButtonInput)
        {
            //Apply jump vel
            _jumpButtonInputDuration += Time.fixedDeltaTime;

            if (_jumpButtonInputDuration < .1f)
            {
                return jumpHeight;
            }
        }

        if (isGrounded)
        {
            return 0f;
        }
        else
        {
            //Apply gravity
            return Mathf.MoveTowards(inputVelocity, MaxGravVelocity * -1, gravity * Time.fixedDeltaTime);
        }
    }

    private bool CastUp()
    {
        // Hit anything other than the player
        var layerMask = ~LayerMask.GetMask("Player");

        var boxCenter = (Vector2)_boxCollider.bounds.center;
        var boxSize = (Vector2)_boxCollider.bounds.size;

        Vector2 origin = boxCenter;
        origin.y += boxSize.y / 2f + pad / 2f;

        Vector2 size = new Vector2(boxSize.x / 2f - pad, pad);

        return Physics2D.OverlapBox(origin, size, 0f, groundMaskCheck);
    }

    public LayerMask groundMaskCheck;
    private bool IsGrounded()
    {
        var boxCenter = (Vector2)_boxCollider.bounds.center;
        var boxSize = (Vector2)_boxCollider.bounds.size;

        Vector2 origin = boxCenter;
        origin.y -= boxSize.y / 2f ;

        Vector2 size = new Vector2(boxSize.x / 2f, pad);

        return Physics2D.OverlapBox(origin, size, 0f, groundMaskCheck);
    }


    public void Die()
    {
        if (isDead != true)
        {
            isDead = true;
            skeletonAnimation.gameObject.SetActive(false);
            DeathObject.SetActive(true);
            DeathCanvas.SetActive(true);
            var deathObject = DeathObject.GetComponent<SkeletonAnimation>();

            _rigidbody.velocity = Vector2.zero;

            Vector3 newScale = transform.localScale;
            newScale.x = 1;
            transform.localScale = newScale;

            var deathAnim = deathObject.AnimationState.SetAnimation(0, "Splat", false);
            deathAnim.TrackTime = 0;
            GameController.G.DeathPause();
        }
    }

    public void Revive()
    {
        isDead = false;
        GameController.G.isPlaying = true;
        GameController.G.isDeathPaused = false;
        DeathObject.SetActive(false);
        DeathCanvas.SetActive(false);
        skeletonAnimation.gameObject.SetActive(true);
    }


    public void OnTriggerEnter2D(Collider2D hit)
    {
        SpecialHitTypeCheck(hit);
    }

    public void OnTriggerStay2D(Collider2D hit)
    {
        SpecialHitTypeCheck(hit);
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
            } else if(hit.gameObject.layer == LayerMask.NameToLayer("Ending"))
            {
                hit.GetComponent<EndGameObject>().DoEnding();
            }
        }
    }

    void Bounce(float bounceVelocity)
    {
        Vector2 newVel = _rigidbody.velocity;
        newVel.y = bounceVelocity;
        _rigidbody.velocity = newVel;
    }

}
