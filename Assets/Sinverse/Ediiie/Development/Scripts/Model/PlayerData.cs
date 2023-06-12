using Ediiie.Model;
using Ediiie.Audio;
using System;
using UnityEngine;

public class PlayerData : MonoBehaviour
{

    #region Statics
    protected static PlayerData m_Instance;
    
    #region Properties
    public static PlayerData Instance { get { return m_Instance; } }
    public static string UserId => BaseModel.User.id;
    public static string UserName => m_Instance.playerName;
    #endregion

    #region Action
    public static Action OnPlayerDataChanged;
    #endregion
    
    #endregion

    #region Variables
    public Sprite playerSprite;
    public string playerName;
    public float fxVolume;
    public float musicVolume;
    public int avatarIndex;
    public int coins = 0;
    public int playerXP;
    #endregion

    //check player data is loading wrong level from level selection screen
    private void Awake()
    {
        m_Instance = this;

        //SettingPopup.OnVolumeChanged += SetAudioVolume;
    }

    private void OnDestroy()
    {
        //SettingPopup.OnVolumeChanged -= SetAudioVolume;
    }

    private void Start()
    {
        Load();
    }

    public static void SaveSprite(Sprite sprite, int index)
    {
        Instance.playerSprite = sprite;
        Instance.avatarIndex = index;

        Save();
    }

    public static void Save()
    {    
        //PlayerPrefs.SetInt(Constants.PP_PLAYER_XP, Instance.playerXP);
        PlayerPrefs.SetFloat(Constants.PP_MUSIC_VOLUME, Instance.musicVolume);
        PlayerPrefs.SetFloat(Constants.PP_FX_VOLUME, Instance.fxVolume);
        PlayerPrefs.SetString(Constants.PP_PLAYER_NAME, Instance.playerName);
        PlayerPrefs.SetInt(Constants.PP_AVATAR_PIC, Instance.avatarIndex);
    }

    public void Load()
    {
        //playerXP = PlayerPrefs.GetInt(Constants.PP_PLAYER_XP, 0); //not maintained yet
        fxVolume = PlayerPrefs.GetFloat(Constants.PP_FX_VOLUME, 0.5f);
        musicVolume = PlayerPrefs.GetFloat(Constants.PP_MUSIC_VOLUME, 0.5f);
        playerName = PlayerPrefs.GetString(Constants.PP_PLAYER_NAME, "Player 1");
        avatarIndex = PlayerPrefs.GetInt(Constants.PP_AVATAR_PIC, 0);

        ApplyGameSettings();
    }

    private void ApplyGameSettings()
    {
        //playerSprite = PoolHolder.AvatarsPool.GetAvatar(avatarIndex);

        AudioManager.Instance.SetAudioVolume(true, musicVolume);
        AudioManager.Instance.SetAudioVolume(false, fxVolume);
    }

    private void SetAudioVolume(bool isMusic, float value)
    {
        if (isMusic)
        {
            musicVolume = value;
        }
        else
        {
            fxVolume = value;
        }

        Save();
    }

    public static void ResetData()
    {
        PlayerPrefs.DeleteAll();
        //m_Instance.playerSprite = PoolHolder.AvatarsPool.GetRandomAvatar();
    }
}
