﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TDC
{
    public class NumberGeneratorContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        //Database connection settings
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./NumberGenerator.db");
        }
    }

    

    //User Model Class used to interact with database
    public class User
    {
        //uid is Primary Key which is autogenerated in database
        [Key]
        public int uid { get; set; }

        //email is required and validation is applied for format
        [Required]
        [EmailAddress]
        public string email { get; set; }

        //password is required and validation is applied for format
        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string  password { get; set; }

        //api key used to communicate with API
        public string APIkey { get; set; }

        //no is returned and incremented
        public Int32 no { get; set; }

       

    }

   



}