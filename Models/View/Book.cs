using System;

namespace basicWebApi.Models.View
{
    public class BookView //satu kelas untuk satu fungsi meskipun isinya sama
    //yang ini untuk bikin view, yang di param untuk nampung parameter
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}