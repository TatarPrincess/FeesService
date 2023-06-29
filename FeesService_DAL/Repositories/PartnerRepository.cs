using Dapper;
using FeesService_BLL.Models.Partner;
using FeesService_BLL.IRepositories;
using System.Data.Common;


namespace FeesService_DAL.Repositories
{
    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        public PartnerRepository(DbProviderFactory provider) : base(provider) { }
        public Partner? FindPartnerByCode(string? code)
        {
            if (code is null) return null;

            string commandText =
            @"select * 
              from[dbo].[Partner] (nolock)
              where code = @code";
            
            using (cn = CreateConnection())
            {
                cn.Open();
                List<Partner> partner = cn.Query<Partner>(commandText, new { code }).ToList();
                return partner.FirstOrDefault();
            }           
        }
    }
}
