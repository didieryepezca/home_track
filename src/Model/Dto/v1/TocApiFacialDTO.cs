namespace Model.Dto.v1
{
    public class TocApiFacialDTO
    {
        public class FaceAndDocumentRespMsg
        {
            public int status { get; set; }

            public string msg { get; set; }
        }

        public class FaceAndDocumentResponse { 
        
            public int status { get; set; }

            public int biometric_result { get; set; } // != 2 no hubo match biometrico

            public string toc_token { get; set; } // token a almacenar conjuntamente con el RUT del cliente para futuras firmas.

            //public class information_from_document{

            //    public string type { get; set; }

            //}
        }

        public class FaceAndTokenRespMsg
        {
            public int status { get; set; }

            public string msg { get; set; }            
        }

        public class FaceAndTokenResponse
        {
            public int status { get; set; }

            public int biometric_result { get; set; } // != 2 no hubo match biometrico

            public string toc_token { get; set; } // token a almacenar conjuntamente con el RUT del cliente para futuras firmas.
        }
    }
}