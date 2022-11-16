using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Pattern_Practice.Models
{
    public class IRecord
    {
        public int id { get; }
        public DateTime recordDate { get; }
        public bool recordPublished { get; }        
        public bool recordReviewed { get; }

        public DateTime record_date { get; }

        public IRecord(int record_id, DateTime record_date, bool record_published, bool record_reviewed)
        {
            this.id= record_id;
            this.recordDate= record_date;
            this.recordPublished= record_published;
            this.recordReviewed= record_reviewed;
        }


    }
}
