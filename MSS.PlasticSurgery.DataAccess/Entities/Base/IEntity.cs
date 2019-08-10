namespace MSS.PlasticSurgery.DataAccess.Entities.Base
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}