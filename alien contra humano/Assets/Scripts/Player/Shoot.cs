using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public GameObject bala;
    public Transform pontoTiro;
    public WeaponManager armas;
    public float cooldownPistola = 0.2f;
    public float cooldownShotgun = 0.8f;

    [Header("Munição da pistola")]
    [Min(1)] public int capacidadePistola = 12;
    [Tooltip("Reserva ao iniciar. Durante o jogo, alterar este campo substitui a reserva atual. Para salvar o valor, edite fora do modo Play. O Inspector tem prioridade sobre o valor inicial do código.")]
    [Min(0)] public int reservaInicialPistola = 144;
    [Min(0.01f)] public float tempoRecargaPistola = 1.5f;
    [Header("Munição da shotgun")]
    [Min(1)] public int capacidadeShotgun = 6;
    [Tooltip("Reserva ao iniciar. Durante o jogo, alterar este campo substitui a reserva atual. Para salvar o valor, edite fora do modo Play. O Inspector tem prioridade sobre o valor inicial do código.")]
    [Min(0)] public int reservaInicialShotgun = 90;
    [Min(0.01f)] public float tempoRecargaShotgun = 2.5f;
    [Header("Interface")]
    public bool mostrarMunicao = true;

    private readonly int[] carregadores = new int[2];
    private readonly int[] reservas = new int[2];
    private readonly int[] reservasConfiguradas = new int[2];
    private readonly float[] proximosTiros = new float[2];
    private int armaRecarregando = -1;
    private float fimRecarga;
    private AmmoDisplay interfaceMunicao;

    public int IndiceArma => armas != null && armas.armaAtual == 2 ? 1 : 0;
    public int MunicaoAtual => carregadores[IndiceArma];
    public int ReservaAtual => reservas[IndiceArma];
    public string NomeArma => IndiceArma == 0 ? "pistola" : "espingarda";
    public bool Recarregando => armaRecarregando >= 0;
    public float TempoRestanteRecarga => Recarregando ? Mathf.Max(0f, fimRecarga - Time.time) : 0f;

    void Awake()
    {
        if (armas == null) armas = GetComponent<WeaponManager>();
        carregadores[0] = Capacidade(0);
        carregadores[1] = Capacidade(1);
        reservas[0] = Mathf.Max(0, reservaInicialPistola);
        reservas[1] = Mathf.Max(0, reservaInicialShotgun);
        reservasConfiguradas[0] = reservas[0];
        reservasConfiguradas[1] = reservas[1];
    }

    void Start()
    {
        if (mostrarMunicao)
        {
            interfaceMunicao = gameObject.AddComponent<AmmoDisplay>();
            interfaceMunicao.DefinirArma(this);
        }
    }

    int Capacidade(int indice) => Mathf.Max(1, indice == 0 ? capacidadePistola : capacidadeShotgun);

    void Update()
    {
        AtualizarReservasConfiguradas();
        if (Time.timeScale <= 0f) return;
        int indice = IndiceArma;
        // Trocar de arma cancela a recarga sem transferir ou criar munição.
        if (Recarregando && armaRecarregando != indice) armaRecarregando = -1;
        if (Recarregando && Time.time >= fimRecarga)
        {
            int quantidade = Mathf.Min(Mathf.Max(0, Capacidade(indice) - carregadores[indice]), reservas[indice]);
            carregadores[indice] += quantidade;
            reservas[indice] -= quantidade;
            armaRecarregando = -1;
        }
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame) Recarregar();
        if (Recarregando) return;
        if (carregadores[indice] == 0)
        {
            Recarregar();
            return;
        }
        if (Mouse.current != null && Mouse.current.rightButton.isPressed && Time.time >= proximosTiros[indice])
            Atirar(indice);
    }

    // Aplica somente mudanças na configuração, sem restaurar munição consumida a cada quadro.
    void AtualizarReservasConfiguradas()
    {
        for (int indice = 0; indice < reservas.Length; indice++)
        {
            int configurada = Mathf.Max(0, indice == 0 ? reservaInicialPistola : reservaInicialShotgun);
            if (configurada == reservasConfiguradas[indice]) continue;
            reservas[indice] = configurada;
            reservasConfiguradas[indice] = configurada;
        }
    }

    public void Recarregar()
    {
        int indice = IndiceArma;
        if (Recarregando || reservas[indice] <= 0 || carregadores[indice] >= Capacidade(indice)) return;
        armaRecarregando = indice;
        fimRecarga = Time.time + Mathf.Max(0.01f, indice == 0 ? tempoRecargaPistola : tempoRecargaShotgun);
    }

    // Permite que futuros itens de coleta reponham a reserva: 1 = pistola, 2 = shotgun.
    public void AdicionarMunicao(int arma, int quantidade)
    {
        if (arma < 1 || arma > 2 || quantidade <= 0) return;
        reservas[arma - 1] += quantidade;
    }

    void Atirar(int indice)
    {
        Camera cameraPrincipal = Camera.main;
        if (bala == null || pontoTiro == null || cameraPrincipal == null || bala.GetComponent<Bullet>() == null) return;
        Vector2 mouseTela = Mouse.current.position.ReadValue();
        Vector3 mouseMundo = cameraPrincipal.ScreenToWorldPoint(new Vector3(mouseTela.x, mouseTela.y, -cameraPrincipal.transform.position.z));
        Vector2 direcao = ((Vector2)mouseMundo - (Vector2)transform.position).normalized;
        if (direcao == Vector2.zero) return;

        int quantidade = indice == 0 ? 1 : 7;
        float anguloBase = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        for (int i = 0; i < quantidade; i++)
        {
            float angulo = anguloBase + (indice == 0 ? 0f : Random.Range(-25f, 25f));
            Vector2 direcaoTiro = new Vector2(Mathf.Cos(angulo * Mathf.Deg2Rad), Mathf.Sin(angulo * Mathf.Deg2Rad));
            GameObject objeto = Instantiate(bala, pontoTiro.position, Quaternion.identity);
            objeto.GetComponent<Bullet>().DefinirDirecao(direcaoTiro);
        }
        // Uma cartucheira consome somente uma unidade por disparo, independentemente dos projéteis.
        carregadores[indice]--;
        proximosTiros[indice] = Time.time + Mathf.Max(0.01f, indice == 0 ? cooldownPistola : cooldownShotgun);
    }

    void OnDisable()
    {
        armaRecarregando = -1;
        if (interfaceMunicao != null) interfaceMunicao.enabled = false;
    }

    void OnEnable()
    {
        if (interfaceMunicao != null) interfaceMunicao.enabled = true;
    }
}
