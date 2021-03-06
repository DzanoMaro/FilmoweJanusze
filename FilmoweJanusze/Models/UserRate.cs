﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class UserRate
    {
        public int UserRateID { get; set; }

        public int? MovieID { get; set; }
        public int? PeopleID { get; set; }
        public string UserID { get; set; }

        [Display(Name = "Twoja ocena")]
        [Range(1, 6)]
        public int Rate { get; set; }

        [StringLength(99, ErrorMessage = "Komentarz może zawierać maksymalnie 99 znaków")]
        [DataType(DataType.MultilineText)]
        [DisplayFormat(NullDisplayText = "Brak komentarza")]
        [Display(Name = "Twój komentarz")]
        public string Comment { get; set; }
        public Movie Movie { get; set; }
        public People People { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser User { get; set; }

        public static int[] GetRateRange()
        {
            int[] raterange = new int[RateMaxRange];
            for (int i = 0; i < RateMaxRange; i++)
                raterange[i] = i + 1;

            return raterange;
        }

        private static int RateMaxRange
        {
            get
            {
                return 6;
            }
        }

    }
}