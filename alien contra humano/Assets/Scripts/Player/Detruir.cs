using UnityEngine;

public class Detruir : MonoBehaviour
{
    // Tempo aproximado que dura a animação
    public float lifetime = 0.5f;

    void Start()
    {
        // Destrói o objeto assim que ele é criado e o tempo acaba
        Destroy(gameObject, lifetime);
    }
}