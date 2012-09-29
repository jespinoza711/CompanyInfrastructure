
namespace Company.Infrastructure.DataAccessLayer
{
    public interface IObjectContextProvider
    {
        System.Data.Objects.ObjectContext ObjectContext { get; }
    }
}
