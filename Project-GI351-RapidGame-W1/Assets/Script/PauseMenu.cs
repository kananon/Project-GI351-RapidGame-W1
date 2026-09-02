using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public GameObject winPanel;

    [Header("Game Over Sound")]
    public AudioSource gameOverAudioSource;
    public AudioClip gameOverSound;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (gameOverPanel != null && gameOverPanel.activeSelf)
                return;

            if (pausePanel.activeSelf)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);

        // เล่นเสียงตอนแพ้
        if (gameOverAudioSource != null && gameOverSound != null)
        {
            gameOverAudioSource.ignoreListenerPause = true;
            gameOverAudioSource.PlayOneShot(gameOverSound);
        }

        Time.timeScale = 0f;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Win()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}