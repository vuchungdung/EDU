using EDU.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace EDU.Services.Interfaces
{
    public interface IProductService
    {
        ProductViewModel GetById(string id, char lang);
        List<ProductViewModel> SearchOneKey(int pageIndex, int pageSize, char lang, string key, out long total);
        List<ProductViewModel> Search(int pageIndex, int pageSize, char lang, string types, out long total);
        bool Create(ProductViewModel model);
        bool Update(ProductViewModel model);
        bool Delete(string json_list_id, string updated_by);
        List<ProductViewModel> GetAll();
    }
}
