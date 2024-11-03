using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTagger : MonoBehaviour
{
    public Tilemap tilemap;

    void Start()
    {
        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(pos)) continue;
            
            var tileObject = tilemap.GetInstantiatedObject(pos);
            if (tileObject != null)
            {
                tileObject.tag = "cuadrados";
            }
        }
    }
}
