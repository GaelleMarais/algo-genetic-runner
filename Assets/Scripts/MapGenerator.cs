using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int size;
    public GameObject[] pieces;
    public GameObject endPiece;

    // Start is called before the first frame update
    void Start()
    {
        Create();
    }

    public void Clear(){
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Create(){
        for(int i = 0; i < size; i++){
            int rand = Random.Range(0, pieces.Length - 1);

            var newPiece = Instantiate(pieces[rand], new Vector3(-i * 4, 0, 0), Quaternion.identity);
            newPiece.transform.parent = gameObject.transform;                   
        }
        var lastPiece = Instantiate(endPiece, new Vector3(-size * 4, 0, 0), Quaternion.identity);
        lastPiece.transform.parent = gameObject.transform;

    }
}
