using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;
using GooglePlayGames.BasicApi;


namespace Srv
{

	public class SocialService : Service
	{
		public void Activate () 
		{
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
		}

		public virtual void LoadLeaderboardIds()
		{}
		
		public string UserId { get { return Social.localUser.id; } }

		// Login 
		public void Login(Action<bool> callback)
		{
			if (Social.localUser.authenticated)
			{
				Core.Dbg.Log("SocialService.Login() - already authentificated");
				
				if (callback != null)
					callback(true);
			}
			else
			{
				Social.localUser.Authenticate((bool success) =>
				{
					if (success)
						Core.Dbg.Log("SocialService.Login() - succeeded");
					else
						Core.Dbg.Log("SocialService.Login() - failed");
					
					if (callback != null)
						callback(success);
				});
			}
		}
		
		
		public bool IsLogged()
		{
			return ((PlayGamesPlatform)Social.Active).IsAuthenticated();
		}
		
		public void ReportScore(int score, string leaderboardId, Action<bool> callback)
		{
			Core.Dbg.Log("SocialService.ReportScore called on" + leaderboardId + " with " + score);

			Social.ReportScore(score, leaderboardId, (bool success) =>
			                   {
				if (success)
					Core.Dbg.Log("SocialService.ResportScore() + " + score.ToString() + " to " + leaderboardId +  " - succeeded");
				else
					Core.Dbg.Log("SocialService.ResportScore() + " + score.ToString() + " to " + leaderboardId +  " - failed");
				
				
				if (callback != null)
					callback(success);
			});
		}
		
		public void LoadScores(string leaderboardId, Action<IScore[]> scores)
		{
			Social.Active.LoadScores(leaderboardId, scores);
		}


		
		// Unlock achievement 
		public void UnlockAchievement(string achievementId, Action<bool> callback)
		{
			Social.ReportProgress(achievementId, 100.0, (bool success) =>
			                      {
				if (success)
					Core.Dbg.Log("SocialService.UnlockAchievement() + " + achievementId +  " - succeeded");
				else
					Core.Dbg.Log("SocialService.UnlockAchievement() + " + achievementId +  " - failed");
				
				
				if (callback != null)
					callback(success);
			});
		}
		
		
		public void UnlockIncrementalAchievement(string achievementId, int steps, Action<bool> callback)
		{
			((PlayGamesPlatform)Social.Active).IncrementAchievement(achievementId, steps, (bool success) =>
			                                                        {
				if (success)
					Core.Dbg.Log("SocialService.UnlockIncrementalAchievement() + " + achievementId +  " - succeeded");
				else
					Core.Dbg.Log("SocialService.UnlockIncrementalAchievement() + " + achievementId +  " - failed");
				
				if (callback != null)
					callback(success);
			});
		}
		
		public void ShowLeaderBoard()
		{
			Social.ShowLeaderboardUI();
		}
		
		public void ShowSpecificLeaderBoard(string leaderBoardId)
		{
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderBoardId);
		}
		
		
		public void ShowAchievements()
		{
			Social.ShowAchievementsUI();
		}
		
		
		public void SignOut()
		{
			((PlayGamesPlatform)Social.Active).SignOut();
		}
	}
}