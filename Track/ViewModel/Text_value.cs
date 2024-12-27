namespace Track.ViewModel
{
    public class Text_value
    {
        public int? Id { get; set; }
        public string? Value { get; set; }
        public Text_value()
        {
            
        }
        public Text_value(int id, string value)
        {
            this.Id = id;
            this.Value = value;
        }
    }
}
