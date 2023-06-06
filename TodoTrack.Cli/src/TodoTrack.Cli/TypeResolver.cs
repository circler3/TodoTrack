using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using SQLitePCL;

namespace TodoTrack.Cli
{

    public sealed class TypeResolver : ITypeResolver, IDisposable
    {
        private readonly IServiceProvider _provider;

        public TypeResolver(IServiceProvider provider)
        {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public object? Resolve(Type? type)
        {
            if (type == null)
            {
                return null;
            }

            return _provider.GetService(type);
        }

        /// <summary>
        /// ServiceProvider as a singleton to run longtime.
        /// </summary>
        public void Dispose()
        {
            if (_provider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}