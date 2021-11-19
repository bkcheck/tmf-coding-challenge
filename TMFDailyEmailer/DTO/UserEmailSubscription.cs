namespace TMFDailyEmailer.DTO
{
    public class UserEmailSubscription
    {
        public int UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string WatchedInstrumentIds { get; set; }
    }
}
