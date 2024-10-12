using Elcometer.Core.Linux;
using Elcometer.Core.Linux.Services;
using Elcometer.Core.Services;
using System;
using System.Linq;
using System.Threading;

namespace Elcometer.Demo.Linux
{
    internal class Program
    {
		private const string DefaultGaugeDescripion = "Elcometer-456/4";
		private const string DefaultDevice = "/dev/tty5";

		private static void Main(string[] args)
        {
            ElcometerCore.Instance.Initialise();

			string deviceDescription = DefaultGaugeDescripion;
			string devicePath = DefaultDevice;

			if (args.Length == 2)
			{
				deviceDescription = args[0];
				devicePath = args[1];
			}
			else
			{
				Console.WriteLine("Usage:");
				Console.WriteLine("Elcometer.Demo.Linux (description) (path)");
				Console.WriteLine("e.g.:");
				Console.WriteLine("Elcometer.Demo.Linux Elcometer-456/4 /dev/ttyS0");
				Console.WriteLine("");
				Console.WriteLine("Using default device and path.");
			}
            
			Console.WriteLine($"Using device {deviceDescription} located {devicePath}");

			var device = new USBDeviceInfo(deviceDescription, devicePath);
            
            IGaugeType gaugeType;
            string serialNumber;
            
            // get the gauge type so we can print out the nice device name
            if (ElcometerCore.Instance.GaugeTypeService.TryIdentifyGauge(device, out gaugeType, out serialNumber))
            {
                Console.WriteLine($"Connecting to {gaugeType.Description} {serialNumber}...");

                // initialise the connection
                var gauge = ElcometerCore.Instance.GaugeService.Connect(device);      

                // wait until batch download is available
                UInt32 start = (UInt32)Environment.TickCount;
                while ((UInt32)Environment.TickCount - start < 10000)
                {
                    // some gauges although connected can take a few seconds to identify if they support batches
                    // (basically the 456C/224C/311C/415C gauges)
                    if (gauge.SupportsBatching)
                    {
                        break;
                    }

                    Thread.Sleep(10);
                }

                // did we timeout or actually connect ok
                if (gauge.SupportsBatching)
                {
                    Console.WriteLine("Enumerating Batches...");

                    // start the batch download process
                    gauge.StartBatching();

                    // enumerate all the batches
                    var gaugeBatches = gauge.GetBatches();

                    Console.WriteLine($"Found {gaugeBatches.Count} batches");

                    // go through the batch list and request all the batch info
                    foreach (var gaugeBatch in gaugeBatches)
                    {
                        // clear temporary store
                        ElcometerCore.Instance.BatchService.Batches.Clear();

                        // download batches to the temporary store
                        gauge.DownloadBatchesTo(new[] { gaugeBatch }, ElcometerCore.Instance.BatchService);

                        if (ElcometerCore.Instance.BatchService.Batches.Count > 0)
                        {
                            var batch = ElcometerCore.Instance.BatchService.Batches[0];

                            // show the batch info
                            Console.WriteLine($"Batch: {batch.Name}");
                            Console.WriteLine($"Readings: {batch.IncludedCount}");

                            foreach (var reading in batch.GetReadings().Select(x => x.Value).OrderBy(x => x.Id).ToList())
                            {
                                if (reading.IncludeInStats)
                                {
                                    string readings = "";

                                    for (int i = 0; i < batch.ColumnCount; i++)
                                    {
                                        if (!String.IsNullOrEmpty(readings))
                                        {
                                            readings += ", ";
                                        }

                                        readings += reading.ToDisplayStringWithUnit(i);
                                    }

                                    Console.WriteLine($"{reading.Id}: {readings}");
                                }
                            }
                        }
                    }

                    gauge.EndBatching();
                    Console.WriteLine();
                }

                // disconnect from the gauge
                ElcometerCore.Instance.GaugeService.Disconnect(gauge);
            }
            

            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}