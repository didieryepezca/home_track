using Model.Dto.v1;
using UnitOfWork_Interface;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Model.Entitie;

namespace Service
{
    public interface IFirmaDocService
    {
        Task<TocAutocapturaV2DTO.SessionIdResponse> AutocapturaV2_Liveness_Initialize15MinuteSession(string api_key);

        Task<TocApiFacialDTO.FaceAndDocumentResponse> FaceAndDocumentAPI(string api_key, string tokenFront, string tokenBack, string tokenLiveness, string docType);

        Task<TocApiFacialDTO.FaceAndTokenResponse> FaceAndTokenAPI(string api_key, string tokenLiveness, string toc_token);

        Task<int> CreaTocToken(Ent_Usu_Toc_Token UsuTocToken);

        Task<int> ExisteTocToken(string Usu_NumRUT, bool Usu_Condicion);

        Task<Ent_Usu_Toc_Token> ObtenTocToken_x_Ruc(string Usu_Ruc, bool Usu_Condicion);
    }

    public class FirmaDocService : IFirmaDocService
    {
        private TocAutocapturaV2DTO.SessionIdResponse autocapturaResponse;
        private TocApiFacialDTO.FaceAndDocumentResponse faceAndDocResponse;
        private TocApiFacialDTO.FaceAndTokenResponse faceAndTokenResponse;

        private IUnitOfWork _unitOfWork;

        public FirmaDocService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TocAutocapturaV2DTO.SessionIdResponse> AutocapturaV2_Liveness_Initialize15MinuteSession(string api_key)
        {
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                var baseAddress = "https://sandbox-api.7oc.cl/session-manager/v1/session-id";
                var api = "/controller/action";
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var data = new Dictionary<string, string>
                {
                    {"apiKey",api_key},//“clave” proporcionada por TOC al momento de registrarse como cliente
                    {"autocapture","true"},//enviar este parámetro con valor ‘true’ para indicar que se desea obtener un id de sesión autorizado para el proceso de autocaptura
                    {"liveness","true"},//(opcional)Se debe enviar este parámetro con valor 1 ‘true’ para indicar que se desea obtener un id de sesión autorizado para el proceso de liveness
                    {"liveness_passive","true"}//Se debe enviar este parámetro con el valor ‘true’ para indicar el uso del método pasivo de detección de vida Se recomienda fuertemente que siempre se setee como true este parámetro
                    //{"fake_detector","true"}//(opcional)Se debe enviar este parámetro con valor ‘true’ si se desea obtener un id de sesión autorizado para el proceso de detección de documentos adulterados
                };

                var jsonData = JsonConvert.SerializeObject(data);
                var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(api, contentData);

                var stringData = await response.Content.ReadAsStringAsync();
                autocapturaResponse = JsonConvert.DeserializeObject<TocAutocapturaV2DTO.SessionIdResponse>(stringData);
            }
            return autocapturaResponse;
        }

        public async Task<TocApiFacialDTO.FaceAndDocumentResponse> FaceAndDocumentAPI(string api_key, string
            tokenFront, string tokenBack, string tokenLiveness, string docType)
        {
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                var baseAddress = "https://sandbox-api.7oc.cl/v2/face-and-document";
                var api = "/controller/action";
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var data = new Dictionary<string, string>
                {
                    {"id_front", tokenFront},//Frente del documento En caso de usar este servicio en conjunto con la herramienta de Autocaptura de TOC, este parámetro debe contener String correspondiente a un token de autocaptura
                    {"id_back", tokenBack},//Trasera del documento En caso de usar este servicio en conjunto con la herramienta de Autocaptura de TOC, este parámetro debe contener String correspondiente a un token de autocaptura
                    {"selfie", tokenLiveness},//Debe enviarse como archivo en formato JPG o enviar un token liveness
                    {"apiKey", api_key},//Se debe enviar este parámetro con el valor ‘true’ para indicar el uso del método pasivo de detección de vida Se recomienda fuertemente que siempre se setee como true este parámetro
                    {"documentType", docType},//tipo de documento al que corresponden las imagenes del documento, se debe crear un selector en el front CHL1 (dni chileno moderno) o CHL2 (dni chileno antiguo.)
                };

                var jsonData = JsonConvert.SerializeObject(data);
                var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(api, contentData);
                
                var stringData = await response.Content.ReadAsStringAsync();
                faceAndDocResponse = JsonConvert.DeserializeObject<TocApiFacialDTO.FaceAndDocumentResponse>(stringData);                         
            }
            return faceAndDocResponse;
        }

        public async Task<TocApiFacialDTO.FaceAndTokenResponse> FaceAndTokenAPI(string api_key, string tokenLiveness, string toc_token)
        {
            using (var client = new HttpClient())
            {
                var contentType = new MediaTypeWithQualityHeaderValue("application/json");
                var baseAddress = "https://sandbox-api.7oc.cl/v2/face-and-token";
                var api = "/controller/action";
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Add(contentType);

                var data = new Dictionary<string, string>
                {
                    {"photo", tokenLiveness},//Debe enviarse como archivo en formato JPG o enviar un token liveness
                    {"apiKey", api_key},//Se debe enviar este parámetro con el valor ‘true’ para indicar el uso del método pasivo de detección de vida Se recomienda fuertemente que siempre se setee como true este parámetro                    
                    {"toc_token", toc_token},//tipo de documento al que corresponden las imagenes del documento, se debe crear un selector en el front
                };

                var jsonData = JsonConvert.SerializeObject(data);
                var contentData = new StringContent(jsonData, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(api, contentData);

                //if (response.IsSuccessStatusCode)
                //{
                var stringData = await response.Content.ReadAsStringAsync();
                faceAndTokenResponse = JsonConvert.DeserializeObject<TocApiFacialDTO.FaceAndTokenResponse>(stringData);
                //}               
            }
            return faceAndTokenResponse;
        }

        public async Task<int> CreaTocToken(Ent_Usu_Toc_Token UsuarioTocToken)
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();
                var Usu_TocTokenId = context.Repositories.UsuarioTocRepository.CreaTocToken(UsuarioTocToken);

                if (Usu_TocTokenId > 0)
                {
                    context.SaveChanges();
                    return Usu_TocTokenId;
                }
                else
                {
                    return Usu_TocTokenId;
                }
            });
        }

        public async Task<int> ExisteTocToken(string Usu_NumRUT, bool Usu_Condicion)
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioTocRepository.TocTokenExistente(Usu_NumRUT, Usu_Condicion);
            });
        }

        public async Task<Ent_Usu_Toc_Token> ObtenTocToken_x_Ruc(string Usu_Ruc, bool Usu_Condicion)
        {
            return await Task.Run(() =>
            {
                using var context = _unitOfWork.Create();

                return context.Repositories.UsuarioTocRepository.ObtenTocToken_x_Ruc(Usu_Ruc, Usu_Condicion);
            });
        }
    }
}
