using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaygroundGenerator : MonoBehaviour
{
    public Tilemap Map;
    public TileBase Ground;
    public List<TileBase> Grass;
    public List<TileBase> Flowers;

    public Vector2Int Size;
    public Vector2Int Center;
    public float GrassRate;
    public float FlowerRate;

    public CinemachineConfiner2D Confiner2D;
    public BoxCollider2D LimitArea;
    public List<BoxCollider2D> EdgeBlockers;
    public float EdgeWidth;

    void Awake()
    {
        Generate();

        LimitArea.size = Size;
        LimitArea.offset = new Vector2(Size.x % 2 / 2.0f, Size.y % 2 / 2.0f);

        EdgeBlockers[0].size = new Vector2(EdgeWidth, Size.y * 1.1f);
        EdgeBlockers[0].offset = new Vector2(-Size.x / 2.0f - EdgeWidth / 2, 0) + LimitArea.offset;


        EdgeBlockers[1].size = new Vector2(Size.x * 1.1f, EdgeWidth);
        EdgeBlockers[1].offset = new Vector2(0, Size.y / 2.0f + EdgeWidth / 2) + LimitArea.offset;


        EdgeBlockers[2].size = new Vector2(EdgeWidth, Size.y * 1.1f);
        EdgeBlockers[2].offset = new Vector2(Size.x / 2.0f + EdgeWidth / 2, 0) + LimitArea.offset;


        EdgeBlockers[3].size = new Vector2(Size.x * 1.1f, EdgeWidth);
        EdgeBlockers[3].offset = new Vector2(0, -Size.y / 2.0f - EdgeWidth / 2) + LimitArea.offset;
    }

    void Start()
    {
        Confiner2D.BoundingShape2D = LimitArea;
    }

    // Generate tilemap
    void Generate()
    {

        for (int i = 0; i < Size.x; i++)
        {
            for (int j = 0; j < Size.y; j++)
            {
                Map.SetTile(GetPos(i, j), GetTile());
            }
        }
    }

    TileBase GetTile()
    {
        var rdValue = Random.value;
        if (rdValue < FlowerRate)
            return Flowers[Random.Range(0, Flowers.Count)];
        else if (rdValue < GrassRate)
            return Grass[Random.Range(0, Grass.Count)];
        return Ground;
    }

    Vector3Int GetPos(int i, int j)
    {
        return new Vector3Int(i - Size.x / 2, j - Size.y / 2);
    }

}
