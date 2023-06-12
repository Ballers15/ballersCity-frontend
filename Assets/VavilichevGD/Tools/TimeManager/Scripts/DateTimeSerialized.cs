using System;
using UnityEngine;

namespace VavilichevGD.Tools
{
    [Serializable]
    public class DateTimeSerialized
    {
        [SerializeField] 
        public string dateTimeStr;

        public DateTimeSerialized(DateTime dateTime)
        {
            SetDateTime(dateTime);
        }

        public DateTimeSerialized()
        {
        }

        public DateTime GetDateTime()
        {
            //if (!string.IsNullOrEmpty(dateTimeStr)) {
                //Debug.Log(dateTimeStr);
                return DateTime.Parse(dateTimeStr);
            //}
            //return new DateTime();
        }

        public void SetDateTime(DateTime dateTime)
        {
            dateTimeStr = dateTime.ToString();
        }

        public override string ToString()
        {
            return dateTimeStr;
        }

        public DateTimeSerialized Clone()
        {
            return new DateTimeSerialized
            {
                dateTimeStr = this.dateTimeStr
            };
        }
    }
}