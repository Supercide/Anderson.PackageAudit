namespace Anderson.PackageAudit.Core.Errors
{
    //TODO: TIn is redundant and should be removed
    public interface IErrorResolver<TIn, out TOut> where TIn : Error
    {
        TOut Resolve(Error error);
    }
}