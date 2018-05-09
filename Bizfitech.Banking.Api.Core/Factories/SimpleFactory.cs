using Bizfitech.Banking.Api.Core.Interfaces;
using System;

namespace Bizfitech.Banking.Api.Core.Factories
{
    public class SimpleFactory : IFactory
    {
        public TType Get<TType>(params object[] args)
        {
            return (TType)Activator.CreateInstance(typeof(TType), args);
        }
    }
}
