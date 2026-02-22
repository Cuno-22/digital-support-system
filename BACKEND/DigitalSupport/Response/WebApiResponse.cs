using System.Collections.Generic;

namespace WebApiDigitalSupport.Response
{
    public class WebApiResponse<T>
    {
        public bool Success { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
        public Response<T> Response { get; set; } = new Response<T>();
    }

    public class Response<T>
    {
        // Siempre una lista cuando usas esta versión
        public List<T> Data { get; set; } = new List<T>();
    }
    public class WebApiResponseV2<T>
    {
        public bool Success { get; set; }
        public List<Error> Errors { get; set; } = new List<Error>();
        public ResponseV2<T> Response { get; set; } = new ResponseV2<T>();
    }

    public class ResponseV2<T>
    {
        // Un único objeto útil para resultados tipo { nRetorno, sRetorno }
        public T Data { get; set; }
    }
}