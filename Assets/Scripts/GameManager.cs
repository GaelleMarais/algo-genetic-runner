using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public Canvas ui;

    public int bestNum = 2;
    private int gen = 0;
    List<PlayerController> playerList;
   
    public int numberPlayer;

    public float mutation = 0.05f;

    private float bestScore = 0;
    private float previousScore = 0f;

    public void NewGeneration()
    {
        gen++;
        if (playerList == null)
        {
            playerList = new List<PlayerController>();

            for (int i = 0; i < numberPlayer; i++)
            {
                PlayerController player = Instantiate(playerPrefab, new Vector3(4, 1, 0), Quaternion.identity, transform).GetComponent<PlayerController>();

                //randomize some value to the see the different cube
                player.transform.position += new Vector3(0, Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                player.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

                //create the network
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
                //randomize some value to the see the different cube
                player.transform.position += new Vector3(0, Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
                player.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

                //create the network
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
            //randomize some value to the see the different cube
            child.transform.position += new Vector3(0, Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));
            child.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

            //create the net work
            child.network = NeuralNetwork.Fuse(parent1.network, parent2.network,mutation);
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
            GenInfo info = GetGenInfo();
            previousScore = info.score;
            if (bestScore < info.score)
                bestScore = info.score;
            NewGeneration();
            StartGameLoop();
            Camera.main.transform.position = new Vector3(7.44f, 6, 0);
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        GenInfo info = GetGenInfo();

        ui.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Gen: " + gen;
        ui.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Alive: " + info.numAlive;
        ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Current score: " + info.score;
        ui.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Previous score: " + previousScore;
        ui.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Best score: " + bestScore;
        if (info.score > bestScore)
            ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
        else
            ui.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.red;

    }

    private GenInfo GetGenInfo()
    {
        int numAlive = 0;
        float score = 0;
        foreach (PlayerController player in playerList)
        {
            if (player.alive)
                numAlive++;
            if (player.score > score)
                score = player.score;
        }
        return new GenInfo(numAlive, score);
    }

    struct GenInfo
    {
        public int numAlive;
        public float score;

        public GenInfo(int numAlive, float score)
        {
            this.numAlive = numAlive;
            this.score = score;
        }
    }
}
