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

        public Movie Movie { get; set; }

        public int Count()
        {
            int count = 0;
            if (this.Action == true)
                count++;
            if (this.Anime == true)
                count++;
            if (this.Biographic == true)
                count++;
            if (this.Documental == true)
                count++;
            if (this.Drama == true)
                count++;
            if (this.Familly == true)
                count++;
            if (this.Fantasy == true)
                count++;
            if (this.Horror == true)
                count++;
            if (this.Comedy == true)
                count++;
            if (this.Short == true)
                count++;
            if (this.Criminal == true)
                count++;
            if (this.Melodrama == true)
                count++;
            if (this.Musical == true)
                count++;
            if (this.Music == true)
                count++;
            if (this.Adventure == true)
                count++;
            if (this.Romans == true)
                count++;
            if (this.SciFi == true)
                count++;
            if (this.Thriller == true)
                count++;
            return count;
        }

        static public string[] GetTypes()
        {
            return new[] { "Akcja", "Animowany", "Biograficzny", "Dokumentalny", "Dramat", "Familijny", "Fantasy", "Horror", "Komedia", "Krótkometrażowy", "Kryminalny", "Melodramat", "Musical", "Muzyczny", "Przygodowy", "Romans", "Sci-Fi", "Thriller" };
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