using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void StartGame()
    {
        //Сделано специально, чтобы кнопка смогла вызвать этот метод
        //У нее проблемы со статическими методами.
        GameManager.StartGame();
    }
}