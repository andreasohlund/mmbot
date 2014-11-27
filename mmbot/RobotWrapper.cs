using System;
using System.Threading;
using MMBot;

namespace mmbot
{
    public class RobotWrapper : MarshalByRefObject
    {
        static Robot _robot;

        public void Start(Options options)
        {
            _robot = Initializer.StartBot(options).Result;

            if (_robot == null)
            {
                // Something went wrong. Abort
                Environment.Exit(-1);
            }

            var resetEvent = new AutoResetEvent(false);
            _robot.ResetRequested += (sender, args) => resetEvent.Set();
            resetEvent.WaitOne();
        }

        public void Stop()
        {
            _robot.Shutdown()
                .Wait(TimeSpan.FromSeconds(10));
        }
    }
}