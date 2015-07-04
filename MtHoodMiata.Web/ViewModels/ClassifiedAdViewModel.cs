#region using
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using MtHoodMiata.Web.Models;
#endregion using

namespace MtHoodMiata.Web.ViewModels
{
    public class ClassifiedAdViewModel
    {
        public IEnumerable<ClassifiedAdType> AdTypes { get; set; }

        public int ClassifiedAdTypeCode { get; set; }

        [Required( ErrorMessage = "The Ad Text field cannot be blank." )]
        [AllowHtml]
        public string AdText { get; set; }
    }
}