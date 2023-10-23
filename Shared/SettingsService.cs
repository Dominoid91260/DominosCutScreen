using System.Net;

namespace DominosCutScreen.Shared
{
    public class QuietTime
    {
        /// <summary>
        /// Whether or not QuietTime is enabled
        /// </summary>
        public bool IsEnabled { get; set; } = false;

        /// <summary>
        /// When does QuietTime start?
        /// </summary>
        public TimeOnly Start { get; set; }

        /// <summary>
        /// When does QuietTime end?
        /// </summary>
        public TimeOnly End { get; set; }
    }

    public class SettingsService
    {
        #region Server
        /// <summary>
        /// 
        /// </summary>
        public string MakelineServer { get; set; } = "http://10.104.37.32:59108";

        public int MakelineCode { get; set; } = 2;
        #endregion // Server

        #region Client
        /// <summary>
        /// How long is the oven time in seconds
        /// </summary>
        public int OvenTime { get; set; } = 300;

        /// <summary>
        /// How long should orders stay on the screen after they have come out of the oven
        /// </summary>
        public int GraceTime { get; set; } = 90;

        /// <summary>
        /// How long in seconds between bumped items should the oven alert be triggered
        /// </summary>
        public int AlertInterval { get; set; } = 150;

        /// <summary>
        /// How often in seconds to fetch new order and bump data from the server.
        /// Also sets how often data is fetched from <paramref name="MakelineServer"/>
        /// </summary>
        public int FetchInterval { get; set; } = 5;

        /// <summary>
        /// How many seconds to make each pizza. Used to calculate how long a timed order should take to make
        /// and sound an alarm the order comes through within the calculated time * 1.5
        /// </summary>
        public int TimedOrderSecondsPerPizza { get; set; } = 15;

        /// <summary>
        /// When enabled, oven and timed order alerts will not sound between Start and End times.
        /// </summary>
        public QuietTime QuietTime { get; set; } = new();
        #endregion // Client
    }
}
