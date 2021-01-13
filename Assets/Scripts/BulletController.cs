using UnityEngine;

//для выполнения этого скрипта необходим компонент RigidBody2D, поэтому мы заботимся об этом сейчас
[RequireComponent(typeof(Rigidbody2D))]
public class BulletController : MonoBehaviour
{
    //Локально сохраняем компонент для дальнейшего управления
    private Rigidbody2D rb;

    private void Start()
    {
        //Сохраняем компонент в локальную переменную
        rb = GetComponent<Rigidbody2D>();
        //Убираем влияние гравитации на ось Z, т.к. мы в 2D пространстве
        rb.gravityScale = 0;
        //
        rb.velocity = new Vector2(1, 0);
    }

    //При столкновении...
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Проверяем с чем столкнулась пуля
        switch(collision.tag)
        {
            //Если это был маленький астероид - уничтожить и добавить очки
            case "Asteroid":
                GameManager.Instance.SpawnExplosion(gameObject, collision.gameObject, 10);
                break;
            case "MediumAsteroid":
                //spawn 2 asteroids
                GameManager.Instance.SpawnExplosion(gameObject, collision.gameObject, 25);
                break;
            case "BigAsteroid":
                //spawn 3 medium asteroids
                GameManager.Instance.SpawnExplosion(gameObject, collision.gameObject, 75);
                break;
            case "HugeAsteroid":
                GameManager.Instance.SpawnExplosion(gameObject, collision.gameObject, 175);
                //spawn 4 big asteroids
                break;
        }
    }
}