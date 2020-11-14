using Newtonsoft.Json;
using NLog;
using SiteBuilder.SiteData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SiteBuilder
{
    public static class RoadMapReader
    {
        public static List<RoadMapItem> ReadAndPrepare(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
            {
                return new List<RoadMapItem>();
            }

            var jsonStr = File.ReadAllText(fileName);

            if(string.IsNullOrWhiteSpace(jsonStr))
            {
                return new List<RoadMapItem>();
            }

            var items = JsonConvert.DeserializeObject<List<RoadMapItem>>(jsonStr);

            FillUpDefaultValues(items);
            CalculateDates(items);

            return items;
        }

        private static void CalculateDates(List<RoadMapItem> items)
        {
            DateTime? prevEnd = null;

            foreach (var item in items)
            {
                if(item.Kind == KindOfRoadMapItem.Unknown)
                {
                    item.Kind = KindOfRoadMapItem.Step;
                }

                if (item.End.HasValue && item.KindOfCompeltion != KindOfRoadMapItemCompeltion.Completed)
                {
                    throw new Exception($"It is weird! '{item.Name}' is finished at {item.End} but is not completed");
                }

                if (item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Completed && !item.End.HasValue)
                {
                    throw new Exception($"It is weird! '{item.Name}' is completed finished but does not have any End date.");
                }

                if (item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Completed)
                {
                    prevEnd = item.End;
                    continue;
                }

                if (item.Start.HasValue)
                {
                    if (item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Unknown)
                    {
                        if(item.End.HasValue)
                        {
                            item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Completed;
                        }
                        else
                        {
                            item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Developed;
                        }
                    }
                }
                else
                {
                    item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Planned;
                    item.Start = prevEnd.Value.AddDays(1);
                }

                if (item.End.HasValue)
                {
                    if(item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Unknown)
                    {
                        item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Completed;
                    }
                }
                else
                {
                    item.End = CalculateEnd(item.Start.Value, item.ExpectedDuration.Value, item.KindOfExpectedDuration);
                }

                prevEnd = item.End;
            }
        }

        private static DateTime CalculateEnd(DateTime start, int duration, KindOfDuration kindOfDuration)
        {
            switch (kindOfDuration)
            {
                case KindOfDuration.Months:
                    return CalculateEndByMonthsDuration(start, duration);

                default:
                    throw new ArgumentOutOfRangeException(nameof(kindOfDuration), kindOfDuration, null);
            }

            throw new NotImplementedException();
        }

        private static DateTime CalculateEndByMonthsDuration(DateTime start, int duration)
        {
            if (start.Day == 1)
            {
                return start.AddMonths(duration).AddDays(-1);
            }

            var targetStartMonth = start.Month + 1;
            var targetStartYear = start.Year;

            if (targetStartMonth == 13)
            {
                targetStartMonth = 1;
                targetStartYear++;
            }

            return (new DateTime(targetStartYear, targetStartMonth, 1)).AddMonths(duration).AddDays(-1);
        }

        private static void FillUpDefaultValues(List<RoadMapItem> items)
        {
            foreach (var item in items)
            {
                if (item.End.HasValue && !item.Start.HasValue)
                {
                    throw new Exception($"In which way can you finish step '{item.Name}' at {item.End} without start.");
                }

                if (item.KindOfCompeltion == KindOfRoadMapItemCompeltion.Unknown)
                {
                    if (item.End.HasValue)
                    {
                        item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Completed;
                    }
                    else
                    {
                        if (item.Start.HasValue)
                        {
                            item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Developed;
                        }
                        else
                        {
                            item.KindOfCompeltion = KindOfRoadMapItemCompeltion.Planned;
                        }

                        if (!item.ExpectedDuration.HasValue)
                        {
                            item.ExpectedDuration = 2;
                        }

                        if (item.KindOfExpectedDuration == KindOfDuration.Unknown)
                        {
                            item.KindOfExpectedDuration = KindOfDuration.Months;
                        }
                    }
                }
            }
        }
    }
}
