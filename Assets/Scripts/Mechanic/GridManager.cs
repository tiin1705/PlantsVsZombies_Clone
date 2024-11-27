using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GridManager : MonoBehaviour
{
    public int rows = 5;
    public int columns = 9;
    public float tileWidth = 0.961f;
    public float tileHeigh = 1.2f;
    public Vector2 gridOrigin;


   
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for(int row =0; row < rows; row++)
        {
            for(int col = 0; col < columns; col++)
            {
                Vector3 tilePosition = new Vector3(gridOrigin.x + col * tileWidth, gridOrigin.y + row * tileHeigh, 0);
                Gizmos.DrawWireCube(tilePosition, new Vector3(tileWidth, tileHeigh, 0));
            }
        }
    }

}
