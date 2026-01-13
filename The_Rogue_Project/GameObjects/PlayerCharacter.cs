using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

public class PlayerCharacter : GameObject
{
    // 플레이어 레벨, 경험치, 체력 변동 시 메서드를 불러올 수 있도록 선언
    public ObservableProperty<int> Level;
    public ObservableProperty<float> Exp;
    public ObservableProperty<float> HP;

    // 스테이지 필드 저장할 타일 배열 변수 선언
    public Tile[,] Field { get; set; }

    // 플레이어 인스턴스 생성시 초기화
    public PlayerCharacter() => Init();
    public static Vector _currentPlayerLoc = new Vector(StageScene.Field_Width / 2, StageScene.Field_Height / 2);

    // 스텟 UI 창 및 크기 선언
    private Ractangle StatUIWindow;
    private const int Stat_UI_Width = 27;
    private const int Stat_UI_Height = 11;

    // 레벨 UI 창 및 크기 선언
    private Ractangle LevelUIWindow;
    private const int Level_UI_Width = 26;
    private const int Level_UI_Height = 7;

    // 플레이어 레벨 및 경험치 관련 선언
    private string LevelIcon = "⭐";
    private float _currentExp = 0;
    private int MaxExp;
    private float _expPercent;
    private string ExpIcon = "✨";
    private string ExpBar = "🟩";
    // 플레이어 체력 관련 선언
    public int MaxHp { get; set; }
    private float _hpPercent;
    private string _hpBar = "🟥";

    // 플레이어 공격 관련 선언
    public int AttackPoint { get; set; }
    private string _attackIcon = "🗡️";
    // 플레이어가 보고 있는 방향 변수
    public Vector FaceVector { get; private set; } = Vector.Right;
    public event Action<Vector, int> OnShoot;
    // 플레이어 총알 발사 쿨타임 선언
    private const float ShootInterval = 0.20f;
    private double _shootCooldown;
    // 플레이어 쉴드 관련 선언
    public int MaxShield { get; set; }
    public int CurrentShield { get; set; }
    private string _ShieldBar = "🛡";

    // 플레이어 객체 초기화 메서드
    private void Init()
    {
        Symbol = "🌟";

        StatInit();
    }
    // 플레이어 스탯 초기화메서드
    public void StatInit()
    {
        MaxExp = 4;
        StatUIWindow = new Ractangle(0, 7, Stat_UI_Width, Stat_UI_Height);
        LevelUIWindow = new Ractangle(27, 0, Level_UI_Width, Level_UI_Height);

        Level = new ObservableProperty<int>(1);
        Exp = new ObservableProperty<float>(0);
        HP = new ObservableProperty<float>();

        Exp.AddListener(NextExp);
        Level.AddListener(NextLevel);
        HP.AddListener(null);
    }
    // 플레이어 스탯 클리어 메서드
    public void StatClear()
    {
        HP.Value = 0;
        _hpPercent = 0;
        Level.Value = 1;
        Exp.Value = 0;
        _expPercent = 0;

        HP.ClearListener();
        Exp.ClearListener();
        Level.ClearListener();
    }
    // 플레이어 상태 업데이트 메서드
    public void Update()
    {
        // 총알 발사 쿨타임 감소
        if (_shootCooldown > 0d)
        {
            _shootCooldown -= Time.DeltaTime;
            if (_shootCooldown < 0d) _shootCooldown = 0d;
        }
        // 스페이스바 입력시 공격 (총알 발사)
        if (InputManager.IsCorrectkey(ConsoleKey.Spacebar))
        {
            if (_shootCooldown <= 0f)
            {
                OnShoot?.Invoke(FaceVector, AttackPoint);
                _shootCooldown = ShootInterval;
            }
        }
        // 방향키 입력시 이동
        if (InputManager.IsCorrectkey(ConsoleKey.UpArrow))
        {
            FaceVector = Vector.Up;
            Move(Vector.Up);
        }
        if (InputManager.IsCorrectkey(ConsoleKey.DownArrow))
        {
            FaceVector = Vector.Down;
            Move(Vector.Down);
        }
        if (InputManager.IsCorrectkey(ConsoleKey.LeftArrow))
        {
            FaceVector = Vector.Left;
            Move(Vector.Left);
        }
        if (InputManager.IsCorrectkey(ConsoleKey.RightArrow))
        {
            FaceVector = Vector.Right;
            Move(Vector.Right);
        }
        // ESC 입력시 메인메뉴로 이동
        if (InputManager.IsCorrectkey(ConsoleKey.Escape))
        {
            SceneManager.ChangeScene("MainMenu");
        }
    }

    // 플레이어 이동 메서드
    private void Move(Vector direction)
    {
        // 필드가 비어있으면 리턴
        if (Field == null) return;
        // 현재 위치와 다음 위치 계산
        Vector current = Position;
        Vector nextPos = current + direction;

        // 1. 맵 바깥 여부 체크
        if (nextPos.X < 1 || nextPos.X > StageScene.Field_Width - 2 ||
            nextPos.Y < 1 || nextPos.Y > StageScene.Field_Height - 2)
            return;
        // 2. 다음 타일 오브젝트 확인
        GameObject nextTileObject = Field[nextPos.Y, nextPos.X].OnTileObject;

        // 3. 이동불가 오브젝트 체크
        if (nextTileObject is Wall || nextTileObject is Monster || nextTileObject is Bullet)
            return;
        // 4. 상호작용 오브젝트 체크
        if (nextTileObject != null)
        {
            if (nextTileObject is IInteractable)
            {
                (nextTileObject as IInteractable)?.Interact(this);
            }
        }
        // 5. 이동 처리
        Field[Position.Y, Position.X].OnTileObject = null;
        Field[nextPos.Y, nextPos.X].OnTileObject = this;
        // 6. 위치 갱신
        Position = nextPos;
    }

    // 플레이어가 데미지를 입는 메서드
    public void TakeDamage(int damage)
    {
        // 음수 데미지 무시
        if (damage <= 0) return;

        // 방어막 먼저 소모
        if (CurrentShield > 0)
        {
            int shieldRemain = CurrentShield - damage;
            if (shieldRemain >= 0)
            {
                CurrentShield = shieldRemain;
                return;
            }

            // 방어막이 다 깨졌을 때 남은 데미지 계산
            CurrentShield = 0;
            // 남은 데미지만큼 체력에서 차감
            damage = -shieldRemain; 
        }

        HP.Value -= damage;
        if (HP.Value <= 0)
        {
            HP.Value = 0;
            SceneManager.ChangeScene("Dead");
        }
    }

    
    public void Render()
    {
        StatUIWindow.Draw(ConsoleColor.Yellow);
        LevelUIWindow.Draw(ConsoleColor.DarkYellow);
        RenderCharacterUI();
    }

    // 캐릭터UI 출력 메서드
    public void RenderCharacterUI()
    {
        RenderHP(2, 8);
        RenderShield(2, 11);
        RenderDamage(2, 14);
        RenderLevel(30, 1);
    }

    // 캐릭터 체력 UI 출력 메서드
    public void RenderHP(int x, int y)
    {
        string hpUI = $"체력 : {HP.Value} / {MaxHp}";

        Console.SetCursorPosition(x, y);
        hpUI.Print();

        Console.SetCursorPosition(x, y + 1);
        _hpPercent = HP.Value / MaxHp * 10;
        for (int i = 0; i < _hpPercent; i++)
        {
            _hpBar.Print();
        }
    }
    // 캐릭터 쉴드 UI 출력 메서드
    public void RenderShield(int x, int y)
    {
        string shieldUI = $"방어막 : {CurrentShield} / {MaxShield}";
        Console.SetCursorPosition(x, y);
        shieldUI.Print();
        Console.SetCursorPosition(x, y + 1);
        for (int i = 0; i < CurrentShield; i++)
        {
            _ShieldBar.Print();
        }
    }
    // 캐릭터 공격력 UI 출력 메서드
    public void RenderDamage(int x, int y)
    {
        Console.SetCursorPosition(x, y);
        string damageUI = $"공격력 : {AttackPoint}";
        damageUI.Print();

        Console.SetCursorPosition(x, y+1);
        for(int i = 0; i< AttackPoint/2; i++)
        {
            _attackIcon.Print();
        }
    }
    // 캐릭터 레벨, Exp UI 출력 메서드
    public void RenderLevel(int x, int y)
    {
        string levelUI = $"{LevelIcon} Level : {Level.Value}";

        Console.SetCursorPosition(x, y);
        levelUI.Print();

        string expUI = $"{ExpIcon} Exp {Exp.Value} / {MaxExp}";
        Console.SetCursorPosition(x, y + 2);
        expUI.Print();

        Console.SetCursorPosition(x, y + 3);
        for (int i = 0; i < _expPercent; i++)
        {
            ExpBar.Print();
        }
    }

    // 캐릭터의 경험치가 변동되면 실행되는 메서드
    public void NextExp(float exp)
    {
        _expPercent = Exp.Value / MaxExp * 10;
        if (_expPercent >= 10)
        {
            Exp.Value = Exp.Value - MaxExp;
            Level.Value++;
        }
    }

    // 캐릭터의 레벨이 오를 때 실행되는 메서드
    public void NextLevel(int level)
    {
        MaxExp = MaxExp/2 * level / 3 + 3;
        MaxHp += level/2 + 1;
        if (HP.Value + HP.Value / 3 < MaxHp)
            HP.Value += MaxHp / 3;
        else
            HP.Value = MaxHp;

        AttackPoint = AttackPoint + level / 3;
    }
}