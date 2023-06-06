using System;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace TodoTrack.Cli
{
    public sealed class TypeRegistrar : ITypeRegistrar
    {
        private readonly IServiceCollection _builder;
        private ITypeResolver? _typeResolver;
        private IServiceProvider _serviceProvider;

        public TypeRegistrar(IServiceCollection builder)
        {
            _builder = builder;
        }

        public ITypeResolver Build()
        {
            _serviceProvider ??= _builder.BuildServiceProvider();
            return new TypeResolver(_serviceProvider.CreateScope().ServiceProvider);
            //return _typeResolver ??= new TypeResolver(_builder.BuildServiceProvider());
        }

        public void Register(Type service, Type implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterInstance(Type service, object implementation)
        {
            _builder.AddSingleton(service, implementation);
        }

        public void RegisterLazy(Type service, Func<object> func)
        {
            if (func is null)
            {
                throw new ArgumentNullException(nameof(func));
            }

            _builder.AddSingleton(service, (provider) => func());
        }
    }
}
