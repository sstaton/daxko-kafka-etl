using System;
using System.Collections.Generic;
using System.Text;

namespace Confluent.Kafka.Examples.SimpleConsumer
{
    public class LocationCheckinData
    {
        public Guid Id { get; set; }
        public MemberData Member { get; set; }
        public LocationData Location { get; set; }
        public DateTime CheckinCompleted { get; set; }

        //public LocationCheckinData(Guid id, Guid memberId, Guid locationId, DateTime checkinCompleted
        //    , string firstName, string lastName, string locationName)
        //{
        //    MemberData mem = new MemberData();
        //    mem.Id = memberId;
        //    mem.FirstName = firstName;
        //    mem.LastName = lastName;

        //    LocationData loc = new LocationData();
        //    loc.Id = locationId;
        //    loc.LocationName = locationName;

        //    Id = id;
        //    Member = mem;
        //    Location = loc;
        //}

        public class MemberData
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class LocationData
        {
            public Guid Id { get; set; }
            public string LocationName { get; set; }
        }
    }

}
