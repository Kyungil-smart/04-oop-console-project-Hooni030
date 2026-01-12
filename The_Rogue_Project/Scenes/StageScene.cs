using System.Numerics;
using System.Runtime.InteropServices;

public class StageScene : Scene
{
    // 스테이지 난이도
    private int _stageLevel;
    // 스테이지 필드 크기
    public static readonly int Field_Width = 13;
    public static readonly int Field_Height = 11;
    // 스테이지 필드 배열 할당
    private Tile[,] _field = new Tile[Field_Height, Field_Width];
    // 필드 출력 좌표 선언
    private int mapPosX = 27;
    private int mapPosY = 7;
    // 시간 UI 크기 및 좌표 선언
    private Ractangle TimeUI = new Ractangle(0, 0, 27, 7);
    // 벽 객체 선언
    public static Wall wall = new Wall();

    // 목표 시간 및 현재 진행된 시간 변수 선언
    public static int purposeTime = 0;
    private int _second = 0;

    // 플레이어 객체 선언
    private PlayerCharacter _player;
    // 승리 시간 및 생존 시간 변수 선언
    private int _victoryTime;
    private double _survivalTime;

    // 몬스터, 총알, 경험치 볼 리스트 선언
    private readonly List<Monster> _monsters = new List<Monster>();
    private readonly List<Bullet> _bullets = new List<Bullet>();
    private readonly List<ExpOrb> _orbs = new List<ExpOrb>();

    // 난이도에 따른 몬스터 랜덤 객체 선언
    private Random _random = new Random();
    // 몬스터 스폰 타이머 및 스폰 간격 변수 선언
    private float _spawnTimer;
    private float _spawnInterval;

    // 몬스터 스탯 변수 선언
    private int _monaterHP;
    private int _monsterDamage;
    private float _monsterMoveInterval;
    // 총알 속도 변수 선언
    private float _bulletSpeed;

    private bool _isBackToMenu = false;

    // 스테이지 씬 생성자
    public StageScene(PlayerCharacter player, int level) => Init(player, level);

    // 스테이지 씬 초기화 함수
    public void Init(PlayerCharacter player, int level)
    {
        // 플레이어 객체 할당
        _player = player;
        // 스테이지 난이도 할당
        _stageLevel = level;

        // 필드 타일 초기화
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                _field[y, x] = new Tile(new Vector(x,y));
            }
        }
    }
    
    // 씬 진입 시 호출되는 메서드
    public override void Enter()
    {
        // 이전 스테이지 필드 타일 초기화
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            for (int x = 0; x < _field.GetLength(1); x++)
            {
                _field[y, x] = new Tile(new Vector(x, y));
            }
        }
        // 벽 배치
        SetWalls();
        // 몬스터, 총알, 경험치 볼 리스트 초기화
        _monsters.Clear();
        _bullets.Clear();
        _orbs.Clear();

        _spawnTimer = 0f;
        _spawnInterval = 0f;

        // 플레이어 스탯 초기화
        PlayerStatInit();
        // 몬스터 스탯 초기화
        MonsterStatInit();

        // 플레이어 필드 및 초기 위치 설정
        _player.Field = _field;
        // 플레이어 초기 위치 설정
        _player.Position = new Vector(Field_Width/2, Field_Height/2);
        // 플레이어를 필드에 배치
        _field[_player.Position.Y, _player.Position.X].OnTileObject = _player;

        // 플레이어 총알 발사 이벤트 핸들러 등록
        _player.OnShoot += HandleShoot;
    }
    // 씬 업데이트 메서드
    public override void Update()
    {
        // 플레이어 업데이트
        _player.Update();

        double deltaTime = Time.DeltaTime;

        _survivalTime += deltaTime;

        UpdateSpawn(deltaTime);
        UpdateBullet(deltaTime);
        UpdateMonster(deltaTime);

    }

    private void PlayerStatInit()
    {
        // 난이도별 플레이어 기본 스탯
        switch (_stageLevel)
        {
            case 0:
                _player.MaxHp = 7;
                _player.CurrentShield = 1;
                _player.MaxShield = 3;
                _player.AttackPoint = 2;
                break;

            case 1:
                _player.MaxHp = 5;
                _player.CurrentShield = 0;
                _player.MaxShield = 3;
                _player.AttackPoint = 1;
                break;

            default:
                _player.MaxHp = 4;
                _player.CurrentShield = 0;
                _player.MaxShield = 2;
                _player.AttackPoint = 1;
                break;
        }


        _player.HP.Value = _player.MaxHp;
        // 총알 속도 설정
        _bulletSpeed = 0.2f;
    }

    private void MonsterStatInit()
    {
        // 난이도별 몬스터 스탯 및 스폰 간격 설정
        switch (_stageLevel)
        {
            case 0:
                _monaterHP = 4;
                _monsterDamage = 1;
                _monsterMoveInterval = 1.0f;
                _spawnInterval = 3f;
                purposeTime = 60;
                break;
            case 1:
                _monaterHP = 6;
                _monsterDamage = 2;
                _monsterMoveInterval = 0.8f;
                _spawnInterval = 2.25f;
                purposeTime = 90;
                break;
            default:
                _monaterHP = 7;
                _monsterDamage = 3;
                _monsterMoveInterval = 0.6f;
                _spawnInterval = 1.5f;
                purposeTime = 120;
                break;
        }
    }

    // 일정 시간마다 몬스터 스폰 메서드 호출
    private void UpdateSpawn(double deltaTime)
    {
        // 몬스터 스폰 타이머 증가
        _spawnTimer += (float)deltaTime;
        // 몬스터 스폰 타이머가 스폰 간격 이상일 때 몬스터 스폰
        if (_spawnTimer >= _spawnInterval)
        {
            // 스폰 타이머에서 스폰 간격만큼 감소
            _spawnTimer -= _spawnInterval;
            SpawnMonster();
        }
    }
    // 몬스터 스폰 메서드
    private void SpawnMonster()
    {
        // 몬스터 스폰 시도 횟수
        int attempt = 30;
        // 몬스터 스폰 시도
        for (int i = 0; i < attempt; i++)
        {
            // 랜덤한 스폰 위치 선택
            int spawn = _random.Next(0, 4);
            // 스폰 위치 좌표 선언
            int x, y;
            // 스폰 위치 좌표 계산
            switch (spawn)
            {
                // 상
                case 0:
                    x = 6;
                    y = 0;
                    break;
                // 하
                case 1:
                    x = 6;
                    y = Field_Height - 2;
                    break;
                // 좌
                case 2:
                    x = 0;
                    y = 5;
                    break;
                // 우
                case 3:
                    x = Field_Width - 2;
                    y = 5;
                    break;
                default:
                    x = 0;
                    y = 0;
                    break;
            }

            // 스폰 위치가 비어있지 않으면 다음 시도
            if (_field[y, x].OnTileObject != null) continue;
            // 몬스터 객체 생성 및 필드에 배치
            Monster monster = new Monster(_monaterHP, _monsterDamage, _monsterMoveInterval);
            // 몬스터 위치 설정
            monster.Position = new Vector(x, y);
            // 몬스터 리스트 추가
            _monsters.Add(monster);
            return;
        }
    }
    // 벽 생성 메서드
    private void SetWalls()
    {
        // 필드 테두리 가로 세로 크기 선언
        int height = _field.GetLength(0);
        int width = _field.GetLength(1);

        // 테두리 벽 배치
        for (int x = 0; x < width; x++)
        {
            if (x == 6) continue;
            _field[0, x].OnTileObject = wall;
            _field[height - 1, x].OnTileObject = wall;
        }
        for(int y = 0; y < height; y++)
        {
            if (y == 5) continue;
            _field[y, 0].OnTileObject = wall;
            _field[y, width - 1].OnTileObject = wall;
        }
    }

    // 씬 렌더링 메서드
    public override void Render()
    {
        // 필드 출력
        PrintField(mapPosX, mapPosY);
        // 플레이어 출력
        _player.Render();
        // 시간 UI 출력
        PrintTimeUI(0, 0);
    }
    // 씬 종료 시 호출되는 메서드
    public override void Exit()
    {
        // 플레이어 총알 발사 이벤트 핸들러 해제
        _player.OnShoot -= HandleShoot;
        // 플레이어를 필드에서 제거
        _field[_player.Position.Y, _player.Position.X].OnTileObject = null;
        _player.Field = null;
    }
    // 플레이어 총알 발사 이벤트 핸들러
    private void HandleShoot(Vector direction, int damage)
    {
        // 총알 발사 시작 위치 계산
        // 플레이어 위치에서 발사 방향으로 한 칸 이동한 위치
        Vector start = _player.Position + direction;

        // 바깥/벽은 발사 불가
        if (start.X < 1 || start.X > Field_Width - 2 || start.Y < 1 || start.Y > Field_Height - 2)
            return;
        // 시작 위치의 타일 객체 재선언
        GameObject obj = _field[start.Y, start.X].OnTileObject;

        // 바로 앞이 벽이면 무시
        if (obj is Wall) return;

        // 바로 앞이 몬스터면 즉시 피격 처리
        if (obj is Monster m)
        {
            m.TakeDamage(damage);
            if (m.IsDead)
                KillMonster(m);
            return;
        }

        // 빈칸일 때만 총알 생성
        if (obj != null) return;

        // 총알 객체 생성 및 필드에 배치
        Bullet bullet = new Bullet(start, direction, damage, _bulletSpeed);
        // 총알을 시작 위치에 배치
        _field[start.Y, start.X].OnTileObject = bullet;
        // 총알 리스트에 추가
        _bullets.Add(bullet);
    }

    // 총알 업데이트 메서드
    private void UpdateBullet(double time)
    {
        // 총알 리스트를 순회하며 업데이트
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            // 현재 총알 가져오기
            Bullet bullet = _bullets[i];
            // 총알의 쿨타임 타이머 증가
            bullet.FireTimer += (float)time;
            // 총알이 제거 되었는지 판단
            bool removed = false;

            // 총알의 쿨타임이 발사 간격 이상일 때까지 반복
            while (bullet.FireTimer >= bullet.FireInterval)
            {
                // 쿨타임 타이머에서 발사 간격만큼 감소
                bullet.FireTimer -= bullet.FireInterval;

                // 다음 위치 계산
                Vector nextPos = bullet.Position + bullet.Direction;

                // 필드 경계 검사 맵 밖으로 나가면 총알 제거
                if (nextPos.X < 1 || nextPos.X >= Field_Width - 2 ||
                   nextPos.Y < 1 || nextPos.Y >= Field_Height - 2)
                {
                    RemoveBullet(bullet);
                    removed = true;
                    break;
                }
                // 다음 위치의 타일 객체 재선언
                GameObject hitBullet = _field[nextPos.Y, nextPos.X].OnTileObject;

                // 다음 위치에 벽, 몬스터, 경험치 볼이 있으면 충돌 처리
                if (hitBullet is Wall)
                {
                    RemoveBullet(bullet);
                    removed = true;
                    break;
                }
                if (hitBullet is Monster m)
                {
                    m.TakeDamage(bullet.Damage);
                    RemoveBullet(bullet);
                    if (m.IsDead)
                        KillMonster(m);
                    RemoveBullet(bullet);
                    removed = true;
                    break;
                }
                if (hitBullet is ExpOrb orb)
                {
                    RemoveBullet(bullet);
                }
                // 현재 위치에서 총알 제거
                if (_field[bullet.Position.Y, bullet.Position.X].OnTileObject == bullet)
                    _field[bullet.Position.Y, bullet.Position.X].OnTileObject = null;
                // 총알을 다음 위치로 이동
                bullet.Position = nextPos;
                // 다음 위치에 총알 배치
                _field[nextPos.Y, nextPos.X].OnTileObject = bullet;
            }
            // 총알이 제거되었으면 다음 총알로 넘어감
            if (removed)
                continue;
        }
    }
    // 총알 제거 메서드
    private void RemoveBullet(Bullet bullet)
    {
        // 현재 위치에서 총알 제거
        if (_field[bullet.Position.Y, bullet.Position.X].OnTileObject == bullet)
            _field[bullet.Position.Y, bullet.Position.X].OnTileObject = null;
        // 총알 리스트에서 제거
        _bullets.Remove(bullet);
    }
    // 몬스터 업데이트 메서드
    private void UpdateMonster(double time)
    {
        // 몬스터 리스트를 순회하며 업데이트
        for (int i = _monsters.Count - 1; i >= 0; i--)
        {
            // 현재 몬스터 가져오기
            Monster m = _monsters[i];
            // 현재 몬스터의 다음 이동 시간 감소
            m.StepTimer -= (float)time;

            // 다음 이동 시간이 아직 남아있으면 다음 몬스터로 넘어감
            if (m.StepTimer > 0f)
                continue;
            // 다음 이동 시간 초기화
            m.StepTimer = m.StepInterval;
            // 몬스터 이동 시도
            TryMonsterMove(m);
        }
    }
    // 몬스터 이동 메서드
    private void TryMonsterMove(Monster monster)
    {
        // 플레이어와 몬스터의 위치 차이 계산
        Vector directionToPlayer = _player.Position - monster.Position;

        // X 및 Y 축 방향으로의 거리 계산
        int moveX = (int)Math.Abs(directionToPlayer.X);
        int moveY = (int)Math.Abs(directionToPlayer.Y);
        // 이동 방향 결정
        Vector moveDirection;

        // 더 먼 쪽으로 이동
        if (moveX > moveY)
        {
            moveDirection = new Vector(Math.Sign(directionToPlayer.X), 0);
        }
        else if (moveY > moveX)
        {
            moveDirection = new Vector(0, Math.Sign(directionToPlayer.Y));
        }
        else
        {
            // 거리가 같을 경우 랜덤으로 방향 결정
            if (_random.Next(2) == 0)
                moveDirection = new Vector(Math.Sign(directionToPlayer.X), 0);
            else
                moveDirection = new Vector(0, Math.Sign(directionToPlayer.Y));
        }

        // 몬스터가 이동 또는 공격 시도
        TryMonsterAttack(monster, moveDirection);
    }

    // 몬스터가 움직일 수 있는지 확인하는 메서드
    // 움직일 수 있으면 대상 공격 또는 이동 수행
    private bool TryMonsterAttack(Monster monster, Vector dir)
    {
        // 다음 위치 계산
        Vector nextPos = monster.Position + dir;

        // 필드 경계 검사
        if (nextPos.X < 1 || nextPos.X >= Field_Width - 2||
            nextPos.Y < 1 || nextPos.Y >= Field_Height - 2)
            return false;

        // 다음 위치의 타일 객체 재선언
        GameObject attackTile = _field[nextPos.Y, nextPos.X].OnTileObject;

        // 공격 대상이 플레이어인 경우 플레이어에게 데미지 입히기
        if (attackTile != null)
        {
            if (attackTile is PlayerCharacter)
            {
                _player.TakeDamage(monster.Damage);
                return false;
            }
            return false;
        }

        // 다음 위치가 벽, 몬스터, 총알, 경험치 볼인 경우 이동 불가
        if (attackTile is Wall || attackTile is Monster || attackTile is Bullet || attackTile is ExpOrb)
            return false;

        // 현재 위치에서 몬스터 제거
        if (_field[monster.Position.Y, monster.Position.X].OnTileObject == monster)
            _field[monster.Position.Y, monster.Position.X].OnTileObject = null;

        // 몬스터를 다음 위치로 이동
        monster.Position = nextPos;
        // 다음 위치에 몬스터 배치
        _field[nextPos.Y, nextPos.X].OnTileObject = monster;

        return true;
    }

    // 몬스터 처치 시 실행 메서드
    private void KillMonster(Monster monster)
    {
        Vector pos = monster.Position;
        if (_field[pos.Y, pos.X].OnTileObject == monster)
            _field[pos.Y, pos.X].OnTileObject = null;

        _monsters.Remove(monster);
         
        DropExpOrb(pos);
    }

    // 몬스터 처치 시 경험치 볼 드랍 메서드
    private void DropExpOrb(Vector pos)
    {
        // 이미 타일에 오브가 있으면 드랍하지 않음
        if (_field[pos.Y, pos.X].OnTileObject != null)
            return;
        // 경험치 오브 객체 생성

        ExpOrb orb = new ExpOrb(2 + _stageLevel, RemoveExpOrb);
        // 경험치 오브 위치 설정
        orb.Position = pos;

        // 경험치 오브 리스트에 추가 및 필드에 배치
        _orbs.Add(orb);
        _field[pos.Y, pos.X].OnTileObject = orb;
    }

    // EXP 오브 제거 메서드
    private void RemoveExpOrb(ExpOrb orb)
    {
        if (_field[orb.Position.Y, orb.Position.X].OnTileObject == orb)
            _field[orb.Position.Y, orb.Position.X].OnTileObject = null;

        _orbs.Remove(orb);
    }

    // 필드 출력 메서드
    private void PrintField(int pX, int pY)
    {
        // 필드 출력
        for (int y = 0; y < _field.GetLength(0); y++)
        {
            Console.SetCursorPosition(pX, pY + y);

            for (int x = 0; x < _field.GetLength(1); x++)
            {
                // 현재 타일이 플레이어 객체인 경우 출력
                if (_field[y, x].OnTileObject == _player)
                    _player.Symbol.Print(ConsoleColor.Red, ConsoleColor.DarkGray);
                else if (_field[y, x].OnTileObject is Wall)
                    wall.Symbol.Print(default, ConsoleColor.DarkGray);
                else if (_field[y, x].OnTileObject is Monster monster)
                    monster.Symbol.Print(ConsoleColor.Green, ConsoleColor.DarkGray);
                else if (_field[y, x].OnTileObject is Bullet bullet)
                    bullet.Symbol.Print(ConsoleColor.Yellow, ConsoleColor.DarkGray);
                else if (_field[y, x].OnTileObject is ExpOrb orb)
                    orb.Symbol.Print(ConsoleColor.Magenta, ConsoleColor.DarkGray);
                else
                    _field[y, x].Print();
            }
            Console.WriteLine();
        }
    }
    // 시간 UI 출력 메서드
    private void PrintTimeUI(int x, int y)
    {
        // 시간 UI 출력
        TimeUI.Draw(ConsoleColor.DarkCyan);

        int survivaltime = (int)_survivalTime;
        int remainTime = purposeTime - survivaltime;
        if (remainTime < 0) remainTime = 0;

        // 난이도 출력
        Console.SetCursorPosition(x + 3, y + 1);
        $"난이도 : {_stageLevel + 1}단계".Print(ConsoleColor.White);

        // 시간 출력
        Console.SetCursorPosition(x + 2, y + 3);
        "🕒목표 시간 ⏳생존 시간".Print(ConsoleColor.Cyan);
        // 목표 시간 및 생존 시간 출력
        Console.SetCursorPosition(x + 7, y + 4);
        string timeString = $"{_victoryTime} 초      {(int)_survivalTime} 초";
        timeString.Print(ConsoleColor.Blue);
    }
}