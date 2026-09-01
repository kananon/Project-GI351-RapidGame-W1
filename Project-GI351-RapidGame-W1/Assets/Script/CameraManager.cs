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
    public bool isAutoSwitch = true;      // ติ๊กเปิด/ปิด ระบบสลับอัตโนมัติ
    public float switchInterval = 5f;     // ระยะเวลาเปลี่ยนกล้อง (วินาที)
    private float timer = 0f;

    void Start()
    {
        UpdateCameraUI();
    }

    void Update()
    {
        // ระบบสลับอัตโนมัติ
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

        // สลับไปกล้องใหม่
        currentCamIndex = newIndex;
        UpdateCameraUI();
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

    // ฟังก์ชันเคลียร์ Anomaly เมื่อเอาเมาส์จ่อ Hitbox สำเร็จ
    public void ResolveCurrentAnomaly()
    {
        if (cameras != null && currentCamIndex < cameras.Length)
        {
            cameras[currentCamIndex].hasAnomaly = false;
            UpdateCameraUI();
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

        // ดึงข้อมูลกล้องปัจจุบัน
        CameraData currentCam = cameras[currentCamIndex];

        // สลับรูปภาพ Normal / Anomaly
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