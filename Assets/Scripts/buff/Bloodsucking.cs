using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bloodsucking", menuName = "Buffs/Bloodsucking")]
public class Bloodsucking : Buff
{
    public float possibility;

    public override void Apply(PlayerController player)
    {
        player.IncreaseHealProbability(possibility); // 增加 10% 的回血概率

    }
}
