using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public int numPiece;
    public int obstacleFrequence;

    public GameObject piece000;
    public GameObject piece001;
    public GameObject piece010;
    public GameObject piece011;
    public GameObject piece100;
    public GameObject piece101;
    public GameObject piece110;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numPiece; i++){
            int rand = Random.Range(0, obstacleFrequence + 6);
            switch (rand){
                case 0: 
                    Instantiate(piece001, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                case 1:
                    Instantiate(piece010, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                case 2:
                    Instantiate(piece011, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                case 3:
                    Instantiate(piece100, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                case 4:
                    Instantiate(piece101, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                case 5:
                    Instantiate(piece110, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
                default:
                    Instantiate(piece000, new Vector3(-i * 4, 0, 0), Quaternion.identity);
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
