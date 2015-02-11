using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace net
{
    public class GetScores : Request
    {
        public enum TimeSpan
        {
            ALL,
            ALL_TIME,
            DAILY,
            WEEKLY
        }

        public GetScores(string playerId, string leaderboardId, TimeSpan timeSpan)
        {
            Url = "https://www.googleapis.com/games/v1/players/";
            Url += playerId;
            Url += "/leaderboards/";
            Url += leaderboardId;
            Url += "/scores/";
            Url += timeSpan.ToString();
        }
    }

}
