using UnityEngine;

public class BoundaryRestriction : MonoBehaviour
{
    [SerializeField]private Vector2 mapBounds;

    private void Start()
    {
        if (GameManager.Instance)
        {
            mapBounds = GameManager.Instance.mapBounds;
        }

    }

    void Update()
    {
        RestrictMovement();
    }

    private void RestrictMovement()
    {
        Vector3 position = transform.position;

        position.x = Mathf.Clamp(position.x, -mapBounds.x, mapBounds.x);
        position.y = Mathf.Clamp(position.y, -mapBounds.y, mapBounds.y);

        transform.position = position;
    }
}

