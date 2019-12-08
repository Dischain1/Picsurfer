namespace FileHub.Models
{
    public class Error
    {
        public Error(string messageText)
        {
            MessageText = messageText;
        }

        public string MessageText { get; set; }
    }
}