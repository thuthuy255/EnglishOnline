namespace API.Model
{
    public class ResponseListTopic
    {
        public string IdTopic { get; set; }
        public string NameTopic { get; set; }
        public List<LessonItem> ListLessons { get; set; }
        public string Status { get; set; }
    }

    public class LessonItem
    {
        public string IdLesson { get; set; }
        public string Title { get; set; }
        //public string Description { get; set; }
        //public string Status { get; set; }
    }
}
