namespace Repository_Interface.Actions
{
    public interface IRemoveRepository<T> where T : class
    {
        int Elimina(T t);
    }
}
