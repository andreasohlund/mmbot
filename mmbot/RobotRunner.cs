﻿using System;
using MMBot;

namespace mmbot
{
    public static class RobotRunner
    {
        private static RobotWrapper _wrapper;

        public  static void Run(Options options)
        {
            var childAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString("N"));
            _wrapper = childAppDomain.CreateInstanceAndUnwrap(typeof(RobotWrapper).Assembly.FullName,
                typeof(RobotWrapper).FullName) as RobotWrapper;

            _wrapper.Start(options); //Blocks, waiting on a reset event.

            //Select and ToList called to force re-instantiation of all strings and list itself inside of this outer AppDomain
            //or we will get crazy exceptions related to disposing/unloading of the child AppDomain in which the bot itself runs

            AppDomain.Unload(childAppDomain);

            PackageDirCleaner.CleanUpPackages();

            Run(options);
        }

        public static void Stop()
        {
            _wrapper.Stop();
        }

    }
}