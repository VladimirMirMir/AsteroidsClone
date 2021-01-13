using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    private AudioSource audioSource;
    private Vector3 localMouseInput;
    private float shipWidth;
    private float shipHeight;

    private void Start()
    {
        //Сохраняем источник звука
        audioSource = GetComponent<AudioSource>();
        //Выключаем системный курсор
        Cursor.visible = false;
        //Сохраняем размеры корабля по осям X и Y, чтобы потом проверять не вышел ли он за экран.
        //Число, полученное таким методом - это половина размера (по каждой из осей), ведь для рассчётов нужно именно столько.
        //Это сделано для того, чтобы лишний раз не делить, а воспользоваться средствами движка.
        shipWidth = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.x;
        shipHeight = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().bounds.extents.y;
    }

    private void Update()
    {
        //Если игра не окончена, воспринимать ввод от игрока
        if (!GameManager.Instance.GameIsOver)
            HandleInput();
    }

    private void LateUpdate()
    {
        //Вызываем этот метод в LateUpdate чтобы убедиться в том, что корабль находится на экране перед отрисовкой кадра, но после ввода от пользователя
        KeepShipOnScreen();
    }

    private void HandleInput()
    {
        HandleMovement();
        HandleRotation();
        if (Input.GetMouseButtonDown(0))
        {
            //Появляется пуля и звук
            GameManager.SpawnBullet();
            audioSource.Play();
        }
    }

    private void HandleMovement()
    {
        //Получаем ввод с клавиатуры. (Как нажатие стрелок, так и WASD)
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //Плавно перемещаем объект с заданной скоростью
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void HandleRotation()
    {
        //Получаем координаты курсора в мировых координатах
        localMouseInput = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.GetComponent<Transform>().position.z));
        //Помещаем изображение курсора в полученные координаты
        GameManager.Instance.CursorArt.position = new Vector2(localMouseInput.x, localMouseInput.y);
        //Поворачиваем корабль по оси Z
        transform.GetChild(0).rotation = Quaternion.Euler(0f, 0f, GameManager.CalculateRotation(localMouseInput, transform.position));
    }

    private void KeepShipOnScreen()
    {
        //Сохраняем текущие координаты корабля
        Vector3 viewPos = transform.position;
        //Удерживаем координаты в границах экрана по осям X и Y, учитывая размеры корабля
        viewPos.x = Mathf.Clamp(viewPos.x, GameManager.Instance.ScreenBounds.x * -1 + shipWidth, GameManager.Instance.ScreenBounds.x - shipWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, GameManager.Instance.ScreenBounds.y * -1 + shipHeight, GameManager.Instance.ScreenBounds.y - shipHeight);
        //Присваиваем кораблю обновлённые координаты
        transform.position = viewPos;
    }
}
