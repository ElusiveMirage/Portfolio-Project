using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class TD_GameManager : MonoBehaviour
{
    public TD_Player playerData;
    //=============================================//
    [SerializeField] public bool gamePaused;
    [SerializeField] public bool stageFailed;
    [SerializeField] public bool stageComplete;
    //=============================================//
    [Header("UI Elements")]
    [SerializeField] private UI_Bar playerMPBar;
    [SerializeField] private Image manaRegen;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject failureScreen;
    [SerializeField] private GameObject victoryScreen;
    [SerializeField] private TextMeshProUGUI MPText;
    [SerializeField] private TextMeshProUGUI unitsText;
    [SerializeField] private TextMeshProUGUI unitLimitText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI enemiesText;
    //=============================================//
    [Header("Stage")]
    //==================================================//
    public int stageScore;
    public int enemyCount;
    public NodeGrid2D stageGrid;
    //==================================================//
    [Header("Player Actions")]
    [SerializeField] private GameObject summonEffect;
    [SerializeField] private Transform deploymentZones;
    [SerializeField] private int deploymentLimit;
    public int unitsDeployed;
    public bool placingUnit;
    public bool unitSelected;
    public GameObject selectedUnit;
    //==================================================//
    private static TD_GameManager _instance;

    public static TD_GameManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StageStart();
    }

    // Update is called once per frame
    void Update()
    {             
        if (gamePaused || stageFailed || stageComplete)
        {
            Time.timeScale = 0f; 
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (unitSelected && !stageFailed && !stageComplete)
        {
            Time.timeScale = 0.2f;
        }

        if (placingUnit && !unitSelected)
        {
            Time.timeScale = 0.2f;

            deploymentZones.gameObject.SetActive(true);

            if (selectedUnit != null)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

                position.x = Mathf.RoundToInt(position.x + 1f) - 1f;
                position.y = Mathf.RoundToInt(position.y + 1f) - 1f;
                position.z = 0;

                position = Camera.main.WorldToScreenPoint(position);

                selectedUnit.GetComponent<UnitSelectButton>().selectedUnitSprite.transform.position = position;
            }
        }
        else
        {
            deploymentZones.gameObject.SetActive(false);
        }

        if(!gamePaused)
        {
            UpdateUI();
        }

        if(playerData.playerHP <= 0)
        {
            Invoke("StageFailed", 2f);
        }

        if (enemyCount <= 0)
        {
            Invoke("StageComplete", 2f);
        }
    }

    private void StageStart()
    {
        foreach (Transform transform in deploymentZones)
        {
            Node2D node = stageGrid.GetNodeFromWorldPoint(transform.position);
            node.isPlaceable = true;
            node.deploymentZone = transform;
        }
    }

    public void DecreaseHP(float value)
    {
        playerData.playerHP -= value;
    }

    private void UpdateUI()
    {
        playerMPBar.SetMaxValue(playerData.playerMaxMP);
        playerMPBar.SetMinValue(playerData.playerMP);
        MPText.text = ((int)playerData.playerMP).ToString();
        unitsText.text = unitsDeployed.ToString();
        unitLimitText.text = deploymentLimit.ToString();
        livesText.text = playerData.playerHP.ToString();
        scoreText.text = stageScore.ToString();
        enemiesText.text = ((int)enemyCount).ToString();

        if(manaRegen.fillAmount < 1f && playerData.playerMP < playerData.playerMaxMP)
        {
            manaRegen.fillAmount += 1f * Time.deltaTime;
        }
        else
        {
            manaRegen.fillAmount = 0f;
        }
    }

    public void PlaceUnit(InputAction.CallbackContext context)
    {
        if (placingUnit)
        {
            if (context.performed)
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(selectedUnit.GetComponent<UnitSelectButton>().selectedUnitSprite.transform.position);

                if (Mathf.RoundToInt(Mathf.Abs(position.y) % 2) == 2 || Mathf.RoundToInt(Mathf.Abs(position.y) % 2) == 0 || position.y == 0)
                {
                    if (Mathf.RoundToInt(Mathf.Abs(position.x) % 2) == 1)
                    {
                        Node2D node = stageGrid.GetNodeFromWorldPoint(position);

                        if (node.isPlaceable && !node.unitPlaced && unitsDeployed < deploymentLimit && playerData.playerMP >= selectedUnit.GetComponent<UnitSelectButton>().unitPrefab.GetComponent<UnitBase>().MPCost)
                        {
                            Instantiate(summonEffect, node.worldPos - new Vector3(0, 0.5f, 0), Quaternion.identity);
                            GameObject unit = Instantiate(selectedUnit.GetComponent<UnitSelectButton>().unitPrefab, node.worldPos, Quaternion.identity);
                            node.unitPlaced = true;
                            node.deploymentZone.gameObject.SetActive(false);
                            unit.GetComponent<UnitBase>().gridNode = node;
                            placingUnit = false;
                            unitsDeployed++;
                            playerData.playerMP -= unit.GetComponent<UnitBase>().MPCost;
                            selectedUnit.GetComponent<UnitSelectButton>().Unselected();
                        }
                    }
                }                
            }
        }   
    }

    public void CanceUnitSelection(InputAction.CallbackContext context)
    {
        if (placingUnit)
        {
            placingUnit = false;
            selectedUnit.GetComponent<UnitSelectButton>().Unselected();
        }
    }

    public void PauseGame()
    {
        if(gamePaused)
        {
            SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Resume"));
        }
        else
        {
            SoundManager.Instance.PlaySound(Resources.Load<AudioClip>("SFX/SFX_UI_Pause"));
        }

        gamePaused = !gamePaused;
        pauseScreen.SetActive(gamePaused);
    }

    public void Return()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene("TD_LevelSelect");
    }

    public void Retry()
    {
        Destroy(gameObject.transform.parent.gameObject);
        Time.timeScale = 1f;
        SceneLoader.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StageFailed()
    {
        SoundManager.Instance.StopBGM();
        stageFailed = true;
        failureScreen.SetActive(true);
    }

    private void StageComplete()
    {
        SoundManager.Instance.StopBGM();
        stageComplete = true;
        victoryScreen.SetActive(true);
    }
}
