using UnityEngine;

[System.Serializable]
public class CameraData
{
    public string cameraName;       // ชื่อกล้อง เช่น "CAM 01 - VILLAGE"
    public Sprite normalSprite;     // รูป A (ฉากปกติ)
    public Sprite anomalySprite;    // รูป B (ฉากมี Anomaly)
    public GameObject hoverHitbox;  // Hitbox ตรงจุด Anomaly

    [HideInInspector]
    public bool hasAnomaly = false; // สถานะปัจจุบันว่าติด Anomaly หรือยัง
}