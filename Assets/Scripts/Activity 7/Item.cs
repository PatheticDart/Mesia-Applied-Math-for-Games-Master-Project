using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;
    public PlayerLogic player;
    private bool hasHit = false;

    [Header("Item Behavior")]
    public bool isJumpable = true;
    public bool isHealthPickup = false;
    public float damageAmount = 25f;

    public Sprite hitSprite;

    void Update()
    {
        float fl = CameraComponent.focalLength;
        float perspective = fl / (fl + itemPosition.z);

        transform.localScale = Vector3.one * perspective;
        transform.position = new Vector2(itemPosition.x, itemPosition.y) * perspective;

        if (!hasHit && player != null)
        {
            float zDistance = Mathf.Abs(itemPosition.z - player.worldPosition.z);
            float xDistance = Mathf.Abs(itemPosition.x - player.worldPosition.x);
            float yDistance = Mathf.Abs(itemPosition.y - player.worldPosition.y);

            bool yOverlap = (isJumpable || isHealthPickup) ? (yDistance < 0.1f) : true;

            if (zDistance < 0.5f && xDistance < 1f && yOverlap)
            {
                hasHit = true;

                if (isHealthPickup)
                {
                    player.Heal(50f);
                    gameObject.SetActive(false);
                }
                else
                {
                    player.TakeDamage(damageAmount);

                    SpriteRenderer sr = GetComponent<SpriteRenderer>();
                    if (sr != null && hitSprite != null)
                    {
                        sr.sprite = hitSprite;
                        sr.color = Color.red;
                    }
                }
            }
        }
    }
}