using System;
using System.IO;
using System.Threading.Tasks;
using DryIoc;

namespace MediatR.Examples.DryIoc
{
    class Program
    {
        static Task Main()
        {
            var writer = new WrappingWriter(Console.Out);
            var mediator = BuildMediator(writer);

            return Runner.Run(mediator, writer, "DryIoc");
        }

        private static IMediator BuildMediator(WrappingWriter writer)
        {
            var container = new Container();

            container.RegisterDelegate<SingleInstanceFactory>(r => r.Resolve);
            container.RegisterDelegate<MultiInstanceFactory>(r => serviceType => r.ResolveMany(serviceType));
            container.UseInstance<TextWriter>(writer);

            //Pipeline works out of the box here

            container.RegisterMany(new[] { typeof(IMediator).GetAssembly(), typeof(Ping).GetAssembly() }, Registrator.Interfaces);

            return container.Resolve<IMediator>();
        }
    }
}
