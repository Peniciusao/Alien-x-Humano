using UnityEngine;

public class PlayerMovement2 : PlayerMovementAnimation
{
    public float speedY = 5f;
    public float speedX = 5f;

    protected override Vector2 GetVelocity(Vector2 input)
    {
        return new Vector2(input.x * speedX, input.y * speedY);
    }
}
