using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QuickFix.Util
{
    public static class IORetry
    {
        public static readonly int NUM_RETRIES = 3;
        public static readonly int TIME_WAIT = 200;

        public static T Try<T>(Func<T> func, bool throwOnFail = true)
        {
            int numRetries = NUM_RETRIES;
            string lastMessage = null;

            do
            {
                try
                {
                    return func();
                }
                catch (IOException ex)
                {
                    lastMessage = ex.Message;
                }

                System.Threading.Thread.Sleep(TIME_WAIT);

            } while (numRetries-- > 0);

            if (throwOnFail)
            {
                throw new IOException(lastMessage);
            }

            return default(T);
        }

        public static bool Try(Action action, bool throwOnFail = true)
        {
            return Try(() =>
            {
                action();
                return true;
            }, throwOnFail);
        }
    }
}
