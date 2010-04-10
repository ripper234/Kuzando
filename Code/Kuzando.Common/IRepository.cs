namespace Kuzando.Common
{
    public interface IRepository<T>
    {
        void Save(T entity);
        T GetById(int id);
    }
}