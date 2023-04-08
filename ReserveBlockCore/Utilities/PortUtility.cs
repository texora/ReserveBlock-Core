﻿using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace ReserveBlockCore.Utilities
{
    public class PortUtility
    {
        public static bool IsUdpPortInUse(int port)
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            var udpListeners = ipGlobalProperties.GetActiveUdpListeners();
            return udpListeners.Any(l => l.Port == port);
        }
        public static bool IsUdpPortInUseOSAgnostic(int port)
        {
            try
            {
                using (var client = new UdpClient(port))
                {
                    return false;
                }
            }
            catch (SocketException ex)
            {
                if (ex.SocketErrorCode == SocketError.AddressAlreadyInUse)
                {
                    return true;
                }
                else
                {
                    throw;
                }
            }
        }

        public static int FindOpenUDPPort(int port)
        {
            var portFound = false;
            var count = 1;
            if (port > 50000)
                port = Globals.DSTClientPort + 1; // reset port back down

            while (!portFound) 
            {
                var nextPort = port + count;
                var portInUse = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? IsUdpPortInUse(nextPort) : IsUdpPortInUseOSAgnostic(nextPort);
                if(!portInUse)
                {
                    portFound = true;
                    return nextPort;
                }
                else
                {
                    count += 1;
                }
            }

            return -1;
        }
    }
}
