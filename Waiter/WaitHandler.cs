using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Waiter
{
    public static class WaitHandler
    {
        private static void ExecuteSplashScreen()
        {
            try
            {
                Waiter.WaitWindow splScr = new Waiter.WaitWindow();
                splScr.ShowDialog();
            }
            catch
            {
            }
        }

        public static void Run(Action act)
        {
            Thread threadSplashScreen = new Thread(ExecuteSplashScreen);
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
