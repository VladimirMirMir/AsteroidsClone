using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AsteroidController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        if (GameManager.Instance.PlayerTransform != null)
            transform.rotation = Quaternion.Euler(0f, 0f, GameManager.CalculateRotation(GameManager.Instance.PlayerTransform.position, transform.position));
        float speed = GameManager.CalculateVelocity(tag);
        if (GameManager.Instance.PlayerTransform != null)
            rb.velocity = GameManager.NormalizedDistance(GameManager.Instance.PlayerTransform.position, transform.position) * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.SpawnExplosion(gameObject, collision.gameObject, 0);
        }
    }
}