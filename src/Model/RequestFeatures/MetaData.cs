namespace Model
{
    public class MetaData
    {
        public int RegistroPagina { get; set; }
        public int NumeroPagina { get; set; }
        public int TotalPagina { get; set; }
        public int TotalRegistro { get; set; }

        public bool TienePaginaAnterior => NumeroPagina > 1;
        public bool TienePaginaProximo => NumeroPagina < TotalPagina;
    }
}
