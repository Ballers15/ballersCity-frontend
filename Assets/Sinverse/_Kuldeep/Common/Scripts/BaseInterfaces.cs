#region Common
public interface IInit
{
    public void Init<T>(T Context);

}
public interface IInitContext : IContext, IInit 
{


}
public interface IContext
{
    public void InitProps();
}

#endregion

