using TMPro;
using UnityEngine;

public class BankUIManager : MonoBehaviour
{
    public static BankUIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private RectTransform coinIconOrTextContainer;

    [Header("Punch Settings (Using Ease Functions from PPTX)")]
    [SerializeField] private float punchDuration = 0.4f;
    [SerializeField] private Vector3 punchScaleTarget = new Vector3(1.3f, 1.3f, 1.3f); // 30% pop

    [Header("Number Counter Settings")]
    [SerializeField] private float counterDuration = 0.5f;

    private Vector3 originalScale;

    // Punch Animation Variables
    private float punchElapsedTime;
    private bool isPunching;

    // Rolling Counter Variables
    private float counterElapsedTime;
    private bool isCounting;
    private int startCoins;
    private int targetCoins;
    private int displayedCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (coinIconOrTextContainer != null)
        {
            originalScale = coinIconOrTextContainer.localScale;
        }
    }

    private void Update()
    {
        if (isPunching)
        {
            punchElapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(punchElapsedTime / punchDuration);

            float easedT = EaseOutElastic(t);

            if (coinIconOrTextContainer != null)
            {
                coinIconOrTextContainer.localScale = Vector3.Lerp(originalScale, punchScaleTarget, easedT);
            }

            if (t >= 1f)
            {
                isPunching = false;
                if (coinIconOrTextContainer != null)
                {
                    coinIconOrTextContainer.localScale = originalScale;
                }
            }
        }

        if (isCounting)
        {
            counterElapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(counterElapsedTime / counterDuration);

            float easedT = EaseOutQuad(t);

            float lerpedVal = Mathf.Lerp(startCoins, targetCoins, easedT);
            displayedCoins = (int)lerpedVal;

            UpdateText();

            if (t >= 1f)
            {
                isCounting = false;
                displayedCoins = targetCoins;
                UpdateText();
            }
        }
    }

    public void OnCoinArrived(int value)
    {
        startCoins = displayedCoins;
        targetCoins += value;
        counterElapsedTime = 0f;
        isCounting = true;

        punchElapsedTime = 0f;
        isPunching = true;
    }

    public void SetCoinsInstant(int initialCoins)
    {
        targetCoins = initialCoins;
        startCoins = initialCoins;
        displayedCoins = initialCoins;
        UpdateText();
    }

    private void UpdateText()
    {
        if (coinText != null)
        {
            coinText.text = displayedCoins.ToString();
        }
    }

    private float EaseOutElastic(float t)
    {
        const float c4 = (2f * Mathf.PI) / 3f;
        return t == 0f ? 0f : t == 1f ? 1f : Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * 10f - 0.75f) * c4) + 1f;
    }

    private float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }
}