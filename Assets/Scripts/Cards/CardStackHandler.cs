using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardStackHandler : MonoBehaviour
{
    private const float yOffset = .3f, zOffset = .05f;

    private void OnTransformChildrenChanged()
    {
        OrginizeTransformChildren();
    }

    private void OrginizeTransformChildren()
    {

        if (transform.childCount < 1) return;
        for (int i = 0; i < transform.childCount; i++)
        {
            float forward = transform.rotation.z == 0 ? 1 : -1; //stack will build upwards depending on which side of the table the player is

            transform.GetChild(i).position = transform.position + new Vector3(0, forward * yOffset * i, -(zOffset * i));
        }
    }
}
