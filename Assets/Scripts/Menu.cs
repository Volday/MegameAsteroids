using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GameController gameController;
    public GameObject escMenu;
    public GameObject inGameInterface;

    public Text changeControlButtonText;
    public Button resumeButton;

    public Text scoreText;
    public Transform health;
    public GameObject healthIcon;
    public float healthIconRightShift;
    private GameObject[] healthIcons;

    private bool menuTriggered = false;

    private int score;

    private void Awake()
    {
        ResetScore();
    }

    private void Update()
    {
        //Включение\Отключение меню по кнопке клавиатуры
        if (Input.GetAxisRaw("Menu") > 0 && menuTriggered == false) {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else {
                PauseGame();
            }
            menuTriggered = true;
        }

        if (Input.GetAxisRaw("Menu") == 0) {
            menuTriggered = false;
        }

        //Отключение кнопки "Продолжить", если игра не начата
        if (escMenu.activeSelf) {
            if (gameController.GetGameStatus())
            {
                resumeButton.interactable = true;
            }
            else
            {
                resumeButton.interactable = false;
            }
        }
    }

    public void UpdateHealthIcons(int _health) {
        _health--;
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (_health > i)
            {
                healthIcons[i].SetActive(true);
            }
            else {
                healthIcons[i].SetActive(false);
            }
        }
    }

    public void ResetScore()
    {
        SetScore(0);
    }

    public void SetScore(int _points)
    {
        score = _points;
        scoreText.text = $"Score: {score}";
    }

    public void IncreaseScore(int _points) {
        SetScore(score + _points);
    }

    public void SetGameController(GameController _gameController)
    {
        gameController = _gameController;
        
        //создание иконок здоровья
        healthIcons = new GameObject[gameController.maxPlayerHealthPoints - 1];
        for (int i = 0; i < gameController.maxPlayerHealthPoints - 1; i++)
        {
            healthIcons[i] = Instantiate(healthIcon, health);
            healthIcons[i].GetComponent<RectTransform>().localPosition = new Vector2(healthIconRightShift * i, 0);
            healthIcons[i].SetActive(false);
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        escMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        if (gameController.GetGameStatus()) {
            Time.timeScale = 1;
            escMenu.SetActive(false);
        }
    }

    public void NewGame() {
        Time.timeScale = 1;
        gameController.NewGame();
        escMenu.SetActive(false);
    }

    //Переключение управления между клавиатурой, и клавиатурой с мышью
    public void ChangeControl() {
        PlayerController playerController = gameController.player.GetComponent<PlayerController>();
        if (playerController.GetCurrentControllerType() == PlayerController.ControllerType.keyboard) {
            gameController.player.GetComponent<PlayerController>().SetController(PlayerController.ControllerType.mouseAndKeyboard);
            changeControlButtonText.text = "Управление:\nKлавиатура + мышь";
            return;
        }
        if (playerController.GetCurrentControllerType() == PlayerController.ControllerType.mouseAndKeyboard)
        {
            gameController.player.GetComponent<PlayerController>().SetController(PlayerController.ControllerType.keyboard);
            changeControlButtonText.text = "Управление:\nKлавиатура";
            return;
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
