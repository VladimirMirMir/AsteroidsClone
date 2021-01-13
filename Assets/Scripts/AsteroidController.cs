using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.CalculateRotation(GameManager.Instance.PlayerTransform.position, transform.position));
        float speed = GameManager.CalculateVelocity(tag);
        
        rb.velocity = new Vector2(1, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.Instance.SpawnExplosion(gameObject, collision.gameObject, 0);
        }
    }
}