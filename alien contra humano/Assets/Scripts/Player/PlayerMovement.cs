using UnityEngine;

public class PlayerMovement : PlayerMovementAnimation
{
    public float speed = 5f;

    protected override Vector2 GetVelocity(Vector2 input)
    {
        return input * speed;
    }
}
