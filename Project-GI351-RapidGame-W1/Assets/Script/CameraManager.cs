using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("UI Reference")]
    public Image monitorDisplay;

    [Header("Camera Data List")]
    public CameraData[] cameras;
    public int currentCamIndex = 0;

    [Header("Anomaly Chance")]
    [Range(0, 100)] public float spawnChance = 50f;

    [Header("Auto Switch System")]
    public bool isAutoSwitch = true;
    public float switchInterval = 5f;
    private float timer = 0f;

    [Header("Camera Switch Sound")]
    public AudioSource audioSource;
    public AudioClip switchSound;
    public float soundDuration = 0.3f;

    void Start()
    {
        UpdateCameraUI();
    }

    void Update()
    {
        if (isAutoSwitch && cameras != null && cameras.Length > 1)
        {
            timer += Time.deltaTime;

            if (timer >= switchInterval)
            {
                timer = 0f;
                int nextCamIndex = (currentCamIndex + 1) % cameras.Length;
                SwitchCamera(nextCamIndex);
            }
        }
    }

    public void SwitchCamera(int newIndex)
    {
        if (cameras == null || newIndex == currentCamIndex || newIndex >= cameras.Length) return;

        // สุ่มเกิด Anomaly ที่กล้องตัวเดิมก่อนสลับหนี
        TrySpawnAnomaly(currentCamIndex);

        currentCamIndex = newIndex;
        UpdateCameraUI();

        // เล่นเสียงตอนสลับกล้อง
        if (audioSource != null && switchSound != null)
        {
            StartCoroutine(PlaySwitchSound());
        }
    }

    private IEnumerator PlaySwitchSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(switchSound);

        yield return new WaitForSeconds(soundDuration);

        audioSource.Stop();
    }

    private void TrySpawnAnomaly(int camIndex)
    {
        if (!cameras[camIndex].hasAnomaly)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= spawnChance)
            {
                cameras[camIndex].hasAnomaly = true;
            }
        }
    }

    // ฟังก์ชันแก้ Anomaly ของกล้องปัจจุบัน
    // ฟังก์ชันแก้ Anomaly ของกล้องปัจจุบัน
    public void ResolveCurrentAnomaly()
    {
        if (cameras != null && currentCamIndex < cameras.Length)
        {
            if (cameras[currentCamIndex].hasAnomaly)
            {
                // 1. เคลียร์สถานะผีออก
                cameras[currentCamIndex].hasAnomaly = false;

                // 2. สั่งให้ MeltdownManager ลดค่า Panic ทันที 20 หน่วย
                MeltdownManager meltdown = FindFirstObjectByType<MeltdownManager>();
                if (meltdown != null)
                {
                    meltdown.ReducePanicOnFix(20f); // ลด 20 หน่วยตรงนี้
                }

                // 3. อัปเดตหน้าจอ UI
                UpdateCameraUI();
            }
        }
    }

    public void UpdateCameraUI()
    {
        if (cameras == null || cameras.Length == 0) return;

        // ซ่อน Hitbox ทั้งหมดก่อน
        for (int i = 0; i < cameras.Length; i++)
        {
            if (cameras[i].hoverHitbox != null)
                cameras[i].hoverHitbox.SetActive(false);
        }

        CameraData currentCam = cameras[currentCamIndex];

        // สลับรูปภาพ Normal / Anomaly และจัดการ Hitbox
        if (currentCam.hasAnomaly)
        {
            if (currentCam.anomalySprite != null)
                monitorDisplay.sprite = currentCam.anomalySprite;

            if (currentCam.hoverHitbox != null)
                currentCam.hoverHitbox.SetActive(true);
        }
        else
        {
            if (currentCam.normalSprite != null)
                monitorDisplay.sprite = currentCam.normalSprite;
        }
    }
}