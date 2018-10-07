namespace Sababa.Logic.Tests.TestsSelfDIContainer.CustomTypes
{
    public interface IDirector
    {
        bool IsDisposed { get; set; }
        string Command();
    }
}
