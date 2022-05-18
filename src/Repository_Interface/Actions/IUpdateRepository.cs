namespace Repository_Interface.Actions
{
    public interface IUpdateRepository<T> where T : class
    {
        int Obten_Cantidad_Existente(T t);

        int Actualiza(T t);

        int Actualiza_Condicion(T t);
    }
}
