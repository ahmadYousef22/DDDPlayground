namespace DDDPlayground.Shared.Enums
{
    public enum HttpStatusCode
    {
        None,
        OK = 200,
        BadRequest = 400,
        NotAuthenticated = 401,
        NotAuthorize = 403 ,
        NotFound = 404,
        InternalError = 500
    }
}
