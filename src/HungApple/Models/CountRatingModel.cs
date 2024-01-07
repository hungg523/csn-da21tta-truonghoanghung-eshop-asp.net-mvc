namespace HungApple.Models
{
    public class CountRatingModel
    {
        public int TotalRating { get; set; }
        public int TotalFive { get; set; }  
        public int TotalFour { get; set; }
        public int TotalThree { get; set; }
        public int TotalTwo { get; set; }   
        public int TotalOne { get; set; }
        public double PercentOne { get; set; }
        public double PercentTwo { get; set; }
        public double PercentThree { get; set; }
        public double PercentFour { get; set; }
        public double PercentFive { get; set; }
    }
}
