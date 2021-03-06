﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.ExpressApp.Blazor.Services;
using Microsoft.AspNetCore.Components.Server.Circuits;

namespace JKXAF.Blazor.Server {
    internal class ValueManagerStorageSynchronizer : CircuitHandler {
        private readonly IValueManagerStorageAccessor storageAccessor;
        private IValueManagerStorage storage;

        public ValueManagerStorageSynchronizer(IValueManagerStorageAccessor storageAccessor) {
            this.storageAccessor = storageAccessor;
        }

        public override Task OnConnectionUpAsync(Circuit circuit,
            CancellationToken cancellationToken) {
            if(storage != null) {
                storageAccessor.Storage = storage;
            }
            else {
                storage = storageAccessor.Storage;
            }
            return Task.CompletedTask;
        }
    }
}
