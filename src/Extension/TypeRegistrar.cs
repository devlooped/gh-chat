using Microsoft.Extensions.DependencyInjection;

namespace Devlooped;

public sealed class TypeRegistrar(IServiceCollection? builder = default) : ITypeRegistrar
{
    readonly IServiceCollection builder = builder ?? new ServiceCollection();
    IServiceProvider? services;

    public IServiceProvider Services => services ??= builder.BuildServiceProvider();

    ITypeResolver ITypeRegistrar.Build() => new TypeResolver(() => Services);

    //public IServiceProvider Build() => Services;

    public void Register(Type service, Type implementation) => builder.AddSingleton(service, implementation);

    public void RegisterInstance(Type service, object implementation) => builder.AddSingleton(service, implementation);

    public void RegisterLazy(Type service, Func<object> func)
    {
        ThrowIfNull(func);
        builder.AddSingleton(service, (provider) => func());
    }

    sealed class TypeResolver(Func<IServiceProvider> provider) : ITypeResolver, IDisposable
    {
        readonly Func<IServiceProvider> provider = provider ?? throw new ArgumentNullException(nameof(provider));

        public object? Resolve(Type? type) => type == null ? null : provider().GetService(type);

        public void Dispose() => (provider() as IDisposable)?.Dispose();
    }
}