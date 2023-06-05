using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Web.Models.Order
{
    public class CheckOutInfoInput
    {
        [Display(Name ="İl")]
        public string Province { get; set; }
        [Display(Name = "İlçe")]
        public string Distrinct { get; set; }
        [Display(Name = "Sokak")]
        public string Street { get; set; }
        [Display(Name = "Posta Kodu")]
        public string ZipCode { get; set; }
        [Display(Name = "Adres")]
        public string Line { get; set; }
        [Display(Name = "İsim ve Soy İsim")]
        public string CardName { get; set; }
        [Display(Name = "Kart Numarası")]
        public string CardNumber { get; set; }
        [Display(Name = "Son Kullanma Tarihi(Ay/Yıl)")]
        public string Expiration { get; set; }
        [Display(Name = "CVV/CVC2 Numarası")]
        public string CVV { get; set; }

    }
}
