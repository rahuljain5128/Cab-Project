using System;
using Business.CabLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers
{
    public class CabController : Controller
    {
        private static bool IsValidLocation(Location location)
        {
            if(location == null)
            {
                return false;
            }
            if(location.Lat < -90 || location.Lat > -90)
            {
                return false;
            }
            if(location.Lon < -180 || location.Lon > -180)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        [Route("api/bookcab/")]
        public IActionResult BookCab(Location customerSource,Location customerDestination, int customerId,DateTime tripStartTime, bool isPink = false)
        {
            try
            {
                if(customerId <= 0 || !IsValidLocation(customerSource) || !IsValidLocation(customerDestination) || tripStartTime == DateTime.MinValue)
                {
                    return BadRequest();
                }
                return Ok(Logic.BookCab(customerSource,customerDestination,customerId,tripStartTime,isPink));
            }
            catch(Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/endtrip/")]
        public IActionResult EndRide(int bookingId,DateTime tripEndTime)
        {
            try 
            {
                if(bookingId < 0 || tripEndTime == DateTime.MinValue)
                {
                    return BadRequest();
                }
                return Ok(Logic.EndRide(bookingId,tripEndTime));
            }
            catch(Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/tripamount/")]
        public IActionResult GetBookingDetail(int bookingId)
        {
            try
            {
                if(bookingId < 0)
                {
                    return BadRequest();
                }
                var bookingDetail = Logic.GetBookingDetail(bookingId);
                return Ok(bookingDetail?.TotalAmount);
            }
            catch(Exception)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
