using System.Collections;
using UnityEngine;

public class AutoDestruction : MonoBehaviour
{
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
