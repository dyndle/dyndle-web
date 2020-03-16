using System.Xml.Serialization;

namespace Dyndle.Modules.Feedback.Models
{
    public class ResultDetails
    {
        [XmlElement("return")]
        public int ReturnCode { get; set; } = -1;   // no success

        [XmlElement("message")]
        public string Message { get; set; }

        public bool Success => (ReturnCode == 1);
    }
}