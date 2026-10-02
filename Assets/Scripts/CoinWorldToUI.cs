using UnityEngine;

public class CoinWorldToUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private RectTransform targetUITransform;
    private int coinValue;

    private Vector2 startScreenPos;
    private Vector2 controlScreenPos;
    private Vector2 targetScreenPos;

    private float duration;
    private float elapsedTime;
    private bool isFlying;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Initialize(Vector3 enemyWorldPosition, RectTransform targetUI, Camera mainCamera, int value = 1, float flyDuration = 1.0f)
    {
        targetUITransform = targetUI;
        coinValue = value;
        duration = flyDuration;

        startScreenPos = mainCamera.WorldToScreenPoint(enemyWorldPosition);
        targetScreenPos = targetUITransform.position;

        Vector2 midPoint = (startScreenPos + targetScreenPos) * 0.5f;
        controlScreenPos = midPoint + new Vector2(Random.Range(-100f, 100f), 150f);

        rectTransform.position = startScreenPos;
        elapsedTime = 0f;
        isFlying = true;
    }

    private void Update()
    {
        if (!isFlying) return;

        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / duration);

        float easedT = EaseInQuad(t);

        rectTransform.position = QuadraticBezier2D(startScreenPos, controlScreenPos, targetScreenPos, easedT);

        if (t >= 1f)
        {
            isFlying = false;

            if (BankUIManager.Instance != null)
            {
                BankUIManager.Instance.OnCoinArrived(coinValue);
            }

            Destroy(gameObject);
        }
    }

    private Vector2 QuadraticBezier2D(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        Vector2 a = Vector2.Lerp(p0, p1, t);
        Vector2 b = Vector2.Lerp(p1, p2, t);

        return Vector2.Lerp(a, b, t);
    }

    private Vector2 QuadraticFast2D(Vector2 p0, Vector2 p1, Vector2 p2, float t)
    {
        float u = 1f - t;
        return u * u * p0 + 2f * u * t * p1 + t * t * p2;
    }

    private float EaseInQuad(float t)
    {
        return t * t;
    }
}