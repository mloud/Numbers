using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Collections.Generic;

namespace Srv
{
    public class SaveGameService : Service
    {
        public enum Mode
        {
            Local = 1 << 0,
            Cloud = 1 << 1
        }

        List<SaveGameImplementation> Implementations { get; set; } 

        private Mode modes;

        public SaveGameService()
        {
            Implementations = new List<SaveGameImplementation>();
        }

        public void Init(Mode modes, Action<Mode, bool> callback)
        {
            this.modes = modes;

            if ((modes & Mode.Local) != 0)
            {
                var impl = new LocalSaveGameImplementation();
                impl.Init((bool succ) =>
                {
                 	if (succ)
				   	{
						Implementations.Add(impl);
						Core.Dbg.Log("SaveGameService.Init() adding " + impl.GetType().ToString());
					}
				   
					callback(Mode.Local, succ);
                });
            }

            if ((modes & Mode.Cloud) != 0)
            {
                var impl = new CloudSaveGameImplementation();
                impl.Init((bool succ) =>
                {
					if (succ)
					{
						Implementations.Add(impl);
						Core.Dbg.Log("SaveGameService.Init() adding " + impl.GetType().ToString());
					}
                    
					callback(Mode.Cloud, succ);
                });
            }
        }

        public void Delete()
        {
            Implementations.ForEach(x => x.Delete());
        }

        public void Save()
        {
            Core.Dbg.Log("SaveGameService.Save() started", Core.Dbg.MessageType.Info);

            Implementations.ForEach(x => x.Save());

            Core.Dbg.Log("SaveGameService.Save() finished", Core.Dbg.MessageType.Info);
        }

        public void Load()
        {
            Core.Dbg.Log("SaveGameService.Load() started", Core.Dbg.MessageType.Info);
            
            // todo 
            Implementations.ForEach(x => x.Load());

            Core.Dbg.Log("SaveGameService.Load() finished", Core.Dbg.MessageType.Info);
        }
    }


    abstract class SaveGameImplementation
    {
        public abstract void Init(Action<bool> result);
        public abstract void Save();
        public abstract void Load();
        public abstract void Delete();
    }

    class LocalSaveGameImplementation : SaveGameImplementation
    {
        public override void Init(Action<bool> result)
        {
            result(true); //always ok
        }

        public override void Load()
        {
            Core.Dbg.Log("LocalSaveGameImplementation.Load() started", Core.Dbg.MessageType.Info);
            
            string saveGameStr = UnityEngine.PlayerPrefs.GetString("saveGame");

            App.Instance.PlayerStatus.Load(saveGameStr, true);

            Core.Dbg.Log("LocalSaveGameImplementation.Load() finished - " + saveGameStr, Core.Dbg.MessageType.Info);
        }

        public override void Save()
        {
            Core.Dbg.Log("LocalSaveGameImplementation.Save() started", Core.Dbg.MessageType.Info);

            string saveGameStr = App.Instance.PlayerStatus.GetSaveGame();
           
            UnityEngine.PlayerPrefs.SetString("saveGame", saveGameStr);
            UnityEngine.PlayerPrefs.Save();

            Core.Dbg.Log("LocalSaveGameImplementation.Save() finished - " + saveGameStr, Core.Dbg.MessageType.Info);
        }

        public override void Delete()
        {
            App.Instance.PlayerStatus.Reset();

            Save();
        }
    }



    class CloudSaveGameImplementation : SaveGameImplementation
    {
		private ISavedGameMetadata MetaData {get; set;}

        public override void Init(Action<bool> result)
        {
            OpenSavedGame("saveGame", (SavedGameRequestStatus status, ISavedGameMetadata metaData) =>
            {
                Core.Dbg.Log("CloudSaveGameImplementation.Init() - status " + status.ToString());

				MetaData = metaData;

                result(status == SavedGameRequestStatus.Success);
            });
        }

        public override void Load()
        {
			Core.Dbg.Log("CloudSaveGameImplementation.Load() started");

			LoadGame(MetaData, (SavedGameRequestStatus status, byte[] data) =>
			{
			
				Core.Dbg.Log ("CloudSaveGameImplementation.Load() result " + status.ToString());

				if (status == SavedGameRequestStatus.Success)
				{
					string strData = System.Text.Encoding.UTF8.GetString(data);
					Core.Dbg.Log("CloudSaveGameImplementation.Load() data is : " + strData);
					App.Instance.PlayerStatus.Load(strData, true);				
				}
			});
			
			Core.Dbg.Log("CloudSaveGameImplementation.Load() finished");
        }

        public override void Save()
        {
			Core.Dbg.Log("CloudSaveGameImplementation.Save() started");

			bool succWritten = false;

			Init((bool succInit) =>
			{
				if (succInit)
				{
					var saveGame = App.Instance.PlayerStatus.GetSaveGame();
					
					SaveGame(MetaData, System.Text.Encoding.UTF8.GetBytes (saveGame), (bool succ) =>
					{
						Core.Dbg.Log ("CloudSaveGameImplementation.Save() result " + succ.ToString());

						succWritten = succ;
					});
				}
			});

			Core.Dbg.Log("CloudSaveGameImplementation.Save() finished with result " + (succWritten ? "Succeeded" : "Failed"), succWritten ? Core.Dbg.MessageType.Info : Core.Dbg.MessageType.Error);
        }


        public override void Delete()
        {
        	
        }

        private void OpenSavedGame(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> onOpenCallback)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, onOpenCallback);
        }

		private void LoadGame (ISavedGameMetadata game, Action<SavedGameRequestStatus, byte[]> result) 
		{
			ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
			savedGameClient.ReadBinaryData(game, result);
		}

        private void SaveGame(ISavedGameMetadata game, byte[] savedData, Action<bool> result)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, (SavedGameRequestStatus status, ISavedGameMetadata metadata) =>
            {
				Core.Dbg.Log("CloudSaveGameImplementation.SaveGame() " + status.ToString(), Core.Dbg.MessageType.Info);

				result(status == SavedGameRequestStatus.Success);
            });
        }

    }
}
