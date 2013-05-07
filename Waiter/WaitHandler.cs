using System;
using System.Threading;

namespace Waiter
{
    public static class WaitHandler
    {
        private static void ExecuteSplashScreen()
        {
            var splScr = new WaitWindow();
            splScr.ShowDialog();
        }

        public static void Run(Action act)
        {
            var threadSplashScreen = new Thread(ExecuteSplashScreen);
            threadSplashScreen.SetApartmentState(ApartmentState.STA);
            try
            {
                threadSplashScreen.Start();
                act();
                threadSplashScreen.Abort();
            }
            catch (Exception ex)
            {
                threadSplashScreen.Abort();
                throw new Exception("Ошибка при выполнении операции.", ex);
            }
        }
    }
}
