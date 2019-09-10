using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciChart.Examples.Demo.Common
{
    public class UsageData
    {
        public string UserId { get; set; }

        /// <summary>Usage dictionary keyed on ExampleId</summary>
        public List<ExampleUsage> Usage { get; set; }
    }

    public class ExampleRating
    {
        public string ExampleID { get; set; }

        /// <summary>Score based on usage stats like vists and time spent</summary>
        public float Popularity { get; set; }

        /// <summary>Score based on user feedback</summary>
        public float Rating { get; set; }
    }

    public enum ExampleFeedbackType
    {
        Frown,
        Smile,
    }

    public class ExampleUsage
    {
        private int _visitCount;
        private int _secondsSpent;
        private int _interactions;
        private bool _viewedSource;
        private bool _exported;
        private ExampleFeedbackType? _feedbackType;
        private string _email;
        private string _feedbackText;

        public string ExampleID { get; set; }

        /// <summary>Total visits to the example</summary>
        public int VisitCount
        {
            get
            {
                return this._visitCount;
            }
            set
            {
                this._visitCount = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>Total time spent looking at the example</summary>
        public int SecondsSpent
        {
            get
            {
                return this._secondsSpent;
            }
            set
            {
                this._secondsSpent = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Number of interactions with functional parts the example
        /// </summary>
        public int Interactions
        {
            get
            {
                return this._interactions;
            }
            set
            {
                this._interactions = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// True if the user ever viewed the source of the example
        /// </summary>
        public bool ViewedSource
        {
            get
            {
                return this._viewedSource;
            }
            set
            {
                this._viewedSource = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>True if the user ever exported the example</summary>
        public bool Exported
        {
            get
            {
                return this._exported;
            }
            set
            {
                this._exported = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>User rating</summary>
        public ExampleFeedbackType? FeedbackType
        {
            get
            {
                return this._feedbackType;
            }
            set
            {
                this._feedbackType = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// Users email, if they would like to be contacted regarding their feedback
        /// </summary>
        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this._email = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        /// <summary>User comments</summary>
        public string FeedbackText
        {
            get
            {
                return this._feedbackText;
            }
            set
            {
                this._feedbackText = value;
                this.LastUpdated = DateTime.UtcNow;
            }
        }

        public DateTime LastUpdated { get; set; }
    }
}
