using Model;

namespace Home_Track_WebApp.HttpRepository
{
	public class PagingResponse<T> where T : class
	{
		public List<T> Cuerpo { get; set; }

		public MetaData MetaData { get; set; }
	}
}