using UnityEngine;
using UnityEngine.UI;

public class MeltdownManager : MonoBehaviour
{
    [Header("UI Sliders")]
    public Slider panicSlider;
    public Slider corruptSlider;

    [Header("Panic Settings")]
    public float currentPanic = 0f;
    public float maxPanic = 100f;

    [Tooltip("ความเร็ว Panic ตอนรูปปกติ")]
    public float normalPanicRate = 3f; // ขึ้นช้าๆ ตลอดเวลา

    [Tooltip("ความเร็ว Panic ตอนหน้าจอเป็นรูปผิดปกติ (Anomaly)")]
    public float anomalyPanicRate = 20f; // ความเร็วเสริมเมื่อกล้องตรงหน้ามี Anomaly

    [Header("Corrupt Settings")]
    public float currentCorrupt = 0f;
    public float maxCorrupt = 100f;
    public float corruptIncreaseRate = 15f;
    public float corruptDecreaseRate = 10f;

    [Header("References")]
    public CameraManager cameraManager;

    private bool isGameOver = false;

    void Start()
    {
        if (cameraManager == null)
            cameraManager = FindFirstObjectByType<CameraManager>();

        if (panicSlider != null) panicSlider.maxValue = maxPanic;
        if (corruptSlider != null) corruptSlider.maxValue = maxCorrupt;
    }

    void Update()
    {
        if (isGameOver) return;

        // เช็กเฉพาะกล้องที่กำลังแสดงอยู่บนหน้าจอ ณ ตอนนี้
        bool isCurrentCamAnomaly = IsCurrentCameraAnomaly();

        // ค่าความเร็วพื้นฐาน (รูปปกติ)
        float currentRate = normalPanicRate;

        // ถ้ากล้องตรงหน้าเป็นรูปผิดปกติ ให้บวกความเร็วเพิ่ม
        if (isCurrentCamAnomaly)
        {
            currentRate += anomalyPanicRate;
        }

        // เพิ่ม Panic ตามความเร็วที่กำหนด
        currentPanic += currentRate * Time.deltaTime;

        // --- ระบบ Corrupt ---
        if (currentPanic >= maxPanic)
        {
            currentCorrupt += corruptIncreaseRate * Time.deltaTime;
        }
        else
        {
            currentCorrupt -= corruptDecreaseRate * Time.deltaTime;
        }

        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        currentCorrupt = Mathf.Clamp(currentCorrupt, 0f, maxCorrupt);

        if (currentCorrupt >= maxCorrupt)
        {
            TriggerGameOver();
        }

        UpdateUI();
    }

    // ฟังก์ชันสั่งลด Panic ทันที 20 หน่วย (เรียกใช้ตอนจ่อแก้สำเร็จ)
    public void ReducePanicOnFix(float amount = 20f)
    {
        currentPanic -= amount;
        currentPanic = Mathf.Clamp(currentPanic, 0f, maxPanic);
        UpdateUI();
        Debug.Log("Anomaly Cleared! Reduced Panic by " + amount);
    }

    // เช็กเฉพาะกล้องปัจจุบันที่กำลังดูอยู่
    private bool IsCurrentCameraAnomaly()
    {
        if (cameraManager == null || cameraManager.cameras == null || cameraManager.cameras.Length == 0)
            return false;

        int index = cameraManager.currentCamIndex;
        if (index >= 0 && index < cameraManager.cameras.Length)
        {
            return cameraManager.cameras[index].hasAnomaly;
        }

        return false;
    }

    private void UpdateUI()
    {
        if (panicSlider != null) panicSlider.value = currentPanic;
        if (corruptSlider != null) corruptSlider.value = currentCorrupt;
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        Debug.Log("<color=red>GAME OVER! MELTDOWN COMPLETE!</color>");
    }
}