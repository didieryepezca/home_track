namespace Model
{
	public class PropiedadParameters
	{
		const int maxRegistroPagina = 50;

		public int NumeroPagina { get; set; } = 1;

		private int _RegistroPagina = 30;

		public int RegistroPagina
		{
			get
			{
				return _RegistroPagina;
			}
			set
			{
				_RegistroPagina = (value > maxRegistroPagina) ? maxRegistroPagina : value;
			}
		}

		public string SearchTerm { get; set; }
		//public string OrderBy { get; set; } = "name";
	}
}
