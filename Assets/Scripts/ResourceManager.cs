using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int coinAmount = 0;
    public int playerHealth = 20;
    public TMP_Text coinText;
    public TMP_Text healthText;
    public GameObject gameOverScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Time.timeScale = 1f;
        gameOverScreen.SetActive(false);

        Instance = this;
    }

    public void Start()
    {
        coinText.text = coinAmount.ToString();
        healthText.text = playerHealth.ToString();
    }

    public void AddCoin(int amount)
    {
        coinAmount += amount;
        coinText.text = coinAmount.ToString();
    }

    public void AddHealth(int amount)
    {
        playerHealth += amount;
        healthText.text = playerHealth.ToString();
    }

    public void SubtractHealth(int amount)
    {
        if(playerHealth - amount < 0)
        {
            playerHealth = 0;
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            playerHealth -= amount;
        }
        healthText.text = playerHealth.ToString();
    }
}
