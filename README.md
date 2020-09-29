# Cab-Project
## Classes for this cab design project


    public class Location
    {
        public double Lat{get;set;}
        public double Lon{get;set;}    
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
    
### Assumption.
We are assuming cabs list will contain cab with id starting 0.
same for booking details list.

## Api for Booking a cab (api/bookcab/)
1. Check for validation of input
2. If input is not valid then return BadRequest()
3. If input is valid then checks for cabs is null or empty
4. If cabs is null or empty return -1 (booking can not be generated)
5. If cabs is not null nor empty then getNearestAvailble cab for given customer location and pink cab if customer required it
6. If cab is not avaible then return -1 (booking can not be generated)
7. If cab is avaible then create a new booking with customer info and cabId, set isAvaible of this assigned cab to false and return this booking id


## End Trip of a booking (api/endtrip/)
1. Check for validation of input
2. If input is not valid then return BadRequest()
3. Check booking id is avaible in bookingDetails list
4. If it is avaible then Update trip endtime with input trip endtime
5. Set location of cab to customer source (since customer will return to their source)
6. Set availability of assigned cab to true
7. Log total amount of trip and return true

## Get Amount of trip for a particular booking (api/tripamount/)
1. Check for validation of input
2. If input is not valid then return false
3. If input is valid then get booking details on the basis of booking id
4. return total trip amount (Roundof(total trip amount))
