namespace travoul.Models.ViewModels
{
    public class EditFinishedTripViewModel : EditPlannedTripViewModel
    {
        public TripRetro DoAgainRetro { get; set; }

        public TripRetro DoDifferentRetro { get; set; }
    }
}
