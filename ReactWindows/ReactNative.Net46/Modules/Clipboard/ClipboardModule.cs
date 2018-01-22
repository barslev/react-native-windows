﻿using ReactNative.Bridge;
using System;
using System.Runtime.InteropServices;

namespace ReactNative.Modules.Clipboard
{
    /// <summary>
    /// A module that allows JS to get/set clipboard contents.
    /// </summary>
    class ClipboardModule : NativeModuleBase
    {
        private readonly IClipboardInstance _clipboard;
        
        public ClipboardModule() : this(new ClipboardInstance())
        {

        }

        public ClipboardModule(IClipboardInstance clipboard)
        {
            _clipboard = clipboard;
        }

        /// <summary>
        /// The name of the native module.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Clipboard";
            }
        }

        /// <summary>
        /// Get the clipboard content through a promise.
        /// </summary>
        /// <param name="promise">The promise.</param>
        [ReactMethod]
        public void getString(IPromise promise)
        {
            if (promise == null)
            {
                throw new ArgumentNullException(nameof(promise));
            }

            DispatcherHelpers.RunOnDispatcher(() =>
            {
                try
                {
                    if (_clipboard.ContainsText())
                    {
                        var text = _clipboard.GetText();
                        promise.Resolve(text);
                    }
                    else
                    {
                        promise.Resolve("");
                    }
                }
                catch (Exception ex)
                {
                    promise.Reject(ex);
                }
            });
        }

        /// <summary>
        /// Add text to the clipboard or clear the clipboard.
        /// </summary>
        /// <param name="text">The text. If null clear clipboard.</param>
        [ReactMethod]
        public void setString(string text)
        {
            DispatcherHelpers.RunOnDispatcher(new Action(() =>
            {
                try
                {
                    _clipboard.SetText(text);
                }
                catch (Exception)
                {
                    // Ignored
                }
            }));
        }
    }
}
