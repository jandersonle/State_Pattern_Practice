using State_Pattern_Practice.Models;
using State_Pattern_Practice.Record_Api_Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Pattern_Practice
{
    public class Runner
    {
        private Record? _record = null;

        public void Initilaize(int numRecords)
        {
            for (var i = 1; i <= numRecords; i++)
            {
                Console.WriteLine("starting for record " + i);
                var record = new Record(i, DateTime.Now, new RecordDraftState());
                this._record = record;
                this.ProcessRecord();
                Console.WriteLine(_record.Log);
            }
        }

        public async Task ApiInitilaize(int numRecords)
        {
            var apiClient = new RecordApiClient();
            apiClient.init();
            await apiClient.ProcessRecords(numRecords);
            var records = apiClient.GetRecords();

            foreach (Record rec in records)
            {
                this._record = rec;
                this.ProcessRecord();
                Console.WriteLine(_record.Log);
            }
        }

        public void ProcessRecord()
        {
            try
            {
                if (this._record is not null)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        _record.DisplaySummary();
                        _record.Update();
                    }
                    _record.DisplaySummary();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }
        }
    }
}
