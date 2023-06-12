using Ediiie.Model;
using UnityEngine;

public class PoolHolder : BaseModel
{
    [SerializeField] private AvatarsPool avatarPool;
 
    public static PoolHolder Instance;
    public static AvatarsPool AvatarsPool => Instance.avatarPool;
 
    private void Awake()
    {
        Instance = this;
    }
}
