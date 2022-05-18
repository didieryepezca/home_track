using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Service;
using AutoMapper;
using Model.Dto.v1;
using Newtonsoft.Json;
using Model.Entitie;

namespace Home_Track_API.Controllers.v1
{

    [Route("api/v{version:apiVersion}/[Controller]")]
    [ApiVersion("1")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FirmaDocController : ControllerBase
    {
        private readonly IFirmaDocService _firmaService;
        private IMapper _mapper;

        private TocAutocapturaV2DTO.SessionIdResponse autocapturaResponse;
        private TocApiFacialDTO.FaceAndDocumentResponse faceAndDocResponse;
        private TocApiFacialDTO.FaceAndTokenResponse faceAndTokenResponse;

        private Ent_Usu_Toc_Token usuTocToken;

        public FirmaDocController(IFirmaDocService FirmaService, IMapper mapper)
        {
            _firmaService = FirmaService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> InitializeTocSessions()
        {
            try
            {
                string api_key = ""; //Setear la API Key de TOC

                var session = await _firmaService.AutocapturaV2_Liveness_Initialize15MinuteSession(api_key);
                if (session is null) {

                    return BadRequest("No se obtuvo ninguna respuesta del Servidor de Sesiones");
                }
                _mapper.Map(autocapturaResponse, session);
                var sessionID = session.session_id; //retornar este ID para realizar la petición de Autocaptura de documentos Y Liveness.
                                                                      //documento AutocapturaV2,pagina 7. FRONT-END consumo
                                                                      //documento Liveness, pagina 7. FRONT-END consumo
                var sessionStatus = session.status;

                var fResp = new TocAutocapturaV2DTO.SessionRespMsg();

                switch (sessionStatus)
                {
                    case 200:
                        fResp.session_id = sessionID;
                        fResp.msg = "Proceso concluyó sin interrupciones";
                        break;                                       
                    case 400:
                        fResp.session_id = sessionID;
                        fResp.msg = "Request mal formado";
                        break;
                    case 401:
                        fResp.session_id = sessionID;
                        fResp.msg = "Servicio no habilitado";
                        break;
                    case 402:
                        fResp.session_id = sessionID;
                        fResp.msg = "El límite de transacciones o tiempo de prueba del cliente fue superado.";
                        break;
                    case 411:
                        fResp.session_id = sessionID;
                        fResp.msg = "Parámetros malos o faltantes. Verifique que el request contenga todos los parámetros indicados y que los nombres coinciden";
                        break;
                    case 421:
                        fResp.session_id = sessionID;
                        fResp.msg = "apiKey no existe";
                        break;
                    case 422:
                        fResp.session_id = sessionID;
                        fResp.msg = "mode no existe";
                        break;
                    case 423:
                        fResp.session_id = sessionID;
                        fResp.msg = "Valor incorrecto de autocapture o liveness";
                        break;
                    case >500:
                        fResp.session_id = sessionID;
                        fResp.msg = "Error interno, contactar a soporte";
                        break;
                }
                return Ok(fResp);                                   
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error interno del servidor: " + e.Message);
            }
        }

        //Recibir respuestas desde el envío de datos del Front.
        [HttpPost]
        public async Task<IActionResult> TocFaceAndDocument(string frontDocResp, string backDocResp, string liveResp, string docType) 
        {
            try
            {
                string api_key = ""; //Setear la API Key de TOC
                string UsuNumRuc = ""; //¿Como obtener el usuario que actualmente se encuentra logeado?

                //data obtenida luego de tomar foto al documento y tomarse el selfie a través de la peticion en el Front-end
                var fDocData = JsonConvert.DeserializeObject<TocAutocapturaV2DTO.AutocaptureOptionsCallBackDocFront>(frontDocResp);
                var bDocData = JsonConvert.DeserializeObject<TocAutocapturaV2DTO.AutocaptureOptionsCallBackDocBack>(backDocResp);
                var liveData = JsonConvert.DeserializeObject<TocLivenessDTO.LivenessOptionsCallBack>(liveResp);

                //Toc Token ya obtenido con anterioridad.
                var UsuTocToken = await _firmaService.ExisteTocToken(UsuNumRuc, true);
                if (UsuTocToken != 0)
                {
                    var UsuTocTokenInfo = await _firmaService.ObtenTocToken_x_Ruc(UsuNumRuc, true);
                    return RedirectToAction("TocFaceAndToken", new { tokenLiveness = liveData.liveness_token, tokenEncontrado = UsuTocTokenInfo.UsuTocTok_Token });
                }                                 

                var faceAndDoc = await _firmaService.FaceAndDocumentAPI(api_key, fDocData.captured_token, bDocData.captured_token,liveData.liveness_token, docType);
                if (faceAndDoc is null)
                {                    
                    return BadRequest("No se obtuvo ninguna respuesta del Servidor de Validación");
                }
                _mapper.Map(faceAndDocResponse, faceAndDoc);
                
                var status = faceAndDoc.status;
                var biometric_res = faceAndDoc.biometric_result;
                var toc_token = faceAndDoc.toc_token;

                var fResp = new TocApiFacialDTO.FaceAndDocumentRespMsg();

                var UsuarioTocToken = new Ent_Usu_Toc_Token();
                _mapper.Map(usuTocToken, UsuarioTocToken);

                UsuarioTocToken.UsuTocTok_Rut = UsuNumRuc;
                UsuarioTocToken.UsuTocTok_Token = toc_token;
                UsuarioTocToken.UsuTocTok_Fecha_Reg = DateTime.Now;
                UsuarioTocToken.UsuTocTok_Condicion = true;

                switch (status)
                {
                    case 200:             
                        await _firmaService.CreaTocToken(UsuarioTocToken);
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y toda la información correspondiente fue devuelta";
                        break;
                    case 201:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y solo se encontró la data del documento(función selfie vs documento de identidad)";
                        break;
                    case 202:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y solo se pudo realizar match biométrico(función selfie vs documento de identidad)";
                        break;
                    case 203:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó sin interrupciones y no se encontró información para devolver (no se detectó rostro y / o no se pudo leer información del documento)";
                        break;
                    case 400:
                        fResp.status = status;
                        fResp.msg = "Request mal formado, verificar Content - Type";
                        break;
                    case 401:
                        fResp.status = status;
                        fResp.msg = "Servicio no habilitado";
                        break;
                    case 402:
                        fResp.status = status;
                        fResp.msg = "El límite de transacciones o tiempo de prueba del cliente fue superado.";
                        break;                    
                        //existen mas estados, ¿ colocarlos todos ?
                    case > 500:
                        fResp.status = status;
                        fResp.msg = "Error interno, contactar a soporte";
                        break;
                }
                return Ok(fResp);
                //return StatusCode(200, "Se validó correctamente el rostro con el documento");
            }
            catch (Exception e) {

                return StatusCode(500, "Error interno del servidor: " + e.Message);
            }        
        }

        [HttpPost]
        public async Task<IActionResult> TocFaceAndToken(string livenessToken, string tokenEncontrado)
        {
            try
            {
                string api_key = ""; //Setear la API Key de TOC

                var tokenAndDoc = await _firmaService.FaceAndTokenAPI(api_key, livenessToken,tokenEncontrado);
                if (tokenAndDoc is null)
                {
                    return BadRequest("No se obtuvo ninguna respuesta del Servidor de Validación");
                }
                _mapper.Map(faceAndTokenResponse, tokenAndDoc);

                var status = tokenAndDoc.status;

                var fResp = new TocApiFacialDTO.FaceAndTokenRespMsg();

                switch (status)
                {
                    case 200:                        
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y toda la información correspondiente fue devuelta";
                        break;
                    case 201:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y solo se encontró la data del documento(función selfie vs documento de identidad)";
                        break;
                    case 202:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó y solo se pudo realizar match biométrico(función selfie vs documento de identidad)";
                        break;
                    case 203:
                        //await _firmaService.CreaTocToken(UsuarioTocToken);?
                        fResp.status = status;
                        fResp.msg = "Proceso concluyó sin interrupciones y no se encontró información para devolver (no se detectó rostro y / o no se pudo leer información del documento)";
                        break;
                    case 400:
                        fResp.status = status;
                        fResp.msg = "Request mal formado, verificar Content - Type";
                        break;
                    case 401:
                        fResp.status = status;
                        fResp.msg = "Servicio no habilitado";
                        break;
                    case 402:
                        fResp.status = status;
                        fResp.msg = "El límite de transacciones o tiempo de prueba del cliente fue superado.";
                        break;
                    //existen mas estados, ¿ colocarlos todos ?
                    case > 500:
                        fResp.status = status;
                        fResp.msg = "Error interno, contactar a soporte";
                        break;
                }
                return Ok(fResp);                
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error interno del servidor: " + e.Message);
            }
        }
    }
}
