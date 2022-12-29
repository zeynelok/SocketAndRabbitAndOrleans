using MongoDB.Bson.Serialization.Attributes;

namespace RabbitMqConsumerService
{
    [BsonIgnoreExtraElements]
    public class AnchorMongo
    {
        public Guid RawID { get; set; }
        public string ModelName { get; set; }
        public string AnchorKey { get; set; }
        public string ShortCode { get; set; }
        public Guid? CompanyID { get; set; }
        public Guid? FacilityID { get; set; }
        public Guid? CommBoxID { get; set; }
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        public double LocationZ { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string LocalIP { get; set; }
        public string MACAddress { get; set; }
        public string RefAnchors { get; set; }
        public double RelativeHeight { get; set; }
        public double FixedDistanceCorrection { get; set; }
        public double DynamicDistanceFactor { get; set; }
        public byte PathBoundaryStatus { get; set; }
        public byte LocationTypeID { get; set; }
        public byte WorkMode { get; set; }
        public string PrivatePathUrl { get; set; }
        public double RotationXDegree { get; set; }
        public double RotationYDegree { get; set; }
        public double RotationZDegree { get; set; }
        public string WifiSSID { get; set; }
        public string WifiPassword { get; set; }
        public int PingPeriod { get; set; }
        public int BlinkPeriod { get; set; }
        public string Description { get; set; }
        public string RemovalReason { get; set; }
        public byte State { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? Creator { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? Updater { get; set; }


        // INFO: Mongo specific public properties
        //public Company Company { get; set; }
        //public Facility Facility { get; set; }
        //public CommBox CommBox { get; set; }
        public string SoftwareVersion { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastDistanceSentDate { get; set; }
        public string LastTagMeasured { get; set; }
        public DateTime? LastDataSentDate { get; set; }
        //public Location Location => new Location(LocationX, LocationY, LocationZ);
        public int? Temperature { get; set; }
        public int? Humidity { get; set; }
        public string CurrentAlertType { get; set; }



        //[BsonIgnore]
        //public Location ClosestEntranceAnchorLocation { get; set; }
        //[BsonIgnore]
        //public double ClosestEntranceAnchorDistance { get; set; }
        //[BsonIgnore]
        //public Location ClosestLocationOnPointCloudCurves { get; set; }
        //[BsonIgnore]
        public string CurrentSessionID { get; set; }
    }
}
