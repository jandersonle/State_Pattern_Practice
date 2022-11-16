using System;
using System.Collections.Generic;
using State_Pattern_Practice.Record_Api_Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using State_Pattern_Practice.Models;
using System.Diagnostics;

namespace State_Pattern_Practice
{
    public class ThreadedRunner
    {
        private List<Record> _records = new List<Record>();

        public void Init(int numberOfRecords)
        {
            for (var i = 1; i <= numberOfRecords; i++)
            {
                var record = new Record(i, DateTime.Now, new RecordDraftState());
                _records.Add(record);
            }

            var threads = new List<Thread>();

            // manually create records
            foreach (Record rec in _records)
            {
                Thread t = new Thread(() => ProcessRecord(rec));
                threads.Add(t);
                t.Start();
            }

            // wait for all threads to finish
            foreach (Thread t in threads)
            {
                t.Join();
            }

            // display record stats
            foreach (Record record in _records)
            {
                Console.WriteLine(record.Log);
            }
        }

        public async Task ApiInit(int numberOfRecords)
        {
            var fetchRecords = new RecordApiClient();
            fetchRecords.init();
            await fetchRecords.ProcessRecords(numberOfRecords);
            this._records = fetchRecords.GetRecords();

            var threads = new List<Thread>();


            foreach (Record rec in _records)
            {
                Thread t = new Thread(() => ProcessRecord(rec));
                threads.Add(t);
                t.Start();
            }

            foreach (Thread t in threads)
            {
                t.Join();
            }

            foreach (Record rec in _records)
            {
                Console.WriteLine(rec.Log);
            }

        }


        private void ProcessRecord(Record record)
        {
            try
            {
                if (record is not null)
                {
                    for (var i = 0; i < 2; i++)
                    {
                        record.DisplaySummary();
                        record.Update();
                    }

                    record.DisplaySummary();
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }

        }
    }
}
