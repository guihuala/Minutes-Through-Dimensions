using UnityEngine;

public class barrierController : MonoBehaviour
{
    public Color barrierColor = Color.white; 

    private SpriteRenderer spriteRenderer;

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            spriteRenderer.color = barrierColor; 
        }
    }

    public virtual void OnPlayerCollision(PlayerController player)
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag( "Player"))
        {
            OnPlayerCollision(collision.gameObject.GetComponent<PlayerController>());
        }
    }
}
