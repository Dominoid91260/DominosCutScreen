using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DominosCutScreen.Shared
{
    public class QuietTime
    {
        /// <summary>
        /// Whether or not QuietTime is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// When does QuietTime start?
        /// </summary>
        public TimeOnly Start { get; set; }

        /// <summary>
        /// When does QuietTime end?
        /// </summary>
        public TimeOnly End { get; set; }
    }

    public class TimedOrderAlarm
    {
        /// <summary>
        /// Whether or not the Timed Order Alarm is enabled
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// How many seconds to make each pizza. Used to calculate how long a timed order should take to make
        /// and sound an alarm the order comes through within the calculated time * 1.5
        /// </summary>
        public int SecondsPerPizza { get; set; }

        /// <summary>
        /// How many pizzas should be in the timed order before triggering the alarm.
        /// Set to 0 to trigger for all timed orders
        /// </summary>
        public int MinPizzaThreshold { get; set; }
    }

    public class SettingsService
    {
        [JsonIgnore]
        [Key]
        public int SettingsServiceId { get; set; }

        #region Server
        /// <summary>
        /// 
        /// </summary>
        public string MakelineServer { get; set; }

        public int MakelineCode { get; set; }
        #endregion // Server

        #region Client
        /// <summary>
        /// How long is the oven time in seconds
        /// </summary>
        public int OvenTime { get; set; }

        /// <summary>
        /// How long should orders stay on the screen after they have come out of the oven
        /// </summary>
        public int GraceTime { get; set; }

        /// <summary>
        /// How long in seconds between bumped items should the oven alert be triggered
        /// </summary>
        public int AlertInterval { get; set; }

        /// <summary>
        /// How often in seconds to fetch new order and bump data from the server.
        /// Also sets how often data is fetched from <paramref name="MakelineServer"/>
        /// </summary>
        public int FetchInterval { get; set; }

        /// <summary>
        /// When enabled, oven and timed order alerts will not sound between Start and End times.
        /// </summary>
        public QuietTime QuietTime { get; set; }

        /// <summary>
        /// When enabled, timed orders who's calculated time to make exceeds the current lead time will play an alarm.
        /// </summary>
        public TimedOrderAlarm TimedOrderAlarm { get; set; }
        #endregion // Client

        public virtual ICollection<PostBake> PostBakes { get; set; }
    }
}
