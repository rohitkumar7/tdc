using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace TDC.Controllers
{

    //Controller Class for "Next" Endpoint - GET
    //Input: Authorization key 
    //Format: Bearer API Key
    //Output: Next Incremented Number
    //Operation: this will check database if API key exists or not
    //if API key don't match then error message is sent
    //if API key matched then number with incremented value is sent
    [Route("v1/[controller]")]
    public class nextController : Controller
    {

        private readonly NumberGeneratorContext _DbContext = new NumberGeneratorContext();

        // GET 
        [HttpGet]
        public ContentResult Get([FromHeader(Name = "Authorization")]string Apicode)
        {
            if(Apicode.IndexOf(" ")>0)
            {
                var userColl = _DbContext.Users
                    .Where(b => b.APIkey == Apicode.Split(new Char[] { ' ' })[1])        
                    .ToList();

                if(userColl.Count>0)
                {
                    var objUser = userColl.First();  
                    objUser.no = objUser.no + 1;
                    _DbContext.SaveChanges();

                    return Content("{ \"id\": " + objUser.no + "}", "application/json");
                }
                else
                {
                    return Content("{ \"Error\": \"Invalid API Key\" }", "application/json");
                }
                
            }
            
            return Content("{ \"Error\": \"API Key missing\" }", "application/json");
        }
    }

    //Controller Class for "Current" Endpoint - GET and PUT
    //GET Endpoint
    //Input: Authorization key 
    //Format: Bearer API Key
    //Output: Current Number Value
    //Operation: this will check database if API key exists or not
    //if API key don't match then error message is sent
    //if API key matched then number with incremented value is sent
    //PUT Endpoint
    //Input: Authorization key and new number value
    //Format: Bearer API Key
    //Output: new Number Value
    //Operation: this will check database if API key exists or not
    //if API key don't match then error message is sent
    //if API key matched then new number is saved and sent as response
    [Route("v1/[controller]")]
    public class currentController : Controller
    {
        private readonly NumberGeneratorContext _DbContext = new NumberGeneratorContext();

        // GET api/values
        [HttpGet]
        public ContentResult Get([FromHeader(Name = "Authorization")]string Apicode)
        {
            if (Apicode.IndexOf(" ") > 0)
            {
                var userColl = _DbContext.Users
                    .Where(b => b.APIkey == Apicode.Split(new Char[] { ' ' })[1])
                    .ToList();

                if (userColl.Count > 0)
                {
                    var objUser = userColl.First();
                   
                    return Content("{ \"no\": " + objUser.no + "}", "application/json");
                }
                else
                {

                    return Content("{ \"Error\": \"Invalid API Key\" }", "application/json");
                }

            }
            //return new JsonResult(new object { "Error":"asad" });
            return Content("{ \"Error\": \"API Key missing\" }", "application/json");
        }

        // PUT api/values/5
        [HttpPut("{current}")]
        public ContentResult Put(int current, [FromHeader(Name = "Authorization")]string Apicode)
        {
            if (Apicode.IndexOf(" ") > 0)
            {      
                var userColl = _DbContext.Users
                    .Where(b => b.APIkey == Apicode.Split(new Char[] { ' ' })[1])
                    .ToList();

                if (userColl.Count > 0)
                {
                    var objUser = userColl.First();
                    objUser.no = current;
                    _DbContext.SaveChanges();

                    return Content("{ \"no\": " + objUser.no + "}", "application/json");
                }
                else
                {

                    return Content("{ \"Error\": \"Invalid API Key\" }", "application/json");
                }

            }
            //return new JsonResult(new object { "Error":"asad" });
            return Content("{ \"Error\": \"API Key missing\" }", "application/json");
        }

    }

    //Controller Class for "register" Endpoint - POST
    //Input: email, password     
    //Output: API Key
    //Operation: this will check database if email already exists or not
    //if email don't match then new user is added and new API key is sent as response
    //if email matches then error message is sent as response
    [Route("v1/[controller]")]
    public class registerController : Controller
    {
        private readonly NumberGeneratorContext _DbContext = new NumberGeneratorContext();


        // GET api/values
        [HttpPost]
        [Produces(typeof (UserPB))]
        public IActionResult Post([FromBody]UserPB value)
        {
            if (!ModelState.IsValid)
                  {
                        return BadRequest(ModelState);
                 }

            var userColl = _DbContext.Users
                   .Where(b => b.email == value.email)
                   .ToList();

            if (userColl.Count > 0)
            {


                return Content("{ \"Error\": \"User already exist\" }", "application/json");
            }
            else
            {
                User objUser = new TDC.User();
                objUser.APIkey= Guid.NewGuid().ToString();
                objUser.email = value.email;
                objUser.no= 0;
                objUser.password = value.password;
                
                _DbContext.Users.Add(objUser);
                var count = _DbContext.SaveChanges();

                return new OkObjectResult(new { objUser.APIkey });
            }
                
        }
    }

    //User Public Class used for communication with clients API
    public class UserPB
    {
        //email is required and validation is applied for format
        [Required]
        [EmailAddress]
        public string email { get; set; }

        //password is required and validation is applied for format
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string password { get; set; }
         



    }



}
