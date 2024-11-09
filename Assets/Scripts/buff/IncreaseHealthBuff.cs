using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseHealthBuff", menuName = "Buffs/IncreaseHealth")]
public class IncreaseHealthBuff : Buff
{
    public int healthIncreaseAmount; // 增加的生命值

    public override void Apply(PlayerController player)
    {
        player.IncreaseMaxHealth(healthIncreaseAmount);
    }
}

