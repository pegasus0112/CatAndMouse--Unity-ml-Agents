using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;
using System.Linq;
using UnityEditor;

using Unity.Collections;
using System.ComponentModel;

public class GameManager : MonoBehaviour
{
    [Header("Setup")]
    public List<GameObject> mice = new();
    public List<GameObject> cats = new();
    public Transform spawnPoints;
    [Space(10)]
    [Header("Settings")]
    [Range(60, 600)] public float gameTime = 90;

    float playedTime;
    float existingMice;
    public float deadMice = 0;

    private SimpleMultiAgentGroup groupMouse;
    private SimpleMultiAgentGroup groupCat;

    // Start is called before the first frame update
    void Start()
    {
        existingMice= mice.Count;

        groupMouse = new SimpleMultiAgentGroup();
        foreach (var mouse in mice)
        {
            groupMouse.RegisterAgent(mouse.GetComponent<AgentMouse>());
        }

        groupCat = new SimpleMultiAgentGroup();
        foreach (var cat in cats)
        {
            groupMouse.RegisterAgent(cat.GetComponent<AgentCat>());
        }
        resetScene();
    }

    // Update is called once per frame
    void Update()
    {
        playedTime += Time.deltaTime;

        if (playedTime > gameTime)
        {
            groupMouse.AddGroupReward(2);
            groupCat.AddGroupReward(-2);

            groupMouse.EndGroupEpisode();
            groupCat.EndGroupEpisode();
            resetScene();
        }
        if (existingMice <= deadMice)
        {
            groupCat.AddGroupReward(2);
            groupMouse.AddGroupReward(-2);

            groupMouse.EndGroupEpisode();
            groupCat.EndGroupEpisode();
            resetScene();
        }
    }

    void placeTeams()
    {
        int miceSpawnNumber = Random.Range(0, spawnPoints.childCount-1);
        int catSpawnNumber = Random.Range(0, spawnPoints.childCount - 1);
        while (catSpawnNumber == miceSpawnNumber)
        {
            catSpawnNumber = Random.Range(0, spawnPoints.childCount - 1);
        }
        placeMice(miceSpawnNumber);
        placeCat(catSpawnNumber);
    }

    void placeMice(int miceSpawnNumber) {
        Vector3 miceLocation = new Vector3(spawnPoints.GetChild(miceSpawnNumber).transform.position.x, 0.25f, spawnPoints.GetChild(miceSpawnNumber).transform.position.z);
        mice[0].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        miceLocation.x += 1;
        mice[1].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        miceLocation.x -= 2;
        mice[2].transform.SetPositionAndRotation(miceLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
    }

    void placeCat(int catSpawnNumber)
    {
        Vector3 catLocation = new Vector3(spawnPoints.GetChild(catSpawnNumber).transform.position.x, 0.25f, spawnPoints.GetChild(catSpawnNumber).transform.position.z);
        cats[0].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        catLocation.x += 1;
        cats[1].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        catLocation.x -= 2;
        cats[2].transform.SetPositionAndRotation(catLocation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
    }

    void resetScene()
    {
        foreach (GameObject mouse in mice)
        {
            mouse.SetActive(false);
        }

        foreach (var cat in cats)
        {
            cat.SetActive(false);
        }

        placeTeams();

        foreach (GameObject mouse in mice)
        {
            mouse.SetActive(true);
            groupMouse.RegisterAgent(mouse.GetComponent<AgentMouse>());
            
        }

        foreach (var cat in cats)
        {
            cat.SetActive(true);
            groupCat.RegisterAgent(cat.GetComponent<AgentCat>());
        }
        deadMice = 0;
        playedTime = 0;
    }
}
