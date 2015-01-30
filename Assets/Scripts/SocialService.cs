﻿using UnityEngine;
using System.Collections;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System;

public abstract class SocialService : MonoBehaviour
{
	public void Activate () 
	{
		PlayGamesPlatform.Activate();
	}

	public abstract void LoadLeaderboardIds();
	
	public string UserId { get { return Social.localUser.id; } }

	// Login 
	public void Login(Action<bool> callback)
	{
		Social.localUser.Authenticate((bool success) =>
		{
			if (success)
				Debug.Log("SocialService.Login() - succeeded");
			else
				Debug.Log("SocialService.Login() - failed");
			
			if (callback != null)
				callback(success);
		});
	}
	
	
	public bool IsLogged()
	{
		return ((PlayGamesPlatform)Social.Active).IsAuthenticated();
	}
	
	public void ReportScore(int score, string leaderboardId, Action<bool> callback)
	{
		Debug.Log("SocialService.ReportScore called on" + leaderboardId + " with " + score);

		Social.ReportScore(score, leaderboardId, (bool success) =>
		                   {
			if (success)
				Debug.Log("SocialService.ResportScore() + " + score.ToString() + " to " + leaderboardId +  " - succeeded");
			else
				Debug.Log("SocialService.ResportScore() + " + score.ToString() + " to " + leaderboardId +  " - failed");
			
			
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
				Debug.Log("SocialService.UnlockAchievement() + " + achievementId +  " - succeeded");
			else
				Debug.Log("SocialService.UnlockAchievement() + " + achievementId +  " - failed");
			
			
			if (callback != null)
				callback(success);
		});
	}
	
	
	public void UnlockIncrementalAchievement(string achievementId, int steps, Action<bool> callback)
	{
		((PlayGamesPlatform)Social.Active).IncrementAchievement(achievementId, steps, (bool success) =>
		                                                        {
			if (success)
				Debug.Log("SocialService.UnlockIncrementalAchievement() + " + achievementId +  " - succeeded");
			else
				Debug.Log("SocialService.UnlockIncrementalAchievement() + " + achievementId +  " - failed");
			
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