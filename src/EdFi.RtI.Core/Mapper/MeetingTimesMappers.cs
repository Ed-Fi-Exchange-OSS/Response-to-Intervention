using EdFi.Ods.Api.Client.Models;
using EdFi.RtI.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace EdFi.RtI.Core.Mapper
{
    public static class MeetingTimesMappers
    {
        public static InterventionMeetingTimesItem MapToInterventionMeetingTimesItem(this MeetingTimev3 a)
        {
            return new InterventionMeetingTimesItem
            {
                EndTime = a.EndTime.ToShortTimeString(),
                StartTime = a.StartTime.ToShortTimeString(),
            };
        }

        public static MeetingTimev3 MapToMeetingTimev3(this InterventionMeetingTimesItem a)
        {
            DateTime mapTime(string dateTimeStr) => string.IsNullOrWhiteSpace(dateTimeStr)
                ? new DateTime()
                : DateTime.Parse(dateTimeStr);

            return new MeetingTimev3
            {
                EndTime = mapTime(a.EndTime),
                StartTime = mapTime(a.StartTime),
            };
        }
    }
}
