namespace SSEProject.Models
{
    public class ResponseView
    {
        public int? order {  get; set; }
        public int time { get; set; }

        public ResponseView(int? order, int time)
        {
            this.order = order;
            this.time = time;
        }

    }
}
