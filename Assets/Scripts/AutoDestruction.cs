using System.Collections;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
    [Range(5f, 15f)]
    public float Timer;

    void Start()
    {
        StartCoroutine(Destr());
    }

    IEnumerator Destr()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
