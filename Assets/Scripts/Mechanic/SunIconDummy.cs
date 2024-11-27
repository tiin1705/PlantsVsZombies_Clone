using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunIconDummy : MonoBehaviour
{
    public Transform dummyObject;

    private void Update()
    {
        if(dummyObject != null)
        {
            Vector3 screenPosition = transform.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Camera.main.nearClipPlane));
            dummyObject.position = worldPosition;
        }
    }
}
