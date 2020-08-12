
namespace GridBeyondTest.Models
{
    /// <summary>
    /// Used by the error view
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
