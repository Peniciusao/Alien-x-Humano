using System;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerMovement2 : MonoBehaviour
{
    public float speedY = 5f;
    public float speedX = 5f;
void Update()
{
    Console.WriteLine("sim");
    if (Input.GetKey(KeyCode.W))
    {
        transform.position += Vector3.up * speedY * Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.S))
    {
        transform.position += Vector3.down * speedY * Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.A))
    {
        transform.position += Vector3.left * speedX * Time.deltaTime;
    }

    if (Input.GetKey(KeyCode.D))
    {
        transform.position += Vector3.right * speedX * Time.deltaTime;
    }
}
}