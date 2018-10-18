using System;
using System.Threading.Tasks;

namespace Playtika.NETSummit.Problem1
{
    public class Controller : IDisposable
    {
        private readonly IUiFactory _uiFactory;
        private readonly IApi _api;

        private IView _view;

        public Controller(IUiFactory uiFactory, IApi api)
        {
            _uiFactory = uiFactory;
            _api = api;
        }

        public void Initialize()
        {
            _view = _uiFactory.Create<IView>();
        }

        public void Start()
        {
            _view.Clicked += ViewOnClicked;
        }

        public void Stop()
        {
            _view.Clicked -= ViewOnClicked;
        }

        public void Dispose()
        {
            _view.Dispose();
        }

        private async void ViewOnClicked()
        {
            _view.Temperature = await _api.GetTemperature();
        }
    }

    public interface IUiFactory
    {
        T Create<T>();
    }

    public interface IView : IDisposable
    {
        event Action Clicked;

        int Temperature { set; }
    }

    public interface IApi
    {
        Task<int> GetTemperature();
    }
}