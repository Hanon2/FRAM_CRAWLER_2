using Elcometer.Core.Model;
using Elcometer.Demo.Xamarin.Forms.MVVM;
using System;
using System.Linq;

namespace Elcometer.Demo.Xamarin.Forms.PageModels
{
    public class BatchInfoPageModel : BaseViewModel
    {
        private string _batchInfo;

        public Batch Batch { get; set; }

        public string BatchInfo
        {
            get { return _batchInfo; }
            set { Set(() => BatchInfo, ref _batchInfo, value); }
        }

        public override void ViewOpened()
        {
            base.ViewOpened();

            string info = "";

            info += $"Batch: {Batch.Name}\n";
            info += $"Readings: {Batch.IncludedCount}\n";
            info += "\n";

            foreach (var reading in Batch.GetReadings().Select(x => x.Value).OrderBy(x => x.Id).ToList())
            {
                if (reading.IncludeInStats)
                {
                    string readings = "";

                    for (int i = 0; i < Batch.ColumnCount; i++)
                    {
                        if (!String.IsNullOrEmpty(readings))
                        {
                            readings += ", ";
                        }

                        readings += reading.ToDisplayStringWithUnit(i);
                    }

                    info += $"{reading.Id}: {readings}\n";
                }
            }

            BatchInfo = info;
        }
    }
}