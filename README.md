# Cab-Project
## Classes for this cab design project


    public class Location
    {
        public long Lat{get;set;}
        public long Lon{get;set;}    
    }
 
    public class Cab
    {
        public int Id{get;set;}
        public bool IsPink{get;set;}
        public bool IsAvailble{get;set;}
        public Location Location{get;set;}
    };
    
    public class Customer
    {
        public int Id{get;set;}
        public string Name{get;set;}
    };
    
    public class BookingDetail
    {
        public int Id{get;set;}
        public int CabId{get;set;}
        public int CustomerId{get;set;}
        public Location Source{get;set;}
        public Location Destination{get;set;}
        public DateTime TripStartTime {get ;set;}
        public DateTime TripEndTime {get ;set;}
        public double TotalAmount {get;set;}
    };
    
### Book a cab steps.
1. 
