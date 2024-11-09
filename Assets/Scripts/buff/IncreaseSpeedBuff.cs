using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseSpeedBuff", menuName = "Buffs/IncreaseSpeed")]
public class IncreaseSpeedBuff : Buff
{
    public float speedIncreaseAmount; // 增加的速度

    public override void Apply(PlayerController player)
    {
        player.IncreaseSpeed(speedIncreaseAmount);
    }
}

