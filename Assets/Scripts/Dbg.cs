using UnityEngine;

namespace Core
{
	public static class Dbg 
	{
		public enum MessageType
		{
			Info,
			Warning,
			Error
		}
		
		[System.Diagnostics.Conditional("DEBUG")]
		public static void Assert(bool condition, string message, bool warningOnly = false)
		{
			if (!condition)
			{
				if (warningOnly)
				{
					Log(message, MessageType.Error);
				}
				else
				{
					throw new System.Exception("Core.Dbg.Assert - " + message);
				}
			}
		}
		
		public static void Log(string message, MessageType type = MessageType.Info)
		{
			Debug.Log (MarkUpMessage(message, type));
		}
		
		private static string MarkUpMessage(string message, MessageType type)
		{
			string markUp = null;
			
			switch (type) 
			{
			case MessageType.Info:
				markUp = "<color=green>Info:</color>";
				break;
				
			case MessageType.Warning:
				markUp = "<color=yellow>Warning:</color>";
				break;
				
			case MessageType.Error:
				markUp = "<color=red>Error:</color>";
				break;
				
			}
			
			return markUp + message;
		}
		
	}
}
