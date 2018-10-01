using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sababa.logic.HttpClient.Tests
{
    public class Listener : IListener
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public Listener(params string[] prefixes)
        {
            if (prefixes == null || prefixes.Length == 0)
            {
                //log
                throw new ArgumentException("Prefixes needed!!!");
            }

            foreach (string item in prefixes)
            {
                _listener.Prefixes.Add(item);
            }
        }

        public void Start()
        {
            try
            {
                _listener.Start();
                ListenTraces();
            }
            catch (HttpListenerException e)
            {
                //log.Warn($"Can't start the agent to listen transaction {e}");
                throw;
            }
        }

        public void Stop()
        {
            _listener.Stop();
            _cts.Cancel();
        }

        private void ListenTraces()
        {
            Task.Run(async () =>
            {
                await WorkerListenet();
            }, _cts.Token);
        }

        private async Task<Stream> WorkerListenet()
        {
            var token = _cts.Token;

            while (!token.IsCancellationRequested)
            {
                var cont = await _listener.GetContextAsync();
                var req = cont.Request;
                var res = cont.Response;

                switch (req.HttpMethod)
                {
                    case "GET":
                        {
                            Book book = new Book(1, "Cats", 5000);
                            await GetResponseJson(book).CopyToAsync(res.OutputStream);
                            res.OutputStream.Close();
                            break;
                        }
                    case "PUT":
                        {
                            AddHeader(req, res);
                            await req.InputStream.CopyToAsync(res.OutputStream);
                            res.OutputStream.Close();
                            break;
                        }
                    case "DELETE":
                        {
                            res.StatusCode = 200;
                            res.OutputStream.Close();
                            break;
                        }
                    default:
                        {
                            await req.InputStream.CopyToAsync(res.OutputStream);
                            res.OutputStream.Close();
                            break;
                        }
                }
            }

            return null;
        }

        private Stream GetResponseJson(Book book)
        {
            var _jsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };
            var memory = new MemoryStream();
            TextWriter writer = new StreamWriter(memory);
            JsonSerializer.CreateDefault(_jsonSettings).Serialize(writer, book);
            writer.Flush();
            memory.Position = 0;

            return memory;
        }

        private void AddHeader(HttpListenerRequest req, HttpListenerResponse res)
        {
            foreach (var item in req.Headers.Keys)
            {
                if (item.Equals("Hello"))
                {
                    res.AddHeader("Hello", "World");
                }
            }
        }

    }
}
