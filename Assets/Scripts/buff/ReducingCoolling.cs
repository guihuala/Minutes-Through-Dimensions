using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReduceCoolDownBuff", menuName = "Buffs/ReduceCooling")]
public class ReducingCoolling :Buff
{
    public float reducingTime;

    public override void Apply(PlayerController player)
    {
        player.ReduceCoolDownTime(reducingTime);
    }
}
