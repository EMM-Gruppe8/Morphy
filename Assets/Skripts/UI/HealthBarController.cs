using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    bool isFlipped = false;

    void Update()
    {
        Debug.Log(transform.parent.localScale.x);
        if (transform.parent.localScale.x < 0 && !isFlipped) {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = true;
        } else if (transform.parent.localScale.x > 0 && isFlipped) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = false;
        }
    }
}
