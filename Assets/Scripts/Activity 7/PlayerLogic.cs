using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLogic : MonoBehaviour
{
    [Header("Health & UI")]
    public float hp = 100f;
    public float maxHp = 100f;
    public float score = 0f;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverScoreText;
    public GameObject gameOverPanel;

    [Header("Sprites")]
    public Sprite defaultSprite;
    public Sprite playerShipLeft;
    public Sprite playerShipRight;
    public Sprite playerShipHit;
    private SpriteRenderer spriteRenderer;
    private float hitTimer = 0f;

    [Header("Spatial Coordinates")]
    public Vector3 worldPosition = new Vector3(0, 0, 0);
    public float laneWidth = 2f;
    public int currentLane = 2;
    private float targetX = 0f;

    [Header("Jump Mechanics")]
    private bool isJumping = false;
    public float jumpHeight = 1f;
    private float jumpTime = 0f;
    public float jumpDuration = 1f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && defaultSprite == null)
        {
            defaultSprite = spriteRenderer.sprite;
        }

        if (gameOverScoreText == null)
        {
            gameOverScoreText = GameObject.Find("GameOverScoreText").GetComponent<TextMeshProUGUI>();
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        else
        {
            gameOverPanel = GameObject.Find("GameOverPanel");
        }

        if (scoreText == null)
        {
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        }

        if (hpText == null)
        {
            hpText = GameObject.Find("HPText").GetComponent<TextMeshProUGUI>();
        }

        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 1) currentLane--;
        if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 3) currentLane++;

        targetX = (currentLane - 2) * laneWidth;

        worldPosition.x = Mathf.Lerp(worldPosition.x, targetX, 10f * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.UpArrow) && !isJumping)
        {
            isJumping = true;
            jumpTime = 0f;
        }

        if (isJumping)
        {
            jumpTime += Time.deltaTime;
            float jumpProgress = jumpTime / jumpDuration;
            worldPosition.y = Mathf.Sin(jumpProgress * Mathf.PI) * jumpHeight;

            if (jumpTime >= jumpDuration)
            {
                isJumping = false;
                worldPosition.y = 0f;
            }
        }

        float fl = CameraComponent.focalLength;
        float perspective = fl / (fl + worldPosition.z);

        transform.localScale = Vector3.one * perspective;
        transform.position = new Vector2(worldPosition.x, worldPosition.y) * perspective;

        UpdatePlayerSprite();

        score += 1f * Time.deltaTime;
        scoreText.text = "SCORE: " + Mathf.Round(score).ToString();

        hpText.text = "HULL INTEGRITY: " + Mathf.Round(hp).ToString();

        if (hp <= 0f)
        {
            gameOverPanel.SetActive(true);
            gameOverScoreText.text = "SCORE: " + Mathf.Round(score).ToString();
            Time.timeScale = 0f;
        }

    }

    private void UpdatePlayerSprite()
    {
        if (spriteRenderer == null) return;

        if (hitTimer > 0f)
        {
            hitTimer -= Time.deltaTime;
            spriteRenderer.sprite = playerShipHit;
            spriteRenderer.color = Color.red;
            return;
        }

        if (currentLane == 1)
        {
            spriteRenderer.sprite = playerShipLeft;
        }
        else if (currentLane == 3)
        {
            spriteRenderer.sprite = playerShipRight;
        }
        else
        {
            spriteRenderer.sprite = defaultSprite;
        }

        spriteRenderer.color = Color.white;
    }

    public void TakeDamage(float damage)
    {
        hp = Mathf.Max(0f, hp - damage);
        hitTimer = 0.3f;
        if (spriteRenderer != null && playerShipHit != null)
        {
            spriteRenderer.sprite = playerShipHit;
        }
    }

    public void Heal(float amount)
    {
        hp = Mathf.Min(maxHp, hp + amount);
    }
}