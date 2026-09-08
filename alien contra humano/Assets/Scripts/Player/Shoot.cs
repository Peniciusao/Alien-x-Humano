using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject bala;
    public Transform pontoTiro;
    public WeaponManager armas;

    public float cooldownPistola = 0.2f;
    public float cooldownShotgun = 0.8f;

    private float proximoTiro;

    void Update()
    {
        if (Mouse.current != null &&
            Mouse.current.rightButton.isPressed &&
            Time.time >= proximoTiro)
        {
            Atirar();
        }
    }

    Vector2 DirecaoDoMouse()
    {
        Vector2 mouseTela = Mouse.current.position.ReadValue();

        Vector3 mouseMundo = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseTela.x, mouseTela.y, -Camera.main.transform.position.z)
        );

        return ((Vector2)mouseMundo - (Vector2)transform.position).normalized;
    }

    void Atirar()
    {
        if (armas.armaAtual == 1)
        {
            TiroNormal();
            proximoTiro = Time.time + cooldownPistola;
        }
        else if (armas.armaAtual == 2)
        {
            Shotgun();
            proximoTiro = Time.time + cooldownShotgun;
        }
    }

    void TiroNormal()
    {
        Vector2 direcao = DirecaoDoMouse();

        GameObject objeto = Instantiate(
            bala,
            pontoTiro.position,
            Quaternion.identity
        );

        Bullet bullet = objeto.GetComponent<Bullet>();

        if (bullet != null)
            bullet.DefinirDirecao(direcao);
    }

    void Shotgun()
    {
        int quantidade = 7;
        float espalhamento = 50f;

        Vector2 direcao = DirecaoDoMouse();

        float anguloBase =
            Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;

        for (int i = 0; i < quantidade; i++)
        {
            float angulo = anguloBase +
                Random.Range(-espalhamento / 2f, espalhamento / 2f);

            Vector2 direcaoTiro = new Vector2(
                Mathf.Cos(angulo * Mathf.Deg2Rad),
                Mathf.Sin(angulo * Mathf.Deg2Rad)
            );

            GameObject objeto = Instantiate(
                bala,
                pontoTiro.position,
                Quaternion.identity
            );

            Bullet bullet = objeto.GetComponent<Bullet>();

            if (bullet != null)
                bullet.DefinirDirecao(direcaoTiro);
        }
    }
}