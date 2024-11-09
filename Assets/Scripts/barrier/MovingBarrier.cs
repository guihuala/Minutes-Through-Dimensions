using UnityEngine;

public class MovingBarrier : barrierController
{
    public Vector3 moveDirection = Vector3.right; // 移动方向
    public float moveSpeed = 2f; // 移动速度
    public float moveDistance = 3f; // 移动距离

    private Vector3 startPosition;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
    }

    private void Update()
    {
        // 实现障碍物在固定范围内移动
        float distance = Mathf.PingPong(Time.time * moveSpeed, moveDistance);
        transform.position = startPosition + moveDirection * distance;
    }

    public override void OnPlayerCollision(PlayerController player)
    {
    }
}

