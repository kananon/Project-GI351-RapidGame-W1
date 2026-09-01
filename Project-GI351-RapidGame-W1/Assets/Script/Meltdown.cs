using UnityEngine;
using UnityEngine.UI;

public class MeltdownManager : MonoBehaviour
{
    public CameraManager camManager;

    [Header("UI Sliders")]
    public Slider panicSlider;
    public Slider corruptSlider;

    [Header("Values")]
    public float panicLevel = 0f;
    public float corruptLevel = 0f;

    [Header("Speed Settings")]
    public float corruptSpeed = 3f;  // ปล่อย Anomaly ทิ้งไว้ ความเฮี้ยนขึ้นวินาทีละกี่ %
    public float panicRecoverSpeed = 5f; // พักสายตาแล้วสติฟื้นฟูวินาทีละกี่ %

    void Update()
    {
        if (camManager == null) return;

        // เช็กว่าในเกมตอนนี้มี Anomaly ค้างอยู่กี่กล้อง
        int activeAnomalies = 0;
        foreach (var cam in camManager.cameras)
        {
            if (cam.hasAnomaly) activeAnomalies++;
        }

        // ถ้ามี Anomaly ค้างอยู่ -> เพิ่มเกจ Corrupt
        if (activeAnomalies > 0)
        {
            corruptLevel += corruptSpeed * activeAnomalies * Time.deltaTime;
        }

        // ฟื้นฟู Panic ค่อยๆ ลดลงเมื่อไม่มีอะไร
        panicLevel -= panicRecoverSpeed * Time.deltaTime;

        // ล็อกค่าให้อยู่ในช่วง 0 - 100
        panicLevel = Mathf.Clamp(panicLevel, 0f, 100f);
        corruptLevel = Mathf.Clamp(corruptLevel, 0f, 100f);

        // อัปเดตไปยัง UI Slider
        if (panicSlider) panicSlider.value = panicLevel;
        if (corruptSlider) corruptSlider.value = corruptLevel;

        // เช็กเงื่อนไขแพ้ (Meltdown)
        if (panicLevel >= 100f || corruptLevel >= 100f)
        {
            Debug.Log("MELTDOWN! GAME OVER!");
        }
    }
}