using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace State_Pattern_Practice.Models
{
    public class Record
    {
        public RecordState? _state = null;

        public int record_id { get; set; }

        public bool record_published { get; set; }

        public bool record_reviewd { get; set; }

        protected DateTime record_date { get; set; }

        public string Log { get; set; }


        [JsonConstructor]
        public Record(int id, DateTime recordDate, bool recordPublished, bool recordReviewed)
        {
            record_id = id;
            record_date = recordDate;
            record_published = recordPublished;
            record_reviewd = recordReviewed;
            _state = new RecordDraftState();
            _state.SetRecord(this);
        }


        public Record(int id, DateTime publish_date, RecordState state)
        {
            this.record_id = id;
            this.record_date = publish_date;
            this.record_published = false;
            this.record_reviewd = false;
            this._state = state;
            this._state.SetRecord(this);
        }

        public void TransitionTo(RecordState state)
        {
            this._state = state;
            this._state.SetRecord(this);
        }

        public void Update()
        {
            this._state.SendRecord();
        }

        public void DisplaySummary()
        {

            this.Log += "---------------------------------------\n";
            this.Log += "Summary for record : " + this.record_id + "\n";
            this.Log += "Record Date        : " + this.record_date + "\n";
            this.Log += "Review Status      : " + this.record_reviewd + "\n";
            this.Log += "Publish Status     : " + this.record_published + "\n";

        }
    }

    public abstract class RecordState
    {
        protected Record? _record;

        public void SetRecord(Record record)
        {
            this._record = record;
        }

        public abstract void SendRecord();

        public void AwaitFor(int x)
        {
            Thread.Sleep(x);
        }

    }

    class RecordDraftState : RecordState
    {

        public RecordDraftState()
        {
        }

        public RecordDraftState(Record record)
        {
            this._record = record;
            this._record.record_reviewd = false;
            this._record.record_published = false;

        }


        public override void SendRecord()
        {
            this.AwaitFor(2000);
            this._record.TransitionTo(new RecordReviewState(this._record));
        }
    }

    class RecordReviewState : RecordState
    {

        public RecordReviewState(Record record)
        {
            this._record = record;
            this._record.record_reviewd = true;

        }


        public override void SendRecord()
        {
            this.AwaitFor(2000);
            this._record.TransitionTo(new RecordPublishState(this._record));

        }
    }

    class RecordPublishState : RecordState
    {
        public RecordPublishState(Record record)
        {
            this._record = record;
            this._record.record_published = true;
            this._record.record_reviewd = false;
        }

        public override void SendRecord()
        {
            this.AwaitFor(2000);
            this._record.TransitionTo(new RecordDraftState(this._record));
        }

    }
}
