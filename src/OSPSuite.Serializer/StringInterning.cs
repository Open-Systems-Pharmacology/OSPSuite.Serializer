using System.Collections.Generic;
using System.Threading;

namespace OSPSuite.Serializer
{
   /// <summary>
   ///    Performs string interning (i.e. same string will only be saved once in memory)
   /// </summary>
   internal static class StringInterning
   {
      private static readonly IDictionary<string, string> _strings = new Dictionary<string, string>();

      private static readonly ReaderWriterLockSlim _locker = new ReaderWriterLockSlim();

      public static void Clear()
      {
         _locker.EnterWriteLock();
         try
         {
            _strings.Clear();
         }
         finally
         {
            _locker.ExitWriteLock();
         }
      }

      public static string Intern(string str)
      {
         if (string.IsNullOrEmpty(str))
            return str;

         string val;
         _locker.EnterReadLock();
         try
         {
            if (_strings.TryGetValue(str, out val))
               return val;
         }
         finally
         {
            _locker.ExitReadLock();
         }

         _locker.EnterWriteLock();
         try
         {
            if (_strings.TryGetValue(str, out val))
               return val;

            _strings.Add(str, str);
            return str;
         }
         finally
         {
            _locker.ExitWriteLock();
         }
      }
   }
}