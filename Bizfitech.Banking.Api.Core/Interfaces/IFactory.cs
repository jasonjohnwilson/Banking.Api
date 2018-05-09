namespace Bizfitech.Banking.Api.Core.Interfaces
{
    public interface IFactory
    {
        TType Get<TType>(params object[] args);
    }
}
