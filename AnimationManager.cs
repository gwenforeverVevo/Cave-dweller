
using System;
using SplashKitSDK;

namespace Cave_dweller
{
    public static class AnimationManager
    {
        public static void UpdateAnimation(ref int currentFrame, ref double lastFrameTime, double frameDuration, int frameCount)
        {
            double currentTime = SplashKit.CurrentTicks();
            if (currentTime - lastFrameTime > frameDuration * 1000)
            {
                currentFrame = (currentFrame + 1) % frameCount;
                lastFrameTime = currentTime;
            }
        }
    }
}
