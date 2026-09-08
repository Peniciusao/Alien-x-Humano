using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public int armaAtual = 1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            armaAtual = 1;
            Debug.Log("Pistola equipada");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            armaAtual = 2;
            Debug.Log("Shotgun equipada");
        }
    }
}