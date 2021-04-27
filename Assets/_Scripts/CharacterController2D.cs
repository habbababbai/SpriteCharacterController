using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float jumpForce = 400f;
    [Range(0, 3f)] [SerializeField] private float movementSmoothing = 0.5f;
    [SerializeField] private bool airControl;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    private float _groundedRadius;
    private bool _isGrounded;
    private Rigidbody2D _rigidbody;
    private bool _isFacingRight = true;
    private Vector3 _velocity = Vector3.zero;

    [SerializeField] private UnityEvent onLandEvent;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundedRadius = GetComponent<SpriteRenderer>().bounds.size.y * 0.01f;
        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, _groundedRadius, groundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _isGrounded = true;
                if (!wasGrounded)
                    onLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool jump)
    {
        if (_isGrounded || airControl)
        {
            Vector3 targetVelocity = new Vector2(move * 10f, _rigidbody.velocity.y);
            _rigidbody.velocity =
                Vector3.SmoothDamp(_rigidbody.velocity, targetVelocity, ref _velocity, movementSmoothing);

            if (move > 0 && !_isFacingRight)
                Flip();
            else if (move < 0 && _isFacingRight)
                Flip();
        }

        if (_isGrounded && jump)
        {
            _isGrounded = false;
            _rigidbody.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void Flip()
    {
        _isFacingRight = !_isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
    
}
