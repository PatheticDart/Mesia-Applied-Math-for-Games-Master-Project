using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIColor : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text healthCounterText;
    [SerializeField] private TMP_Text damagedText;
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image image4;

    [Header("Color Settings")]
    [SerializeField] private Color alertColor = Color.orange;
    [SerializeField] private Color damagedColor = Color.red;

    [Header("Linger Settings")]
    [SerializeField] private float damageLingerDuration = 0.3f;

    private Color originalTextColor;
    private Color originalText2Color;
    private Color originalImage1Color;
    private Color originalImage2Color;
    private Color originalImage3Color;
    private Color originalImage4Color;

    private float damageTimer = 0f;
    private bool isCurrentlyAlerting = false;

    private void Awake()
    {
        if (healthText != null) originalTextColor = healthText.color;
        if (healthCounterText != null) originalText2Color = healthCounterText.color;
        if (image1 != null) originalImage1Color = image1.color;
        if (image2 != null) originalImage2Color = image2.color;
        if (image3 != null) originalImage3Color = image3.color;
        if (image4 != null) originalImage4Color = image4.color;

        if (damagedText != null)
        {
            damagedText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;

            if (damageTimer <= 0f)
            {
                if (damagedText != null)
                {
                    damagedText.gameObject.SetActive(false);
                }

                RefreshUIColors();
            }
        }
    }
    public void Alert(bool isAlerting)
    {
        isCurrentlyAlerting = isAlerting;

        if (damageTimer <= 0f)
        {
            RefreshUIColors();
        }
    }

    public void TriggerDamage()
    {
        damageTimer = damageLingerDuration;

        if (damagedText != null)
        {
            damagedText.gameObject.SetActive(true);
            damagedText.color = damagedColor;
        }

        ApplyColors(damagedColor, damagedColor, damagedColor);
    }

    private void RefreshUIColors()
    {
        if (isCurrentlyAlerting)
        {
            ApplyColors(alertColor, alertColor, alertColor);
        }
        else
        {
            RevertImageColors();
        }
    }

    private void ApplyColors(Color textColor, Color text2Color, Color imageColor)
    {
        if (healthText != null) healthText.color = textColor;
        if (healthCounterText != null) healthCounterText.color = text2Color;

        if (image1 != null) image1.color = imageColor;
        if (image2 != null) image2.color = imageColor;
        if (image3 != null) image3.color = imageColor;
        if (image4 != null) image4.color = imageColor;
    }

    private void RevertImageColors()
    {
        if (healthText != null) healthText.color = originalTextColor;
        if (healthCounterText != null) healthCounterText.color = originalText2Color;

        if (image1 != null) image1.color = originalImage1Color;
        if (image2 != null) image2.color = originalImage2Color;
        if (image3 != null) image3.color = originalImage3Color;
        if (image4 != null) image4.color = originalImage4Color;
    }
}