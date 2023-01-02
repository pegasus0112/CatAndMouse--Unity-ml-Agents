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
    public bool enableTeamRewards = false;
    public List<GameObject> mice = new();
    public List<GameObject> cats = new();
    public Transform spawnPoints;
    public float spawnDeviation = 0.25f;
    [Space(10)]

    [Header("Settings")]
    [Range(0.5f, 600)] public float gameTime = 90;
    [Space(10)]

    [Range(60, 600)] public float catTeamRewards = 2;
    [Range(60, 600)] public float catTeamPenalty = -2;
    [Space(10)]

    [Range(60, 600)] public float mouseTeamRewards = 2;
    [Range(60, 600)] public float mouseTeamPenalty = -2;
    [Space(10)]

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
            if (enableTeamRewards)
            {
                groupMouse.AddGroupReward(mouseTeamRewards);
                groupCat.AddGroupReward(catTeamPenalty);
            }
            groupMouse.EndGroupEpisode();
            groupCat.EndGroupEpisode();
            resetScene();
        }
        if (existingMice <= deadMice)
        {
            if (enableTeamRewards)
            {
                groupCat.AddGroupReward(catTeamPenalty);
                groupMouse.AddGroupReward(mouseTeamPenalty);
            }
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
        foreach (GameObject mouse in mice)
        {
            Vector3 positionDeviation = new Vector3(miceLocation.x + Random.Range(-spawnDeviation, spawnDeviation), miceLocation.y, miceLocation.z + Random.Range(-spawnDeviation, spawnDeviation));
            mouse.transform.SetPositionAndRotation(positionDeviation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        };
    }

    void placeCat(int catSpawnNumber)
    {
        Vector3 catLocation = new Vector3(spawnPoints.GetChild(catSpawnNumber).transform.position.x, 0.25f, spawnPoints.GetChild(catSpawnNumber).transform.position.z);
        foreach (GameObject cat in cats)
        {
            Vector3 positionDeviation = new Vector3(catLocation.x + Random.Range(-spawnDeviation, spawnDeviation), catLocation.y, catLocation.z + Random.Range(-spawnDeviation, spawnDeviation));
            cat.transform.SetPositionAndRotation(positionDeviation, Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up));
        };
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
