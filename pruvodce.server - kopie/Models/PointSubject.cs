namespace pruvodce.server.Models
{
    public class PointSubject
    {
        public string PointId { get; set; } = string.Empty;
        public Point Point { get; set; } = null!;

        public string SubjectId { get; set; } = string.Empty;
        public Subject Subject { get; set; } = null!;
    }
}