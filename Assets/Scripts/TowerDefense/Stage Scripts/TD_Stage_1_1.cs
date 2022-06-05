using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_Stage_1_1 : MonoBehaviour
{
    [SerializeField] private AudioClip stageBGM;
    [SerializeField] List<GameObject> spawnPoints = new List<GameObject>();
    [SerializeField] private NodeGrid2D stageGrid;
    [SerializeField] private float spawnInterval;
    [SerializeField] private float lastSpawnTime;
    //=============================================//
    [Header("Unit Prefabs")]
    [SerializeField] private List<GameObject> enemyPrefabs;
    [SerializeField] private GameObject speedyBoi;
    [SerializeField] private GameObject slimyBoi;
    [SerializeField] private GameObject chonkyBoi;
    //=============================================//
    private int spawnCount;
    private int enemyCount;
    private float stageStartTime;
    private bool stageStart = false;

    // Start is called before the first frame update
    void Start()
    {
        lastSpawnTime = Time.time;
        stageStartTime = Time.time;
        spawnCount = 0;
        enemyCount = TD_GameManager.Instance.enemyCount;

        SoundManager.Instance.PlayBGM(stageBGM);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > stageStartTime + 5f)
        {
            stageStart = true;
        }

        if (!stageStart)
            return;

        if (TD_GameManager.Instance.enemyCount > 0 && spawnCount < enemyCount)
        {
            if (Time.time > lastSpawnTime + spawnInterval)
            {
                int r = Random.Range(0, spawnPoints.Count);
                int r2 = Random.Range(0, enemyPrefabs.Count);

                GameObject enemy = Instantiate(enemyPrefabs[r2], spawnPoints[r].transform.position, Quaternion.identity);
                enemy.GetComponent<TD_EnemyBase>().Initialize(spawnPoints[r].GetComponent<RedBox>().waypointRoute, stageGrid);

                lastSpawnTime = Time.time;
                spawnCount++;
            }
        }
    }
}
