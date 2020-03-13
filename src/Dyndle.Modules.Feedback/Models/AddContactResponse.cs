using System.Xml.Serialization;

namespace Dyndle.Modules.Feedback.Models
{
    public class AddContactResponse
    {
        [XmlElement("return")]
        public int ReturnCode { get; set; } = -1;   // no success

        [XmlElement("message")]
        public string Message { get; set; } = string.Empty;

        public bool Success => (ReturnCode == 1);
    }
}