using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private GameController gameController;
    public GameObject escMenu;
    public GameObject inGameInterface;
    public Text changeControlButtonText;
    public Button resumeButtorn;

    private bool menuTriggered = false;

    private void Update()
    {
        if (Input.GetAxisRaw("Menu") > 0 && menuTriggered == false) {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else {
                if (gameController.GetGameStatus())
                {
                    resumeButtorn.interactable = true;
                }
                else {
                    resumeButtorn.interactable = false;
                }

                PauseGame();
            }
            menuTriggered = true;
        }

        if (Input.GetAxisRaw("Menu") == 0) {
            menuTriggered = false;
        }
    }

    public void SetGameController(GameController _gameController)
    {
        gameController = _gameController;
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

    }
}
