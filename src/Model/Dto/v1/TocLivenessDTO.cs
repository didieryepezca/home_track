namespace Model.Dto.v1
{
    public class TocLivenessDTO
    {
        public class LivenessOptions
        { //Enviar desde el FRONT-END

            public string locale { get; set; }

            public string session_id { get; set; }                        
        }

        public class LivenessOptionsCallBack
        { //Capturar respuesta desde el FRONT-END

            public string liveness_token { get; set; } //Token a enviar a API Facial.

            public string image { get; set; }

            public string data { get; set; }
        }

        public class LivenessOptionsCallBackFailure
        { //Capturar respuesta desde el FRONT-END

            public string error { get; set; }

            public string data { get; set; }
        }

    }
}
