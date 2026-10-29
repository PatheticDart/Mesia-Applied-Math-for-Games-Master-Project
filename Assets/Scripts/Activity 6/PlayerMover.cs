using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AABBCollider))]
public class PlayerMover : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 12f;
    [SerializeField] private float _gravity = 25f;

    [Header("Finish UI")]
    [SerializeField] private GameObject finishUI;

    private Vector2 _velocity;
    private bool _isGrounded;
    private AABBCollider _myCollider;

    private void Awake()
    {
        _myCollider = GetComponent<AABBCollider>();
    }

    void Start()
    {
        if (finishUI == null)
        {
            finishUI = GameObject.Find("Finish UI");
        }
        finishUI.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        _velocity.x = inputX * _moveSpeed;

        if (Input.GetButton("Jump") && _isGrounded)
        {
            _velocity.y = _jumpForce;
            _isGrounded = false;
            Debug.Log("Player has jumped.");
        }

        if (!_isGrounded)
        {
            _velocity.y -= _gravity * Time.deltaTime;
        }

        MoveAndResolveCollisions();
    }

    private void MoveAndResolveCollisions()
    {
        _isGrounded = false;

        transform.position += new Vector3(_velocity.x * Time.deltaTime, 0f, 0f);
        UpdateColliderBounds();

        foreach (var other in CollisionManager.Instance.Colliders)
        {
            if (other == _myCollider) continue;

            if (CollisionManager.Overlaps(_myCollider, other))
            {
                HandleCollision(other, isHorizontalAxis: true);
            }
        }

        transform.position += new Vector3(0f, _velocity.y * Time.deltaTime, 0f);
        UpdateColliderBounds();

        foreach (var other in CollisionManager.Instance.Colliders)
        {
            if (other == _myCollider) continue;

            if (CollisionManager.Overlaps(_myCollider, other))
            {
                HandleCollision(other, isHorizontalAxis: false);
            }
        }
    }

    private void HandleCollision(AABBCollider other, bool isHorizontalAxis)
    {
        if (other.Tag == ColliderTag.FallZone)
        {
            RestartGame();
            return;
        }

        if (other.Tag == ColliderTag.FinishZone)
        {
            finishUI.SetActive(true);
            Time.timeScale = 0f;
            return;
        }

        if (other.Tag == ColliderTag.Ground || other.Tag == ColliderTag.Obstacle)
        {
            if (isHorizontalAxis)
            {
                if (_velocity.x > 0)
                {
                    float overlap = _myCollider.Max.x - other.Min.x;
                    transform.position -= new Vector3(overlap, 0f, 0f);
                }
                else if (_velocity.x < 0)
                {
                    float overlap = other.Max.x - _myCollider.Min.x;
                    transform.position += new Vector3(overlap, 0f, 0f);
                }
                _velocity.x = 0;
            }
            else
            {
                if (_velocity.y < 0)
                {
                    float overlap = _myCollider.Min.y - other.Max.y;
                    transform.position -= new Vector3(0f, overlap, 0f);
                    _velocity.y = 0;
                    if (other.Tag == ColliderTag.Ground)
                    {
                        _isGrounded = true;
                    }
                    Debug.Log("Player has landed on the ground or obstacle.");
                }
                else if (_velocity.y > 0)
                {
                    float overlap = other.Min.y - _myCollider.Max.y;
                    transform.position += new Vector3(0f, overlap, 0f);
                    _velocity.y = 0;
                }
            }
            UpdateColliderBounds();
        }
    }

    private void UpdateColliderBounds()
    {
        Vector3 pos = transform.position;
        float halfW = _myCollider.Width * 0.5f;
        float halfL = _myCollider.Length * 0.5f;

        _myCollider.GetType()
                   .GetProperty("Max")
                   ?.SetValue(_myCollider, new Vector2(pos.x + halfW, pos.y + halfL));
        _myCollider.GetType()
                   .GetProperty("Min")
                   ?.SetValue(_myCollider, new Vector2(pos.x - halfW, pos.y - halfL));
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}