namespace POS.Persistence.PostgreSql.Abstractions;
internal interface IEntity<TID>
{
    TID Id { get; set; }
}
