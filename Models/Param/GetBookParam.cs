using System;

namespace basicWebApi.Models.Param
{
    public class GetBookParam
    {
        //public/private tipe-data namaParam/properti
        public string Title { get; set; } //case-sensitive //get set bisa diambil dan diubah oleh class lainnya
        public DateTime Date { get; set; }
    }
}