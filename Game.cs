using Cave_dweller;
using SplashKitSDK;

public class Game
{
    private Player _player;
    private List<Goblin> _goblins;
    private List<Wolf> _wolfs;
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

    public Game()
    {
        _player = new Player(3, 10, new Vector2D() { X = 100, Y = 100 }, 0.6);
        _goblins = new List<Goblin>
        {
            new Goblin(new Vector2D() { X = 500, Y = 200 }),
            new Goblin(new Vector2D() { X = 500, Y = 300 })
        };

        _wolfs = new List<Wolf>
        {
            new Wolf(new Vector2D() { X = 600, Y = 500 }),
            new Wolf(new Vector2D() { X = 600, Y = 400 })
        };

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

        _floatingTexts = new List<FloatingText>();
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

        GameStateUpdater.UpdateGameState(_player, _goblins);
        ProjectileManager.UpdateProjectiles(_player, _goblins);
       

        int playerSpriteWidth = _playerRestBitmap.Width;
        int playerSpriteHeight = _playerRestBitmap.Height;
        InputHandler.HandleInput(_player, ref _isMoving, playerSpriteWidth, playerSpriteHeight);

        _player.HandleInput();
        _player.UpdateReloadAnimation();

        foreach (Goblin goblin in _goblins)
        {
            goblin.AttackPlayer(_player);
            if (_player.IsDead)
            {
                _gameOver = true;
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
            SplashKit.RefreshScreen();
            return;
        }

        GameDrawer.DrawGame(_player, _goblins, _wolfs, _playerRunRightFrames, _playerRunLeftFrames, _playerRestBitmap, _currentFrame, _isMoving, _showHitboxes, _floorBitmap);
        _player.DrawReloadMessage();

        foreach (var floatingText in _floatingTexts)
        {
            floatingText.Draw();
        }

        SplashKit.DrawBitmap(_cursorBitmap, SplashKit.MouseX(), SplashKit.MouseY());

        SplashKit.RefreshScreen();
    }
}