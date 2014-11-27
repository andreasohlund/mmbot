using System;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using MMBot;

namespace mmbot
{
    partial class Service : ServiceBase
    {
        readonly Options _options;
        Thread _thread;
        bool _shutdownRequested;
        ManualResetEvent _robotIsStopped;
        bool _isStopped;

        public Service(Options options)
        {
            _options = options;
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            if (_options.LastParserState != null && _options.LastParserState.Errors.Any())
            {
                return;
            }
            _robotIsStopped = new ManualResetEvent(false);

            _thread = new Thread(MMBotWorkerThread);
            _thread.Name = "MMBot Worker Thread";
            _thread.IsBackground = true;
            _thread.Start();
        }

        protected override void OnStop()
        {
            _shutdownRequested = true;
            RobotRunner.Stop();

            _robotIsStopped.WaitOne(TimeSpan.FromSeconds(20));

            if (!_isStopped)
            {
                _thread.Abort();
            }

        }

        void MMBotWorkerThread()
        {
            while (!_shutdownRequested)
            {
                RobotRunner.Run(_options);
            }
            _isStopped = true;
        }
    }
}