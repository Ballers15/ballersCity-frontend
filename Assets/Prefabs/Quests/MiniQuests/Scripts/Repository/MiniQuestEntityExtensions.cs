namespace SinSity.Repo
{
    public static class MiniQuestEntityExtensions
    {
        public static MiniQuestEntity Clone(this MiniQuestEntity entity)
        {
            return new MiniQuestEntity
            {
                id = entity.id,
                json = entity.json
            };
        }
    }
}