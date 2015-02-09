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
                Implementations.Add(impl);
                impl.Init((bool succ) =>
                {
                    callback(Mode.Local, succ);
                });
            }

            if ((modes & Mode.Cloud) != 0)
            {
                var impl = new CloudSaveGameImplementation();
                Implementations.Add(impl);
                impl.Init((bool succ) =>
                {
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

            App.Instance.PlayerStatus.Load(saveGameStr);

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
        public override void Init(Action<bool> result)
        {
            OpenSavedGame("saveGame", (SavedGameRequestStatus status, ISavedGameMetadata metaData) =>
            {
                Core.Dbg.Log("CloudSaveGameImplementation.Load() - status " + status.ToString());
                result(status == SavedGameRequestStatus.Success);
            });
        }

        public override void Load()
        {
          
        }

        public override void Save()
        {
         
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

        private void SaveGame(ISavedGameMetadata game, byte[] savedData)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
            SavedGameMetadataUpdate updatedMetadata = builder.Build();

            savedGameClient.CommitUpdate(game, updatedMetadata, savedData, (SavedGameRequestStatus status, ISavedGameMetadata metadata) =>
            {
                if (status == SavedGameRequestStatus.Success)
                {
                    // handle reading or writing of saved game.
                }
                else
                {
                    // handle error
                }
            });
        }
    }
}
