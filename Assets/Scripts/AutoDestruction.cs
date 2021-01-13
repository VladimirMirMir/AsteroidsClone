using System.Collections;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    //Время на самоуничтожение
    [Range(1f, 15f)]
    public float Timer;

    void Start()
    {
        StartCoroutine(Destr());
    }

    IEnumerator Destr()
    {
        //Запускаем отложенное уничтожение
        yield return new WaitForSeconds(Timer);
        Destroy(gameObject);
    }
}
