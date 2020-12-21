using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyMaterialsSharingSystem.Models.HomeViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public IEnumerable<House> Houses { get; set; }
        public IEnumerable<Software> Softwares { get; set; }
        public IEnumerable<Document> Documents { get; set; }
    }
}
