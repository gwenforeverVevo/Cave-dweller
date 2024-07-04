using Cave_dweller;
using SplashKitSDK;

public class Game
{
    private Player _player;
    private List<Monster> _monsters;
    //private List<Goblin> _goblins;
    //private List<Wolf> _wolfs;
    //private List<Spider> _spiders;
    private List<Bitmap> _playerRunRightFrames;
    private List<Bitmap> _playerRunLeftFrames;
    private Bitmap _playerRestBitmap;
    private Bitmap _cursorBitmap;
    private Bitmap _floorBitmap;
    private int _currentFrame;
    private double _frameDuration;
    private double _lastFrameTime;
    private bool _isMoving;
    private bool _showHitboxes;
    private SplashKitSDK.Timer _gameTimer;
    private bool _gameOver;
    private List<FloatingText> _floatingTexts;
    private Random _random;
    private double _timeSinceLastSpawn;
    private int _highScore;


    public Game()
    {

        _player = new Player(3, 10, new Vector2D() { X = 100, Y = 100 }, 0.6);
        _monsters = new List<Monster>();
        _random = new Random();
        //_goblins = new List<Goblin>
        //{
        //    new Goblin(new Vector2D() { X = 500, Y = 200 }),
        //    new Goblin(new Vector2D() { X = 500, Y = 300 })
        //};

        //_wolfs = new List<Wolf>
        //{
        //    new Wolf(new Vector2D() { X = 600, Y = 500 }),
        //    new Wolf(new Vector2D() { X = 600, Y = 400 })
        //};

        //_spiders = new List<Spider>
        //{
        //    new Spider(new Vector2D() { X = 600, Y = 500 }),
        //    new Spider(new Vector2D() { X = 600, Y = 400 })
        //};
        SpawnMonster();
        SpawnMonster();

        AssetLoader assetLoader = new AssetLoader();
        _playerRunRightFrames = assetLoader.LoadFrames("player_run_right", "asset/player_run", 20);
        _playerRunLeftFrames = assetLoader.LoadFrames("player_run_left", "asset/player_run", 20);
        _playerRestBitmap = assetLoader.LoadBitmap("player_rest", "asset/player_rest.png");
        _cursorBitmap = assetLoader.LoadBitmap("cursor", "asset/cursor.png");
        _floorBitmap = assetLoader.LoadBitmap("floor", "asset/floor.png");

        _currentFrame = 0;
        _frameDuration = 0.1; // Duration of each frame in seconds
        _lastFrameTime = SplashKit.CurrentTicks();
        _isMoving = false;
        _showHitboxes = false;

        _gameTimer = SplashKit.CreateTimer("game_timer");
        SplashKit.StartTimer(_gameTimer);
        _gameOver = false;
        _timeSinceLastSpawn = 0.0;
        _floatingTexts = new List<FloatingText>();
        _highScore = FileHelper.LoadHighScore();


    }

    private void SpawnMonster()
    {
        Vector2D spawnPosition;
        do
        {
            spawnPosition = new Vector2D
            {
                X = _random.NextDouble() * GameConstants.GameWidth,
                Y = _random.NextDouble() * GameConstants.GameHeight
            };
        } while (VectorUtils.DistanceTo(spawnPosition, _player.GetLocation()) < 100); // Ensure monsters spawn at least 100 units away

        Monster newMonster;
        int monsterType = _random.Next(3);
        switch (monsterType)
        {
            case 0:
                newMonster = new Goblin(spawnPosition);
                break;
            case 1:
                newMonster = new Wolf(spawnPosition);
                break;
            case 2:
                newMonster = new Spider(spawnPosition);
                break;
            default:
                newMonster = new Goblin(spawnPosition);
                break;
        }
        _monsters.Add(newMonster);
    }

    public void Update()
    {
        if (_gameOver)
        {
            return;
        }

        if (SplashKit.KeyTyped(KeyCode.HKey))
        {
            _showHitboxes = !_showHitboxes;
        }

        double currentTime = SplashKit.CurrentTicks();
        double deltaTime = (currentTime - _lastFrameTime) / 1000.0;
        _lastFrameTime = currentTime;

        _timeSinceLastSpawn += deltaTime;
        if (_timeSinceLastSpawn >= 2.0)
        {
            SpawnMonster();
            _timeSinceLastSpawn = 0.0;
        }
        //GameStateUpdater.UpdateGameState(_player, _goblins);
        //GameStateUpdater.UpdateGameState(_player, _wolfs);
        //GameStateUpdater.UpdateGameState(_player, _spiders);
        GameStateUpdater.UpdateGameState(_player, _monsters);
        ProjectileManager.UpdateProjectiles(_player, _monsters);


        int playerSpriteWidth = _playerRestBitmap.Width;
        int playerSpriteHeight = _playerRestBitmap.Height;
        InputHandler.HandleInput(_player, ref _isMoving, playerSpriteWidth, playerSpriteHeight);

        _player.HandleInput();
        _player.UpdateReloadAnimation();

        //foreach (Goblin goblin in _goblins)
        //{
        //    goblin.AttackPlayer(_player);
        //    if (_player.IsDead)
        //    {
        //        _gameOver = true;
        //        return;
        //    }
        //}

        //foreach (Wolf wolf in _wolfs)
        //{
        //    wolf.AttackPlayer(_player);
        //    if (_player.IsDead)
        //    {
        //        _gameOver = true;
        //        return;
        //    }
        //}

        //foreach (Spider spider in _spiders)
        //{
        //    spider.AttackPlayer(_player);
        //    if (_player.IsDead)
        //    {
        //        _gameOver = true;
        //        return;
        //    }
        //}

        foreach (Monster monster in _monsters)
        {
            monster.AttackPlayer(_player);
            if (_player.IsDead)
            {
                _gameOver = true;
                // Save the high score if it's a new high score
                if (_player.Score > _highScore)
                {
                    _highScore = _player.Score;
                    FileHelper.SaveHighScore(_highScore);
                }
                return;
            }
        }


        if (_isMoving)
        {
            AnimationManager.UpdateAnimation(ref _currentFrame, ref _lastFrameTime, _frameDuration, _playerRunRightFrames.Count);
        }

        CheckItemPickup();
        UpdateFloatingTexts();
    }

    private void CheckItemPickup()
    {
        foreach (var item in Goblin.DroppedItems.ToList())
        {
            if (SplashKit.RectanglesIntersect(_player.Hitbox, item.Hitbox))
            {
                _player.PickUpItem(item);
                Goblin.DroppedItems.Remove(item);
                _floatingTexts.Add(new FloatingText($"Picked up: {item.Name}", Color.Green, new Vector2D { X = item.Position.X, Y = item.Position.Y }));
            }
        }

        foreach (var item in Wolf.DroppedItems.ToList())
        {
            if (SplashKit.RectanglesIntersect(_player.Hitbox, item.Hitbox))
            {
                _player.PickUpItem(item);
                Wolf.DroppedItems.Remove(item);
                _floatingTexts.Add(new FloatingText($"Picked up: {item.Name}", Color.White, new Vector2D { X = item.Position.X, Y = item.Position.Y }));
            }
        }

        foreach (var item in Spider.DroppedItems.ToList())
        {
            if (SplashKit.RectanglesIntersect(_player.Hitbox, item.Hitbox))
            {
                _player.PickUpItem(item);
                Spider.DroppedItems.Remove(item);
                _floatingTexts.Add(new FloatingText($"Picked up: {item.Name}", Color.White, new Vector2D { X = item.Position.X, Y = item.Position.Y }));
            }
        }
    }

    private void UpdateFloatingTexts()
    {
        foreach (var floatingText in _floatingTexts.ToList())
        {
            floatingText.Update();
            if (floatingText.IsExpired)
            {
                _floatingTexts.Remove(floatingText);
            }
        }
    }

    public void Draw()
    {
        SplashKit.ClearScreen(Color.White);

        if (_gameOver)
        {
            SplashKit.DrawText("Game Over", Color.Red, 700, 400);
            SplashKit.DrawText($"High Score: {_highScore}", Color.Black, 700, 450);
            SplashKit.RefreshScreen();
            return;
        }

        GameDrawer.DrawGame(_player, _monsters, _playerRunRightFrames, _playerRunLeftFrames, _playerRestBitmap, _currentFrame, _isMoving, _showHitboxes, _floorBitmap, _highScore);

        _player.DrawReloadMessage();

        foreach (var floatingText in _floatingTexts)
        {
            floatingText.Draw();
        }

       
        SplashKit.DrawBitmap(_cursorBitmap, SplashKit.MouseX(), SplashKit.MouseY());
        SplashKit.RefreshScreen();
    }
}