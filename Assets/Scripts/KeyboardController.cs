using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardController", menuName = "PlayerControllers/KeyboardController", order = 1)]
public class KeyboardController: Controller
{
    public override void ControlsUpdate()
    {
        if (Input.GetAxisRaw("KeyboardShoot") > 0) {
            player.Shoot();
        }

        if (Input.GetAxisRaw("KeyboardMoveForward") > 0)
        {
            player.MoveForward();
        }

        if (Input.GetAxisRaw("KeyboardRotation") != 0)
        {
            player.Rotate(Input.GetAxisRaw("KeyboardRotation"));
        }
    }
}
