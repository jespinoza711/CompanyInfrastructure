
namespace Company.DataAccessLayer.Infrastructure
{
    public interface IObjectContextProvider
    {
        System.Data.Objects.ObjectContext ObjectContext { get; }
    }
}
