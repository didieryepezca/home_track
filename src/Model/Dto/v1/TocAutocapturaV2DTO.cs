namespace Model.Dto.v1
{
    public class TocAutocapturaV2DTO
    {
        public class SessionIdResponse { 
        
            public string session_id { get; set; }

            public int status { get; set; } // codigo 200 Generacion de session id exitosa        
        }

        public class SessionRespMsg
        {
            public string session_id { get; set; }

            public string msg { get; set; } 
        }


        public class AutocaptureOptions { //Enviar desde el FRONT-END

            public string locale { get; set; }

            public string session_id { get; set; }

            public string document_type { get; set; } //Crear un selector en FRONT-END para elegir el tipo de documento antiguo o nuevo.

            public string document_side { get; set; }               
        }

        public class AutocaptureOptionsCallBackDocFront { //Respuesta Callback para el frente del documento

            public string captured_token { get; set; } //Token del Frente y Atras del Documento a enviar a API Facial.

            public string image { get; set; }

            public string data { get; set; }
        }

        public class AutocaptureOptionsCallBackDocBack //Respuesta Callback para la trasera del documento
        {
            public string captured_token { get; set; } //Token del Frente y Atras del Documento a enviar a API Facial.

            public string image { get; set; }

            public string data { get; set; }
        }

        public class AutocaptureOptionsCallBackFailure//Capturar respuesta desde el FRONT-END
        { 
            public string error { get; set; }            

            public string data { get; set; }
        }
    }
}