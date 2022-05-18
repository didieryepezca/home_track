namespace Repository_Interface.Actions
{
    public interface ICreateRepository<T> where T : class
    {
        int Obten_Cantidad_Existe(T t);

        int Crea(T t);
    }
}
