using UnityEngine;

public class DestructiveBarrier : barrierController
{
    public override void OnPlayerCollision(PlayerController player)
    {
        if (!player.isInvincible)
        {
            player.GetHurt();
            SfxManager.instance.PlaySFX(3);
            Destroy(gameObject); 
        }
    }
}

