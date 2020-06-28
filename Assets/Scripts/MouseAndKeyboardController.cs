using UnityEngine;

[CreateAssetMenu(fileName = "MouseAndKeyboardController", menuName = "PlayerControllers/MouseAndKeyboardController", order = 1)]
public class MouseAndKeyboardController : Controller
{
    public override void ControlsUpdate()
    {
        Rotate();

        if (Input.GetAxisRaw("MouseAndKeyboardMoveForward") > 0) {
            player.MoveForward();
        }

        if (Input.GetAxisRaw("MouseAndKeyboardShoot") > 0 && !shootButtonDown)
        {
            player.Shoot();
            shootButtonDown = true;
        }
        if (Input.GetAxisRaw("MouseAndKeyboardShoot") == 0)
        {
            shootButtonDown = false;
        }
    }

    private void Rotate() {
        Camera cam = player.GetGameController().mainCamera;
        float screenRatio = (float)Screen.width / Screen.height;
        Vector3 cursorPosition = new Vector3(((Input.mousePosition.x - Screen.width / 2) / (Screen.width / 2)) * cam.orthographicSize * screenRatio
            , 0, ((Input.mousePosition.y - Screen.height / 2) / (Screen.height / 2)) * cam.orthographicSize);

        player.Rotate(cursorPosition);
    }
}
