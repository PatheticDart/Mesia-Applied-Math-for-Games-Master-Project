using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public SceneHandler sceneHandler = GameObject.Find("SceneHandler").GetComponent<SceneHandler>();
    public int maxHealth = 5;
    public float alertRange = 10f;
    private int currentHealth;

    public TMPro.TextMeshProUGUI healthText;
    public GameObject alertText;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void Update()
    {
        MissileDetection();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = currentHealth.ToString();
        }
    }
    public void Damage(int damageAmount)
    {
        PlayerUIColor playerUIColor = GetComponent<PlayerUIColor>();
        playerUIColor.TriggerDamage();
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            sceneHandler.RestartScene();
        }
    }

    public void MissileDetection()
    {
        foreach (HomingMissileLogic missile in HomingMissileLogic.ActiveMissiles)
        {
            float distanceToMissile = Vector3.Distance(transform.position, missile.transform.position);
            if (distanceToMissile <= alertRange)
            {
                Alert(true);
                return;
            }
        }
        Alert(false);
    }

    public void Alert(bool isAlerting)
    {
        PlayerUIColor playerUIColor = GetComponent<PlayerUIColor>();
        if (isAlerting)
        {
            alertText.SetActive(true);
            playerUIColor.Alert(true);
        }
        else
        {
            alertText.SetActive(false);
            playerUIColor.Alert(false);
        }
    }
}
