using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Material[] materials;
    public List<GameObject> tiles;

    // Start is called before the first frame update
    void Start()
    {
        tiles = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            tiles.Add(transform.GetChild(i).gameObject);
            if (i % 2 > 0)
            {
                tiles[i].GetComponent<MeshRenderer>().material = materials[0];
            }
            else
            {
                tiles[i].GetComponent<MeshRenderer>().material = materials[1];
            }
        }
    }
}
