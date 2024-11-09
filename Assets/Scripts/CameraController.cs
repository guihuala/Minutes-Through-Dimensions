using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 0, -10);
    public float smoothSpeed = 0.2f;
    public Vector2 mapBounds = new Vector2(10f, 10f);

    private Camera cam;

    public float shakeDuration = 0.05f; 
    public float shakeMagnitude = 0.05f; 
    private bool isShaking = false;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        if (player != null && !isShaking)
        {
            Vector3 desiredPosition = player.position + offset;
            float distance = Vector3.Distance(transform.position, desiredPosition);

            if (distance > 0.01f) 
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

                // 获取相机视野范围（宽度和高度）
                float camHeight = cam.orthographicSize;
                float camWidth = camHeight * cam.aspect;

                // 应用边界限制
                float minX = -mapBounds.x + camWidth;
                float maxX = mapBounds.x - camWidth;
                float minY = -mapBounds.y + camHeight;
                float maxY = mapBounds.y - camHeight;

                // 将平滑位置约束在边界范围内
                smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, minX, maxX);
                smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, minY, maxY);

                // 最后更新相机位置
                transform.position = smoothedPosition;
            }
        }
    }


    // 震动效果
    public IEnumerator CameraShake()
    {
        Vector3 originalPosition = transform.position; 
        float elapsed = 0.0f; 

        isShaking = true; 

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = new Vector3(originalPosition.x + offsetX, originalPosition.y + offsetY, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null; 
        }

        transform.position = originalPosition;

        isShaking = false; 
    }
}

