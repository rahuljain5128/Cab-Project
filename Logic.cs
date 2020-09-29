using System;
using System.Collections.Generic;
using System.Linq;
namespace Business.CabLogic
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Extension method to check whether list is null or empty or not
        /// </summary>
        /// <param name="listToCheck">List</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>If list is null or empty then return true otherwise false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> listToCheck)
        {
            return listToCheck == null || !listToCheck.Any();
        }
    }

    /// <summary>
    /// Location class contains Latitude and Longitude of any location
    /// </summary>
    public class Location
    {
        public long Lat{get;set;}
        public long Lon{get;set;}    
    }

    /// <summary>
    /// Cab class contains cabId, Cab is pink or not and cab is Availble or not
    /// </summary>
    public class Cab
    {
        public int Id{get;set;}
        public bool IsPink{get;set;}
        public bool IsAvailble{get;set;}
        public Location Location{get;set;}
    };
    /// <summary>
    /// Customer class contains customerId and Name of customer
    /// </summary>
    public class Customer
    {
        public int Id{get;set;}
        public string Name{get;set;}
    };
    /// <summary>
    /// Booking detail contains bookingId, cab assigned to customer , customerId, customer source location,
    /// customer destination location, trip start time, trip end time and total amount of trip (rounded of)
    /// NOTE : customer wants to go from point a to point b and returns back to point a.
    /// We are considering point a is customer source and point b is customer destination.
    /// </summary>
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
    public static class Logic 
    {
        private static readonly List<BookingDetail> _bookingDetails = new List<BookingDetail>();
        private static readonly List<Cab> _cabs = new List<Cab>();
        private const double _pinkCabCharge = 5;
        private const double _chargePerKm = 2;
        private const double _chargePerMinute = 1;
        /// <summary>
        /// Get booking details on the basis of booking Id
        /// </summary>
        /// <param name="bookingId"></param>
        /// <returns>booking detail</returns>
        public static BookingDetail GetBookingDetail(int bookingId)
        {
            if(bookingId < 0 || bookingId >= _bookingDetails.Count)
            {
                return null;
            }
            return _bookingDetails[bookingId];
        }

        private static void SetAvailabilityOfCab(int cabId , bool isAvailble = true)
        {
            var cab = _cabs[cabId];
            cab.IsAvailble = isAvailble;          
        }
        private static void SetLocationOfCab(int cabId,Location customerLocation)
        {
            var cab = _cabs[cabId];
            cab.Location = customerLocation;
        }
        /// <summary>
        /// 1. Check for nearest available cab. if cab is not available then return -1
        /// 2. if cabId is greater than 0 then a new booking with booking id = bookingDetailsList count
        /// set cab availability false and return this new booking id
        /// (We consider 0 also valid booking id. since list index starts with 0)
        /// We can also handle this. but for simplicity we are not considering this
        /// </summary>
        /// <param name="customerSource">customer source location</param>
        /// <param name="customerDestination">customer destination location</param>
        /// <param name="customerId">customer id</param>
        /// <param name="tripStartTime">trip start time</param>
        /// <param name="isPink">pink cab is needed or not</param>
        /// <returns>booking id if cab is booked otherwise return -1</returns>
        public static int BookCab(Location customerSource,Location customerDestination, int customerId,DateTime tripStartTime, bool isPink = false)
        {
            int cabId = GetNearestAvailableCab(customerSource,isPink);
            if(cabId < 0)
            {
                return -1;
            }
            int newBookingId = _bookingDetails.Count;
            // new booking Id will be size of booking details list
            var bookingDetail = new BookingDetail
            {
                Id = newBookingId,
                CabId = cabId,
                CustomerId = customerId,
                Source = customerSource,
                Destination = customerDestination,
                TripStartTime = tripStartTime
            };
            _bookingDetails.Add(bookingDetail);
            SetAvailabilityOfCab(cabId,false);
            return newBookingId;
        }
        private static int GetNearestAvailableCab(Location customerSource,bool isPink)
        {
            if(_cabs.IsNullOrEmpty())
            {
                return -1;
            }
            var availablecabs = isPink ? _cabs.Where(x => x.IsAvailble && x.IsPink)?.ToList() : _cabs.Where(x=> x.IsAvailble)?.ToList();
            // if pink cab needed then checks Availble pink cabs else checks Availble cabs
            if(availablecabs.IsNullOrEmpty())
            {
                return -1;
            }
            int nearestCabId = availablecabs[0].Id;
            double nearestDistance = CalculateDistance(customerSource,availablecabs[0].Location);
            int size = availablecabs.Count;
            for(int i=1;i<size;i++)
            {
                double distance = CalculateDistance(customerSource, availablecabs[i].Location);
                if(nearestDistance > distance)
                {
                    nearestDistance = distance;
                    nearestCabId = availablecabs[i].Id;
                }
            }
            return nearestCabId;
        }
        private static double CalculateDistance(Location source,Location destination)
        {
            long latDistance = Math.Abs(source.Lat - destination.Lat);
            long lonDistance = Math.Abs(source.Lon - destination.Lon);
            return Math.Sqrt(lonDistance * lonDistance + latDistance*latDistance);
        }
        
        public static bool UpdateTripEndTime(int bookingId,DateTime tripEndTime)
        {
            var bookingDetail = _bookingDetails[bookingId];
            bookingDetail.TripEndTime = tripEndTime;
            return true;
        }
        /// <summary>
        /// 1. Check booking id is valid or not. If it is not valid throw invalid argument exception
        /// 2. If booking id is valid, update end time of trip
        /// 3. Set location of cab to customer source (as mentioned in the problem statement)
        /// 4. Set availability of cab to true
        /// 5 Log total amount of trip
        /// </summary>
        /// <param name="bookingId">booking id</param>
        /// <param name="tripEndTime">trip end time</param>
        /// <returns>Whether trip endtime is updated or not</returns>
        public static bool EndRide(int bookingId,DateTime tripEndTime)
        {
            if(bookingId >= _bookingDetails.Count)
            {
                throw new ArgumentException(nameof(bookingId));
            }
            var bookingDetail = _bookingDetails[bookingId];
            UpdateTripEndTime(bookingId,tripEndTime);
            SetLocationOfCab(bookingDetail.CabId, bookingDetail.Source);
            SetAvailabilityOfCab(bookingDetail.CabId);
            LogAmountOfTrip(bookingDetail);
            return true;
        }
        private static void LogAmountOfTrip(BookingDetail bookingDetail)
        {
            if(bookingDetail == null || bookingDetail.Id == 0)
            {
                return;
            }
            double amount = CalculateAmountOfTrip(bookingDetail);
            _bookingDetails[bookingDetail.Id].TotalAmount = amount;
        }
        private static double CalculateAmountOfTrip(BookingDetail bookingDetail)
        {
            if(bookingDetail == null || bookingDetail.Id == 0 || _cabs.IsNullOrEmpty())
            {
                return 0;
            }
            double totalAmount = _cabs[bookingDetail.CabId].IsPink ? _pinkCabCharge : 0;
            // since total distance will be 2 times source to destination distance. if customer wants to go from point a to point b then
            // we are assuming point `a` is source and point `b` is destination. 
            totalAmount += CalculateDistance(bookingDetail.Source,bookingDetail.Destination) * 2 * _chargePerKm;
            totalAmount += (bookingDetail.TripEndTime - bookingDetail.TripStartTime).TotalMinutes * _chargePerMinute;
            return Math.Round(totalAmount);
        }
    }
}
