using UnityEngine;

public class SmallAsteroid : Asteroid
{
    public override void Die()
    {
        gameObject.SetActive(false);
    }
}
