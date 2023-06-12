using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// mention all fixed values like screen/scene names, default values of player, gameplay round
/// </summary>
public static class Constants
{
    #region Clips        
    public const string GAME_BG = "GameBG";
    #endregion

   
    #region Rounds
    public static int MaxOpponentLimit = 4;
    public const int MIN_PLAYER_REQ = 3;
    public const byte MAX_PLAYER_REQ = 50;
    public const int MIN_ROUND_TIME = 1;
    #endregion

    #region Scenes
    public class Scene
    {
        public const string SERVER_SCENE = "Server";
        public const string INIT_SCENE = "Init";
        public const string LOGIN_SCENE = "Login";
        public const string HOME_SCENE = "LoginScene";
        public const string MAIN_MENU_SCENE = "MainMenu";
        public const string GAME_PLAY_SCENE = "Sin City Centre";
        public const string THE_HILLS_SCENE = "TheHills";


        //public const string MAP_TYPE_SIN_CITY_CENTRE = "Sin City Centre";
        public const string MAP_TYPE_SIN_CITY_CENTRE = "China Town";
        public const string MAP_TYPE_SILICON_VALLEY = "Silicon Valley";
        public const string MAP_TYPE_FAVELAS = "Favelas";
        public const string MAP_TYPE_CHINA_TOWN = "China Town";
        public const string MAP_TYPE_THE_HILLS = "The Hills";

        //Minigame
        public const string SHOOTING_RANGE = "ShootingRange";
        public const string SQUID_GAME_LAUNCHER = "SinverseSquidGameLauncher";
        public const string SQUID_GAME_WAITING = "SinverseSquidGameWaiting";
        public const string SQUID_GAME = "SinverseSquidGame";

    }

    public const string LOBBY_FILTER_KEY = "SceneName";

    #endregion

    #region CharactersName
    public class Characters
    {
        public const string MALE = "Male";
        public const string FEMALE1 = "Female1";
        public const string FEMALE2 = "Female2";
        public const string FEMALE3 = "Female3";
        public const string FEMALE4 = "Female4";
    }

    public class Message
    {
        public const string NO_INTERNET_MESSAGE = "No Internet Available.";
    }

    #endregion

    #region Bundle
    public class Bundle
    {
        public const string SIN_CITY_CENTRE_BUNDLE = "Sin City Centre";
        public const string SILICON_VALLEY_BUNDLE = "Silicon Valley";
        public const string THE_HILLS_BUNDLE = "The Hills";
        public const string FAVELS_BUNDLE = "Favels";
        public const string CHINA_TOWN_BUNDLE = "China Town";
    }

    #endregion
    
    #region RPC
    public class RPC
    {
        public const string BuildMode = "BuildMode";


        public const string PURCHASE_PLOT = "OnPurchasePlot";        
        public const string PLOT_BUILD_RPC = "PlotBuildRPC";        
        public const string REQUEST_CONSTRUCTION_DATA_RPC = "RequestConstructionDataRPC";        
        public const string RESPONSE_CONSTRUCTION_DATA_RPC = "ResponsePlotDataRPC";        
        public const string FLOOR_PLACEABLE_RPC = "FloorPlaceablePlaceRPC";        
        public const string EDGE_PLACEABLE_RPC = "EdgePlaceablePlaceRPC";        
        public const string FLOOR_PLACEABLE_DEMOLISH_RPC = "FloorPlaceableDemolishRPC";        
        public const string FLOOR_EDGE_PLACEABLE_DEMOLISH_RPC = "FloorEdgePlaceableDemolishRPC";        
    }

    #endregion

    public const string PP_VERSION = "Version";
    public class PunAppId
    {
        //public const string SERVER_APPID_REALTIME = "338e4528-a708-48ca-91e3-d572cf320179"; //OLD
        public const string SERVER_APPID_REALTIME = "b16994f8-d460-4c79-ae28-568309efff3e"; //NEW
    }


    public const string Avatar_Code = "avatarCode";
    public static string CurrentAvatarName;
    public const int MaxPlayerCount = 20;


    #region PlayerPrefs
    public const string PP_PLAYER_NAME = "Name";
    public const string PP_PLAYER_PRICE = "Price";
    public const string PP_MUSIC_VOLUME = "MusicVolume";
    public const string PP_FX_VOLUME = "FXVolume";
    public const string PP_PLAYER_XP = "XP";
    public const string PP_AVATAR_PIC = "AvatarPic";
    public const string PP_PLAYER_INFO = "PlayerInfo";
    public const string PP_THEME = "Theme";
    public const string PP_IS_AUTHENTICATED = "isAuthenticated";
    public const string PP_AUDIO_TOGGLE = "PlayerAudioToggle";

    //vijendra

    public class PlayerPreference
    {
        public const string PP_ACCOUNT = "Account";
    }

    #endregion

    #region DefaultValues
    public const float DEFAULT_PLAYER_AMT = 10000f;
    public static byte MaxPlayersInGame = 2;
    public const int MAX_PLAYERS_IN_LOBBY = 8;
    public const int CONNECTION_RETRIES_LIMIT = 2;
    public const bool IS_OLD_VERSION = false;
    public const string VERSION = "2.0";
    #endregion

    #region Particles
    public const float MAX_PARTICLE_VOLUME = 0.3f;
    #endregion

    #region Audios
    public const string BUTTON_CLICK_CLIP = "Button Click";
    #endregion

    #region LavaValues
    public const int WIN_XP_LAVA = 5;
    public const int LOSS_XP_LAVA = 2;
    #endregion

}
