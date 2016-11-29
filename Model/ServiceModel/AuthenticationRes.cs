using System;
namespace Model.ServiceModel
{
    public class AuthenticationRes : IResult
    {
        public Result result { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string token { get; set; }
        public DateTime expiryTime { get; set; }
        public string DBVersion { get; set; }
    }
}