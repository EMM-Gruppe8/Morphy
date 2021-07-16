using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    /// <summary>
    /// Should the health bar be flipped.
    /// This is needed to make sure the health bar is always in the right orientation,
    /// even if the object itself is flipped
    /// </summary>
    bool isFlipped = false;

    /// <summary>
    /// Update the flipped state of the health bar based on the parent object's orientation
    /// </summary>
    void Update()
    {
        if (transform.parent.localScale.x < 0 && !isFlipped) {
            transform.localScale = new Vector3(-1f * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = true;
        } else if (transform.parent.localScale.x > 0 && isFlipped) {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            isFlipped = false;
        }
    }
}
