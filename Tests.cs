using System;
using System.Collections.Generic;
using Business.CabLogic;
using Xunit;

namespace UnitTest
{
    public  class Tests
    {
        private static readonly List<BookingDetail> _bookingDetails = new List<BookingDetail>();
        private static readonly List<Cab> _cabs = new List<Cab>();
        private static readonly BookingDetail _bookingDetail1 = new BookingDetail{Id  = 0,CabId = 1,CustomerId = 1,TripStartTime = _today,TripEndTime = _today.AddHours(1)};
        private static readonly BookingDetail _bookingDetail2 = new BookingDetail{Id  = 1,CabId = 2, CustomerId = 2,TripStartTime = _today, TripEndTime = _today.AddHours(2)};
        private static readonly Location _sourceLocation = new Location{Lat= 19.23,Lon = 72.45};
        private static readonly Location _destinationLocation = new Location{Lat = 20.23, Lon = 73.45};
        private static DateTime _today = DateTime.Now;
        private static readonly Cab _unAvailbleCab = new Cab{Id = 1,IsAvailble = false,IsPink = false};
        private static readonly Cab _availbleCab1 = new Cab{Id = 1,IsAvailble = true,IsPink = false,Location={Lat = 19.23, Lon=72.45}};
        private static readonly Cab _availbleCab2 = new Cab{Id = 0,IsAvailble = true,IsPink = false,Location={Lat = 20.23, Lon=72.68}};
        private static readonly List<Cab> _unAvailbleCabs = new List<Cab>{_unAvailbleCab};
        private static readonly Cab _unAvailblePinkCab = new Cab{Id = 1,IsAvailble = false,IsPink = true};
        private static readonly List<Cab> _unAvailblePinkCabs = new List<Cab>{_unAvailblePinkCab};      
        private static readonly List<Cab> _availbleCabs = new List<Cab>{_availbleCab1,_availbleCab2};
        /// We have assigned cabs and booking details in logic layer,
        /// We are assuming we can pass these two fields as input, so I am covering test cases
        
        public Tests()
        {
            _bookingDetail1.Source = _sourceLocation;
            _bookingDetail1.Destination = _destinationLocation;
            _bookingDetail2.Source = _sourceLocation;
            _bookingDetail2.Destination = _destinationLocation;
            _bookingDetails.Add(_bookingDetail1);
            _bookingDetails.Add(_bookingDetail2);
        }

        // Theory can be used to remove duplication of code.
        [Fact]
        public void BookCab_CabsIsNullOrEmpty_ReturnsMinusOne()
        {
            //pass cabs null or empty
            var result = Logic.BookCab(_sourceLocation,_destinationLocation,1,_today);
            Assert.Equal(-1, result);
            // request for pink cab
            var result2 = Logic.BookCab(_sourceLocation,_destinationLocation,1,_today,true);
            Assert.Equal(-1, result2);
        }

        [Fact]
        public void BookCab_CabIsNotAvailable_ReturnsMinusOne()
        {
            //pass unavailable object (_unAvailbleCabs)
            var result = Logic.BookCab(_sourceLocation,_destinationLocation,1,_today);
            Assert.Equal(-1, result);
            // request for pink cab and pass unavailable pink cabs object (_unAvailblePinkCabs)
            var result2 = Logic.BookCab(_sourceLocation,_destinationLocation,1,_today,true);
            Assert.Equal(-1, result2);
        }
        [Fact]
        public void BookCab_CabsAvailable_ReturnsbookingId()
        {
            //pass available cabs object (_availbleCabs)
            var result = Logic.BookCab(_sourceLocation,_destinationLocation,1,_today);
            Assert.Equal(0, result); // booking id will be `0`. since there is no booking till now.
        }

        [Fact]
        public void EndTrip_InvalidBookingId_ReturnsFalse()
        {
            // pass booking details list also (null)
            var result = Logic.EndRide(-1,_today);
            Assert.False(result);
            // pass booking details list object null or empty (new List<BookingDetail>())
            var result2 = Logic.EndRide(0,_today);
            Assert.False(result2);
        }

        [Fact]
        public void EndTrip_ValidBookingId_ReturnsTrue()
        {
            // pass booking details list object also(_bookingDetails)
            var result = Logic.EndRide(1,_today);
            // for bookingId 1, there is id in bookingdetails. so it will assing end trip for this booking id.
            Assert.True(result);
        }

        [Fact]
        public void GetBookingDetail_InvalidBookingId_ReturnsNull()
        {
            // pass booking details list also (null)
            var result = Logic.GetBookingDetail(-1);
            Assert.Null(result);
            // pass booking details list object null or empty (new List<BookingDetail())
            var result2 = Logic.GetBookingDetail(0);
            Assert.Null(result2);
        }
        
        [Fact]
        public void GetBookingDetail_ValidBookingId_ReturnsBookingDetail()
        {
            // pass booking details list object also (_bookingDetails)
            var result = Logic.GetBookingDetail(1);
            // for booking id 1, it will return _bookingDetail2. so compare these two object
            Assert.Equal(result.Id,_bookingDetail2.Id);
        }
    }
}
