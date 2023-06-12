using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "AvatarsPool", menuName = "Auto Battler/Avatars Pool", order = 1)]
public class AvatarsPool : ScriptableObject
{
    public GameObject avatarPrefab;
    public List<Sprite> avatars;
    
    public void GenerateAvatars(ToggleGroup toggleGroup)
    {
        for (int i = 0; i < avatars.Count; i++)
        {
            GameObject avatar = Instantiate(avatarPrefab, toggleGroup.transform);
            avatar.GetComponent<ProfilePicToggleView>().SetData(avatars[i], i, toggleGroup);
        }
    }

    public Sprite GetAvatar(int index)
    {
        return avatars[index];
    }

    public Sprite GetRandomAvatar()
    {
        int index = 0; //Random.Range(0, avatars.Count);
        return avatars[index];
    }
}
