using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycle : MonoBehaviour
{
    [SerializeField] private int dayLength = 500;
    public int timeLeft;

    // Start is called before the first frame update
    void Awake()
    {
        timeLeft = dayLength;
        StartCoroutine("DepleteTimeLeft");
    }
    private IEnumerator DepleteTimeLeft ()
    {
        timeLeft--;

        yield return new WaitForSeconds(1);

        StartCoroutine("DepleteTimeLeft");
    }
}
