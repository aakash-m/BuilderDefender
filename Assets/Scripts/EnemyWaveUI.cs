using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyWaveUI : MonoBehaviour
{
    [SerializeField] EnemyWaveManager enemyWaveManager;

    private Camera mainCamera;
    private TextMeshProUGUI waveNumberText;
    private TextMeshProUGUI waveMessageText;
    private RectTransform enemyWaveSpawnPositionIndicator;
    private RectTransform enemyClosestPositionIndicator;

    private void Awake()
    {
        waveNumberText = transform.Find("waveNumberText").GetComponent<TextMeshProUGUI>();
        waveMessageText = transform.Find("waveMessageText").GetComponent<TextMeshProUGUI>();
        enemyWaveSpawnPositionIndicator = transform.Find("enemyWaveSpawnPositionIndicator").GetComponent<RectTransform>();
        enemyClosestPositionIndicator = transform.Find("enemyClosestPositionIndicator").GetComponent<RectTransform>();
    }

    private void Start()
    {
        mainCamera = Camera.main;
        enemyWaveManager.OnWaveNumberChanged += EnemyWaveManager_OnWaveNumberChanged;
        SetWaveNumberText("Wave " + enemyWaveManager.GetwaveNumber());
    }

    private void EnemyWaveManager_OnWaveNumberChanged(object sender, System.EventArgs e)
    {
        SetWaveNumberText("Wave " + enemyWaveManager.GetwaveNumber());
    }

    private void Update()
    {
        HandleNextWaveMessage();

        HandleEnemyWaveSpawnPositionIndicator();

        HandleEnemyClosestPositionIndicator();

    }

    private void HandleNextWaveMessage()
    {
        float nextWaveSpawnTimer = enemyWaveManager.GetNextWaveSpawnTimer();
        if (nextWaveSpawnTimer <= 0f)
        {
            SetMessageText("");
        }
        else
        {
            SetMessageText("Next Wave in " + nextWaveSpawnTimer.ToString("F1") + " secs");
        }
    }

    private void HandleEnemyWaveSpawnPositionIndicator()
    {
        Vector3 dirToNextSpawnPosition = (enemyWaveManager.GetSpawnPosition() - mainCamera.transform.position).normalized;

        float offset = 300f;
        enemyWaveSpawnPositionIndicator.anchoredPosition = dirToNextSpawnPosition * offset;
        enemyWaveSpawnPositionIndicator.eulerAngles = new Vector3(0, 0, HelperClass.GetAngleFromVector(dirToNextSpawnPosition));

        float distanceToNextEnemySpawnPosition = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);


        enemyWaveSpawnPositionIndicator.gameObject.SetActive(distanceToNextEnemySpawnPosition > mainCamera.orthographicSize * 1.5f);

    }

    private void HandleEnemyClosestPositionIndicator()
    {
        float targetMaxRadius = 9999f;
        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(mainCamera.transform.position, targetMaxRadius);

        Enemy targetEnemy = null;
        foreach (Collider2D collider2D in collider2DArray)
        {
            Enemy enemy = collider2D.GetComponent<Enemy>();
            if (enemy != null)
            {
                //Is a enemy
                if (targetEnemy == null)
                {
                    targetEnemy = enemy;
                }
                else
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <
                        Vector3.Distance(transform.position, targetEnemy.transform.position))
                    {
                        //Closer!
                        targetEnemy = enemy;
                    }
                }
            }
        }


        if (targetEnemy != null)
        {
            Vector3 dirToClosestEnemy = (targetEnemy.transform.position - mainCamera.transform.position).normalized;

            float offset = 250f;
            enemyClosestPositionIndicator.anchoredPosition = dirToClosestEnemy * offset;
            enemyClosestPositionIndicator.eulerAngles = new Vector3(0, 0, HelperClass.GetAngleFromVector(dirToClosestEnemy));

            float distanceToClosestEnemy = Vector3.Distance(enemyWaveManager.GetSpawnPosition(), mainCamera.transform.position);


            enemyClosestPositionIndicator.gameObject.SetActive(distanceToClosestEnemy > mainCamera.orthographicSize * 1.5f);
        }
        else
        {
            //No enemies alive
            enemyClosestPositionIndicator.gameObject.SetActive(false);
        }
    }

    private void SetWaveNumberText(string text)
    {
        waveNumberText.SetText(text);
    }

    private void SetMessageText(string message)
    {
        waveMessageText.SetText(message);
    }


}
