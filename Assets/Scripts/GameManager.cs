using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public int bestNum = 2;
    private int gen = 0;
    List<PlayerController> playerList;
   
    public int numberPlayer;

    public void NewGeneration()
    {
        gen++;
        if (playerList == null)
        {
            playerList = new List<PlayerController>();

            for (int i = 0; i < numberPlayer; i++)
            {
                PlayerController player = Instantiate(playerPrefab, new Vector3(4, 1, 0), Quaternion.identity, transform).GetComponent<PlayerController>();
                player.network = new NeuralNetwork(3, 10, 1);
                player.network.InitializeRandomWeight();
                playerList.Add(player);
            }
        }
        else
        {
            playerList.Sort((x, y) => x.score.CompareTo(y.score));
            while (playerList.Count > bestNum)
            {
                Destroy(playerList[0].gameObject);
                playerList.RemoveAt(0);
            }

            List<PlayerController> newGen = Reproduction(playerList, numberPlayer - bestNum);
            foreach(PlayerController parent in playerList)
            {
                PlayerController player = Instantiate(playerPrefab, new Vector3(4, 1, 0), Quaternion.identity, transform).GetComponent<PlayerController>();
                player.network = parent.network;
                newGen.Add(player);
                Destroy(parent.gameObject);
            }
            playerList = newGen;

        }
    }

    private List<PlayerController> Reproduction(List<PlayerController> parents, int numChild)
    {
        int numParents = parents.Count;
        List<PlayerController> childs = new List<PlayerController>();

        for(int i = 0; i < numChild; i++)
        {
            PlayerController parent1 = parents[Random.Range(0, numParents)];
            PlayerController parent2 = parents[Random.Range(0, numParents)];
            while (parent1 == parent2)
                parent2 = parents[Random.Range(0, numParents)];

            PlayerController child = Instantiate(playerPrefab, new Vector3(4, 1, 0), Quaternion.identity, transform).GetComponent<PlayerController>();
            child.network = NeuralNetwork.Fuse(parent1.network, parent2.network);
            childs.Add(child);
        }
        return childs;
    }

    bool AllDead()
    {
        foreach(PlayerController player in playerList)
        {
            if (player.alive)
                return false;
        }
        return true;
    }

    public void StartGameLoop()
    {
        Debug.Log("Generation: " + gen);
        foreach(PlayerController pc in playerList)
        {
            StartCoroutine(pc.GameLoop());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        NewGeneration();
        StartGameLoop();
    }

    // Update is called once per frame
    void Update()
    {
        if (AllDead())
        {
            MapGenerator mapGenerator = GameObject.Find("Map").GetComponent<MapGenerator>();
            mapGenerator.Clear();
            mapGenerator.Create();
            NewGeneration();
            StartGameLoop();
            Camera.main.transform.position = new Vector3(7.44f, 6, 0);
        }
    }
}
