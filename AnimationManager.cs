
using System;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class AnimationManager
    {
        // Updates the current frame of the animation based on the frame duration and time elapsed
        public static void UpdateAnimation(ref int currentFrame, ref double lastFrameTime, double frameDuration, int frameCount)
        {
            double currentTime = SplashKit.CurrentTicks();
            // Check if enough time has passed to advance to the next frame
            if (currentTime - lastFrameTime > frameDuration * 1000)
            {
                // Update the current frame
                currentFrame = (currentFrame + 1) % frameCount;
                // Update the last frame time to the current time
                lastFrameTime = currentTime;
            }
        }
    }
}
