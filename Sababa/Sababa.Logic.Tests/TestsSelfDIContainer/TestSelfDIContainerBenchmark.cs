using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Sababa.Logic.DIContainer;
using Sababa.Logic.Tests.TestsSelfDIContainer.CustomTypes;

namespace Sababa.Logic.Tests.TestsSelfDIContainer
{
    public class TestSelfDIContainerBenchmark
    {
        private DIContainer.ContainerBuilder _builderSelf;
        private DIContainer.IContainer _containerSelf;
        private ContainerBuilder _builder;
        private IContainer _container;
        private int _count = 500_000;

        [Benchmark]
        public void RegisterTypesSelfDI_InitArrayObjects_CallMethodDisposeInContainer()
        {
            _builderSelf = new DIContainer.ContainerBuilder();
            _builderSelf.RegisterType<Worker>().As<IWorker>().SingleInstance();
            _builderSelf.RegisterType<Director>().As<IDirector>();
            _builderSelf.RegisterType<CustomTypeAsSelf>().AsSelf();
            _containerSelf = _builderSelf.Build();

            var array = new object[_count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _containerSelf.Resolve<IDirector>();
            }

            var lastItem = array[array.Length - 1];
            _containerSelf.Dispose();
        }

        [Benchmark]
        public void RegisterTypesSelfDI_InitArraySingleInstance_CallMethodDisposeInContainer()
        {
            _builderSelf = new DIContainer.ContainerBuilder();
            _builderSelf.RegisterType<Worker>().As<IWorker>().SingleInstance();
            _builderSelf.RegisterType<Director>().As<IDirector>();
            _builderSelf.RegisterType<CustomTypeAsSelf>().AsSelf();
            _containerSelf = _builderSelf.Build();

            var array = new object[_count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _containerSelf.Resolve<IWorker>();
            }

            var lastItem = array[array.Length - 1];
            _containerSelf.Dispose();
        }

        [Benchmark]
        public void RegisterTypesDIAutofac_InitArrayObjects_CallMethodDisposeInContainer()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<Worker>().As<IWorker>().SingleInstance();
            _builder.RegisterType<Director>().As<IDirector>();
            _builder.RegisterType<CustomTypeAsSelf>().AsSelf();
            _container = _builder.Build();

            var array = new object[_count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _container.Resolve<IDirector>();
            }
            var obj = array[array.Length - 1];
            _container.Dispose();
        }

        [Benchmark]
        public void RegisterTypesDIAutofac_InitArraySingleInstance_CallMethodDisposeInContainer()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<Worker>().As<IWorker>().SingleInstance();
            _builder.RegisterType<Director>().As<IDirector>();
            _builder.RegisterType<CustomTypeAsSelf>().AsSelf();
            _container = _builder.Build();

            var array = new object[_count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = _container.Resolve<IWorker>();
            }
            var obj = array[array.Length - 1];
            _container.Dispose();
        }

        [Test]
        public void PerformanceComparisonDISelf_VS_DIAutofac()
        {
            var summary = BenchmarkRunner.Run<TestSelfDIContainerBenchmark>();
        }
    }
}
