namespace Domain.Entities
{
    public class BaseEntity<TKey>  // TKey => To Specify the primary key Type
    {
        public TKey Id { get; set; }
    }
}
