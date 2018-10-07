using NUnit.Framework;
using Sababa.Logic.DIContainer;
using Sababa.Logic.Tests.TestsSelfDIContainer.CustomTypes;

namespace Sababa.Logic.Tests.TestsSelfDIContainer
{
    [TestOf(typeof(Container))]
    public class TestSelfDIContainer
    {
        private ContainerBuilder _builder;
        private IContainer _container;

        [SetUp]
        public void Init()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<Worker>().As<IWorker>().SingleInstance();
            _builder.RegisterType<Director>().As<IDirector>();
            _builder.RegisterType<CustomTypeAsSelf>().AsSelf();
        }

        [Test]
        public void Registration_GetDoesNotRegistrationType_CheckResultThatArgumentException()
        {
            _container = _builder.Build();

            Assert.That(() => _container.Resolve<NotRegisteredInterface>(), Throws.ArgumentException);
        }

        [Test]
        public void Registration_RegistrationExistsType_CheckResultThatTypeIsRegisteredAndThrowInvalidOperationException()
        {
            Assert.That(() => _builder.RegisterType<Worker>().As<IWorker>(), Throws.InvalidOperationException);
            Assert.That(() => _builder.RegisterType<Manager>().As<IDirector>(), Throws.InvalidOperationException);
            Assert.That(() => _builder.RegisterType<CustomTypeAsSelf>().AsSelf(), Throws.InvalidOperationException);
        }

        [Test]
        public void Registration_RemoveRegistrationType_CheckResultThatObjectDoesNotRegistered()
        {
            Assert.That(() => _builder.UnRegisterType<IWorker>(), Is.True);
            Assert.That(() => _builder.UnRegisterType<IWorker>(), Is.Not.True);
        }

        [Test]
        public void Registration_RegistrationDifferentTypes_CheckResultThatTypeIsNotRegisteredAndThrowInvalidOperationException()
        {
            Assert.That(() => _builder.RegisterType<Worker>().As<IDirector>(), Throws.InvalidOperationException);
        }

        [Test]
        public void BuildContainer_GetObjectByGenericType_CheckObjectOnCorrectValue()
        {
            _container = _builder.Build();

            var director = _container.Resolve<IDirector>();

            Assert.That(() => director, Is.Not.Null);
            Assert.That("Director: Worker work!!!", Is.EqualTo(director.Command()));
        }

        [Test]
        public void BuildContainer_GetSingleObject_CheckResultThatSingleInstance()
        {
            _container = _builder.Build();

            var worker = _container.Resolve<IWorker>();
            var worker2 = _container.Resolve<IWorker>();

            Assert.That(worker.GetHashCode(), Is.EqualTo(worker2.GetHashCode()));
            Assert.That(worker, Is.SameAs(worker2));
        }

        [Test]
        public void BuildContainer_SetPropertyExternallyOwnedIsTrueInMethodDispose_CheckResultPropertyExternallyOwned()
        {
            _container = _builder.Build();

            var director = _container.Resolve<IDirector>();
            _container.Dispose();

            Assert.That(() => director.IsDisposed, Is.True);
        }
    }
}
