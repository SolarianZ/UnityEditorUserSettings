﻿using System;
using UnityEngine.Assertions;

namespace GBG.EditorUserSettings.Editor
{
    public struct Batching : IDisposable
    {
        private IEditorUserSettingsStorage _storage;

        internal Batching(IEditorUserSettingsStorage storage)
        {
            _storage = storage;
            Assert.IsTrue(_storage.BatchingCounter > 0);
        }

        void IDisposable.Dispose()
        {
            if (_storage == null)
            {
                return;
            }

            _storage.EndBatching();
            _storage = null;
        }
    }
}