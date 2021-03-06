﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Fakes;

namespace Microsoft.UI.Test
{
    public class TemplatesFixture
    {
        private static bool syncExecuted = false;

        public TemplatesRepository Repository { get; private set; }

        [SuppressMessage(
        "Usage",
        "VSTHRD002:Synchronously waiting on tasks or awaiters may cause deadlocks",
        Justification = "Required por unit testing.")]
        public void InitializeFixture(string platform, string language)
        {
            var path = $"{Path.GetPathRoot(Environment.CurrentDirectory)}\\UIT\\UI\\";
            var source = new LocalTemplatesSource(null);
            GenContext.Bootstrap(source, new FakeGenShell(platform, language), Platforms.Uwp, language);
            GenContext.Current = new FakeContextProvider
            {
                ProjectName = "Test",
                DestinationPath = path,
                GenerationOutputPath = path,
            };

            if (!syncExecuted)
            {
                GenContext.ToolBox.Repo.SynchronizeAsync(true).Wait();
                syncExecuted = true;
            }

            Repository = GenContext.ToolBox.Repo;
        }
    }
}
