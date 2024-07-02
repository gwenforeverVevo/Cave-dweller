using Cave_dweller;
using SplashKitSDK;
using System.Runtime.InteropServices;

public class Program
{
    // Import the ShowCursor function from the user32.dll
    [DllImport("user32.dll")]
    private static extern int ShowCursor(bool bShow);

    public static void Main()
    {
        // Hide the system cursor
        ShowCursor(false);

        Window window = new Window("Cave_dweller", 1600, 900);
        Game game = new Game();
        do
        {
            SplashKit.ProcessEvents();
            game.Update();
            game.Draw();
            SplashKit.RefreshScreen();
        } while (!window.CloseRequested);

        // Show the system cursor when the game ends
        ShowCursor(true);
    }
}
