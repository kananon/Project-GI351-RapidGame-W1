using UnityEngine;
using UnityEngine.EventSystems;

public class AnamolyHitbox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float timeToFix = 1f; // ระยะเวลาเอาเมาส์จ่อ (วินาที)
    private float hoverTimer = 0f;
    private bool isHovered = false;
    private CameraManager camManager;

    void OnEnable()
    {
        // รีเซ็ตค่าทุกครั้งที่ Hitbox โผล่มา
        hoverTimer = 0f;
        isHovered = false;

        if (camManager == null)
        {
            camManager = FindFirstObjectByType<CameraManager>();
        }
    }

    void Update()
    {
        // แค่เมาส์ลอยมาจ่อทับก็นับเวลาทันที
        if (isHovered)
        {
            hoverTimer += Time.deltaTime;
            Debug.Log("Hovering... " + hoverTimer.ToString("F1"));

            if (hoverTimer >= timeToFix)
            {
                Debug.Log("Fix Anomaly!");
                if (camManager != null)
                {
                    camManager.ResolveCurrentAnomaly();
                }

                isHovered = false;
                hoverTimer = 0f;
            }
        }
    }

    // แค่เมาส์เลื่อนเข้ามาโดนพื้นที่ (Hover)
    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    // เมื่อเลื่อนเมาส์ออก
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        hoverTimer = 0f;
    }
}