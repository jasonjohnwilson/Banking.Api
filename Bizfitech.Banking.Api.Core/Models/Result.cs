namespace Bizfitech.Banking.Api.Core.Models
{
    public class Result<TObject>  where TObject : class
    {
        public string Message { get; set; }
        public ErrorCategory ErrorCategory { get; set; } = ErrorCategory.NoError;
        public TObject Obj { get; set; }
    }
}
