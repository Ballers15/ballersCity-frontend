using System;
using System.Collections;
using UnityEngine;

namespace Orego.Util
{
    public static class RoutineExtensions
    {
        #region Create

        public static Routine NewRoutine(this MonoBehaviour monoBehaviour)
        {
            return new Routine(monoBehaviour);
        }

        #endregion

        #region Start

        public static void Start(this Routine routine, Func<IEnumerator> func)
        {
            var enumerator = func.Invoke();
            routine.Start(enumerator);
        }

        public static void ForceStart(this Routine routine, Func<IEnumerator> func)
        {
            var enumerator = func?.Invoke();
            routine.ForceStart(enumerator);
        }

        public static void ForceStart(this Routine routine, IEnumerator enumerator)
        {
            if (routine.IsWorking())
            {
                routine.Cancel();
            }

            routine.Start(enumerator);
        }

        #endregion

        #region Join

        public static IEnumerator Join(this Routine routine, Func<IEnumerator> func)
        {
            var enumerator = func.Invoke();
            yield return routine.Join(enumerator);
        }

        public static IEnumerator ForceJoin(this Routine routine, Func<IEnumerator> func)
        {
            var enumerator = func?.Invoke();
            yield return routine.ForceJoin(enumerator);
        }

        public static IEnumerator ForceJoin(this Routine routine, IEnumerator enumerator)
        {
            if (routine.IsWorking())
            {
                routine.Cancel();
            }

            yield return routine.Join(enumerator);
        }

        #endregion
    }
}