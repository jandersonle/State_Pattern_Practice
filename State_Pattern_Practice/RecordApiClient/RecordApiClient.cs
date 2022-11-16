using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net;
using System.Text.Json;
using State_Pattern_Practice.Models;
using System.Data.Common;
using System.Net.Http.Json;

namespace State_Pattern_Practice.Record_Api_Client
{
    public class RecordApiClient
    {
        private HttpClient? _client = new HttpClient();

        private string baseURL = "https://localhost:7193/api/RecordItemsController/";

        private List<Record> _records = new List<Record>();

        /// <summary>
        /// Sets the default parameter for the Http web client. Must be ran before any other methods.
        /// </summary>
        public void init()
        {
            System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            this._client.DefaultRequestHeaders.Accept.Clear();
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            this._client.DefaultRequestHeaders.Add("FetchAPI", "local dev");
        }


        /// <summary>
        /// Attempts to create and upload a record through the API, sent as a seralized to json IRecord object.
        /// </summary>
        public async Task CreateRecord(int recordId, bool recordPublished, bool recordReviewed, DateTime recordDate)
        {
            try
            {
                var connStr = baseURL + recordId + "/";
                var record = new IRecord(recordId, recordDate, recordPublished, recordReviewed);
                var json = JsonSerializer.Serialize(record);
                var response = await this._client.PostAsJsonAsync(connStr, json);
            }
            catch(Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }

            return;
        }

        /// <summary>
        /// Attempts to update the record with the provided ID through the API using a HTTP PUT method. 
        /// Sent as a seralized to json IRecord object.
        /// </summary>
        public async Task UpdateRecord(int recordId, bool recordPublished, bool recordReviewed, DateTime recordDate)
        {
            try
            {
                var connStr = baseURL + recordId + "/";
                var record = new IRecord(recordId, recordDate, recordPublished, recordReviewed);
                var json = JsonSerializer.Serialize(record);
                var response = await this._client.PutAsJsonAsync(connStr, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return;
            
        }

        /// <summary>
        /// Attempts to delete the record with the provided ID through the API using a HTTP Delete method. 
        /// </summary>
        public async Task DeleteRecord(int recordId)
        {
            try
            {
                var connStr = baseURL + recordId + "/";
                var response = await this._client.DeleteAsync(connStr);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
           
            return;
        }

        /// <summary>
        /// Attempts to retrieve the record with the provided ID through the API using a HTTP GET method. 
        /// Converts the response json into a Record object and stores the record in the internal _records variable
        /// </summary>
        public async Task GetRecord(int recordId)
        {
            this._records.Clear(); 

            try
            {
                var connStr = baseURL + recordId + "/";
                var json = await this._client.GetStringAsync(connStr);

                var record = Newtonsoft.Json.JsonConvert.DeserializeObject<Record>(json);
                if (record != null)
                {
                    _records.Add(record);
                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        /// <summary>
        /// Attempts to retrieve the provided number of records through the API using a HTTP GET method. 
        /// Converts the response json into a Record object and stores all records in the internal _records variable
        /// </summary>
        private async Task ProcessRecordsAsync(int numberOfRecords)
        {
            this._records.Clear();

            try
            {
                for (var id = 1; id <= numberOfRecords; id++)
                {
                    var connStr = baseURL + id + "/";
                    var json = await this._client.GetStringAsync(connStr);
                    
                    var record = Newtonsoft.Json.JsonConvert.DeserializeObject<Record>(json);
                    if (record != null)
                    {
                        _records.Add(record);
                    }
                }

            } 

            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            } 
        }


        /// <summary>
        /// Wrapper method for ProcessRecordsAsync, call this method.
        /// </summary>
        public async Task ProcessRecords(int numRecords)
        {
           await this.ProcessRecordsAsync(numRecords);
        }


        /// <summary>
        /// Returnd the classes internal result list of records.
        /// </summary>
        public List<Record> GetRecords()
        {
            return _records;
        }
    }
}
