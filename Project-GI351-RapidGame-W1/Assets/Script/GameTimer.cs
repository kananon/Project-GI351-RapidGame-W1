using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float gameTime = 300f;
    private float currentTime;
    private bool isGameOver = false;

    [Header("UI Reference")]
    public TMP_Text timerText;

    [Header("Game Over Reference")]
    public PauseMenu pauseMenu;

    [Header("Game Over Sound")]
    public AudioSource audioSource;
    public AudioClip gameOverSound;


    void Start()
    {
        currentTime = gameTime;

        if (pauseMenu == null)
            pauseMenu = FindFirstObjectByType<PauseMenu>();

        UpdateTimerUI();
    }


    void Update()
    {
        if (isGameOver)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0f)
        {
            currentTime = 0f;
            UpdateTimerUI();

            TriggerGameOver();

            return;
        }

        UpdateTimerUI();
    }


    private void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);

            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }


    private void TriggerGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        // เล่นเสียงแพ้
        if (audioSource != null && gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound);
        }

        // เรียก Game Over ที่มีอยู่แล้ว
        if (pauseMenu != null)
        {
            pauseMenu.GameOver();
        }
    }
}