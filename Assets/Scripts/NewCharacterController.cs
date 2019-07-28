using UnityEngine;

public class NewCharacterController : MonoBehaviour
{
    private const float pad = 0.01f;

    private enum HorizInputs { None, Left, Right }

    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody;
    private SkeletonAnimator _skeletonAnimator;

    private HorizInputs _currentHorizInput = HorizInputs.None;

    private bool _jumpButtonInput = false;
    private float _jumpButtonInputDuration = 0f;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _skeletonAnimator = GetComponent<SkeletonAnimator>();
    }

    private void Update()
    {
        // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
        float moveInput = Input.GetAxis("Horizontal");

        if (Mathf.Approximately(moveInput, 0))
        {
            _currentHorizInput = HorizInputs.None;
        }
        else if (moveInput > 0)
        {
            _currentHorizInput = HorizInputs.Right;
        }
        else
        {
            _currentHorizInput = HorizInputs.Left;
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
        Vector2 velocity = _rigidbody.velocity;

        velocity.x = AdjustHorizontalVelocity(velocity.x);
        velocity.y = AdjustVerticalVelocity(velocity.y);

        _rigidbody.velocity = velocity;
    }

    private float AdjustHorizontalVelocity(float inputVelocity)
    {
        const float Acceleration = 60f;
        const float MaxVelocity = 12f;

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

        return Mathf.MoveTowards(inputVelocity, desiredVelocity, Acceleration * Time.fixedDeltaTime);
    }

    private float AdjustVerticalVelocity(float inputVelocity)
    {
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
            return Mathf.MoveTowards(Mathf.Min(0f, inputVelocity), -16f, 60f * Time.fixedDeltaTime);
        }
        else if (canJumpUpdate && _jumpButtonInput)
        {
            _jumpButtonInputDuration += Time.fixedDeltaTime;

            if (_jumpButtonInputDuration < .2f)
            {
                return 16f;
            }
        }

        if (isGrounded)
        {
            return 0f;
        }
        else
        {
            return Mathf.MoveTowards(inputVelocity, -16f, 60f * Time.fixedDeltaTime);
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

        return Physics2D.OverlapBox(origin, size, 0f, layerMask);
    }

    private bool IsGrounded()
    {
        // Hit anything other than the player
        var layerMask = ~LayerMask.GetMask("Player");

        var boxCenter = (Vector2)_boxCollider.bounds.center;
        var boxSize = (Vector2)_boxCollider.bounds.size;

        Vector2 origin = boxCenter;
        origin.y -= boxSize.y / 2f - pad / 2f;

        Vector2 size = new Vector2(boxSize.x / 2f - pad, pad);

        return Physics2D.OverlapBox(origin, size, 0f, layerMask);
    }
}
