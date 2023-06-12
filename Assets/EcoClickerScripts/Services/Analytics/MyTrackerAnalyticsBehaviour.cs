using System.Collections.Generic;
//using Mycom.Tracker.Unity;

namespace SinSity.Services
{
    //TODO: Андрей, предлагаю сделать общий статический фасад для событий,
    //который будет трансформировать общее событие по всем аналитикам. Как в ледоколах. 
    //Так не будет лишних проходов foreach по трансформированию.

    //Тут преобразование из Dictionary<string, object> -> Dictionary<string, string> - не стоит свеч.
    public sealed class MyTrackerAnalyticsBehaviour : AnalyticsBehavior
    {
        public override void LogEvent(string eventKey, Dictionary<string, object> eventParameters)
        {
            var parameters = new Dictionary<string, string>();
            foreach (var eventParameter in eventParameters)
            {
                parameters.Add(eventParameter.Key, eventParameter.Value.ToString());
            }

            //MyTracker.TrackEvent(eventKey, parameters);
        }

        public override void LogEvent(string eventKey)
        {
            //MyTracker.TrackEvent(eventKey);
        }
    }
}