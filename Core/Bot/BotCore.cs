﻿using System;
using System.Threading.Tasks;
using System.Threading;
using Lomztein.Moduthulhu.Core.Bot.Client;
using System.Globalization;

namespace Lomztein.Moduthulhu.Core.Bot {
    public class BotCore : IDisposable
    {
        public DateTime BootDate { get; private set; }
        public TimeSpan Uptime { get => DateTime.Now - BootDate; }

        public BotClient Client { get; private set; }
        private readonly ErrorReporter _errorReporter = new ErrorReporter ();

        internal static string BaseDirectory { get => AppContext.BaseDirectory; }
        internal static string DataDirectory { get => AppContext.BaseDirectory + "/Data"; }
        public static string ResourcesDirectory { get => AppContext.BaseDirectory + "/Resources"; }

        private readonly CancellationTokenSource _shutdownToken = new CancellationTokenSource();

        // TODOD: Create a CoreInitializer or CoreFactory or something, in order to conceal InitializeCore and avoid accidental calls from plugins on an already initialized core.
        public async Task InitializeCore(string[] args)
        {
            // Set up core
            BootDate = DateTime.Now;

            // Set up client manager
            Client = new BotClient(this);
            Client.ExceptionCaught += OnExceptionCaught;
            Client.Initialize().GetAwaiter().GetResult();

            Consent.Init();
            Localization.Init(new CultureInfo("en-US"));

            // Keep the core alive.
            await Task.Delay(-1, _shutdownToken.Token);
            Log.Write(Log.Type.BOT, $"Shutting down..");
            Environment.Exit(0);
        }

        public void Shutdown ()
        {
            _shutdownToken.Cancel();
        }

        private Task OnExceptionCaught(Exception exception)
        {
            return _errorReporter.ReportError(exception);
        }

        public override string ToString() => $"Core uptime: {Uptime.ToString ("%d\\:%h", CultureInfo.InvariantCulture)}\n{Client}";

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            if (managed)
            {
                _shutdownToken.Dispose();
            }
        }
    }
}