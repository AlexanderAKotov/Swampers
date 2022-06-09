using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorkWithTile : MonoBehaviour
{
    public TileBase A;
    public TileBase B;
    public static Dictionary<TileBase, TileSetting> TileProperty = new Dictionary<TileBase, TileSetting>();
    // Start is called before the first frame update
    private void Awake()
    {
        Debug.Log(A.name);
        TileProperty.Add(A, TileObject.Ground);
        TileProperty.Add(B, TileObject.Blocked);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
