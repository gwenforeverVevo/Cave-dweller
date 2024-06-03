using SplashKitSDK;

namespace Cave_dweller
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Cave_dweller", 800, 600);
            Game game = new Game();

            do
            {
                SplashKit.ProcessEvents();
                game.Update();
                game.Draw();
                SplashKit.RefreshScreen();
            } while (!window.CloseRequested);
        }
    }
}