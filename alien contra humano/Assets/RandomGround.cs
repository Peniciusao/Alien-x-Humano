using UnityEngine;

public class RandomGround : MonoBehaviour
{
    public Sprite textura;

    public int largura = 20;
    public int altura = 20;

    public float tamanho = 1f;

    void Start()
    {
        CriarChao();
    }

    void CriarChao()
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                GameObject bloco = new GameObject("GroundTile");

                bloco.transform.parent = transform;

                bloco.transform.position = new Vector3(
                    x * tamanho,
                    y * tamanho,
                    0
                );

                // Rotação aleatória de 90 graus
                int rotacao = Random.Range(0, 4);

                bloco.transform.rotation = Quaternion.Euler(
                    0,
                    0,
                    rotacao * 90f
                );

                SpriteRenderer sprite = bloco.AddComponent<SpriteRenderer>();

                sprite.sprite = textura;
            }
        }
    }
}