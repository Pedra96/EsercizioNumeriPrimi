﻿using EsercizioNumeriPrimi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EsercizioNumeriPrimi
{
    public class NumberContext : DbContext
    {
        static string connectionString = "";
        public NumberContext() : base(GetOptions()){ }

        public DbSet<PrimeNumber> PrimeNumbers { get; set; }

        private static DbContextOptions GetOptions()
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(),connectionString).Options;
        }
    }
}
