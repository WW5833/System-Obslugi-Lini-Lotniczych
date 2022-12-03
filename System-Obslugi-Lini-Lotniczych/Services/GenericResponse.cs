namespace LotSystem.Services;

public readonly struct GenericResponse
{
    public bool Successfull => Message is null;
    public readonly string Message;

    public GenericResponse(string message)
    {
        Message = message;
    }

    public GenericResponse()
    {
        Message = null;
    }

    public static implicit operator bool(GenericResponse me)
    {
        return me.Successfull;
    }

    public static implicit operator GenericResponse(string me)
    {
        return new GenericResponse(me);
    }
}