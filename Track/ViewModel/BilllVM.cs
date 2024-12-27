namespace Track.ViewModel
{
    public class BilllVM
    {
        public double total { get; set; }
        public double received {  get; set; }
        
        public double after_TDS { get; set; }   
        public double VAT { get; set; }
        public double received_VAT { get; set; }
        public double received_VAT_per { get; set;}
        public double government_VAT_per { get; set;}

    }
}
