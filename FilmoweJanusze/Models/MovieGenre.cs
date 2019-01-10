using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FilmoweJanusze.Models
{
    public class MovieGenre
    {

        [Key]
        [ForeignKey("Movie")]
        public int MovieID { get; set; }

        [Display(Name = "Akcja")]
        public bool Action { get; set; }
        [Display(Name = "Animowany")]
        public bool Anime { get; set; }
        [Display(Name = "Biograficzny")]
        public bool Biographic { get; set; }
        [Display(Name = "Dokumentalny")]
        public bool Documental { get; set; }
        [Display(Name = "Dramat")]
        public bool Drama { get; set; }
        [Display(Name = "Familijny")]
        public bool Familly { get; set; }
        [Display(Name = "Fantasy")]
        public bool Fantasy { get; set; }
        [Display(Name = "Horror")]
        public bool Horror { get; set; }
        [Display(Name = "Komedia")]
        public bool Comedy { get; set; }
        [Display(Name = "Krótkometrażowy")]
        public bool Short { get; set; }
        [Display(Name = "Kryminalny")]
        public bool Criminal { get; set; }
        [Display(Name = "Melodramat")]
        public bool Melodrama { get; set; }
        [Display(Name = "Musical")]
        public bool Musical { get; set; }
        [Display(Name = "Muzyczny")]
        public bool Music { get; set; }
        [Display(Name = "Przygodowy")]
        public bool Adventure { get; set; }
        [Display(Name = "Romans")]
        public bool Romans { get; set; }
        [Display(Name = "Sci-Fi")]
        public bool SciFi { get; set; }
        [Display(Name = "Thriller")]
        public bool Thriller { get; set; }

        public virtual Movie Movie { get; set; }

        public int Count()
        {
            int count = 0;
            if (Action == true)
                count++;
            if (Anime == true)
                count++;
            if (Biographic == true)
                count++;
            if (Documental == true)
                count++;
            if (Drama == true)
                count++;
            if (Familly == true)
                count++;
            if (Horror == true)
                count++;
            if (Comedy == true)
                count++;
            if (Short == true)
                count++;
            if (Criminal == true)
                count++;
            if (Melodrama == true)
                count++;
            if (Musical == true)
                count++;
            if (Music == true)
                count++;
            if (Adventure == true)
                count++;
            if (Romans == true)
                count++;
            if (SciFi == true)
                count++;
            if (Thriller == true)
                count++;
            return count;
        }
    }

    
}
/*
public enum GenreList
{
    [Display(Name = "Akcja")]
    Action,
    [Display(Name = "Animowany")]
    Anime,
    [Display(Name = "Biograficzny")]
    Biographic,
    [Display(Name = "Dokumentalny")]
    Documental,
    [Display(Name = "Dramat")]
    Drama,
    [Display(Name = "Familijny")]
    Familly,
    [Display(Name = "Fantasy")]
    Fantasy,
    [Display(Name = "Horror")]
    Horror,
    [Display(Name = "Komedia")]
    Comedy,
    [Display(Name = "Krótkometrażowy")]
    Short,
    [Display(Name = "Kryminalny")]
    Criminal,
    [Display(Name = "Melodramat")]
    Melodrama,
    [Display(Name = "Musical")]
    Musical,
    [Display(Name = "Muzyczny")]
    Music,
    [Display(Name = "Przygodowy")]
    Adventure,
    [Display(Name = "Romans")]
    Romans,
    [Display(Name = "Sci-Fi")]
    SciFi,
    [Display(Name = "Thriller")]
    Thriller,
}*/