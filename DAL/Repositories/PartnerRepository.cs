using Dapper;
using FeesService.DAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FeesService.DAL.Repositories
{
    public class PartnerRepository : BaseRepository, IPartner
    {
        public PartnerRepository(DbProviderFactory provider) : base(provider) { }
        public PartnerEntity? FindPartnerByCode(string? code)
        {
            if (code is null) return null;

            string commandText =
            @"select * 
              from[dbo].[Partner] (nolock)
              where code = @code";
            
            using (cn = CreateConnection())
            {
                cn.Open();
                List<PartnerEntity> partner = cn.Query<PartnerEntity>(commandText, new { code }).ToList();
                return partner.FirstOrDefault();
            }           
        }
    }
}
