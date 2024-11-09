using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IncreaseSkillBuff", menuName = "Buffs/IncreaseSkillTimes")]
public class IncreaseSkillTimes : Buff
{
    public int skillTimesIncrease; 

    public override void Apply(PlayerController player)
    {
        player.AddSkillTime(skillTimesIncrease);
    }
}
