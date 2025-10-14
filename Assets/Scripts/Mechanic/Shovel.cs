using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;

public class Shovel : MonoBehaviour
{
   [SerializeField] private LayerMask plantLayer;
   [SerializeField] private GameObject shovelCursorPrefab;
   [SerializeField] private PlantPlacer plantPlacer;

   private GameObject shovelCursor;
   private bool isActive = false;

   private void Update(){
    if(!isActive) return;
    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    var hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, plantLayer);

    Vector3 world = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    world.z = 0;
    if(shovelCursor != null) shovelCursor.transform.position = hit ? (Vector3) hit.point : world;

    if(Input.GetMouseButtonDown(1)){
        Deactivate();
        return;
    }

    if(Input.GetMouseButtonDown(0)){
        if(hit && hit.collider.CompareTag("Plant")){
            Destroy(hit.collider.gameObject);
            Deactivate();
        }
    }
   }
	public void Activate()
	{
		if (isActive) return;
		isActive = true;
		if (shovelCursorPrefab != null) shovelCursor = Instantiate(shovelCursorPrefab);
		if (plantPlacer != null) plantPlacer.CancelPlacingExternally();
	}
   public void Deactivate(){
    if(!isActive) return;
    isActive = false;
    if(shovelCursor != null) Destroy(shovelCursor);
   }
}
