﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Gma.UserActivityMonitor
{
    /// <summary>
    /// Provides kernal-based services
    /// </summary>
    public class Kernel32
    {
        /// <summary>
        /// Gets a module handle
        /// </summary>
        /// <param name="lpFileName">file name</param>
        /// <returns>module handle</returns>
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
    }
}
