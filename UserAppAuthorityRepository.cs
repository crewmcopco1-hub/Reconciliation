using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDSReconciliation.Infrastructure;

namespace VDSReconciliation.Repositories.SQL
{
    public class UserAppAuthorityRepository : IUserAppAuthorityRepository
    {
        private readonly IConfiguration _config;

        public UserAppAuthorityRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<List<UserAppAuthority>> GetByEidAndGlinAsync(string eid, string glin, string Application_ID)
        {
            var list = new List<UserAppAuthority>();
            using var conn = new SqlConnection(_config["SqlConnection"]);
            using var cmd = new SqlCommand(
                @"select *from userAppAuthority 
                where user_id=@user_id and location_id=@location_id and application_id=@application_id", conn);

            cmd.Parameters.AddWithValue("@user_id", eid);
            cmd.Parameters.AddWithValue("@location_id", glin);
            cmd.Parameters.AddWithValue("@application_id", Application_ID);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UserAppAuthority
                {
                    UserAppAuthId = reader.GetDecimal(reader.GetOrdinal("user_app_auth_id")),
                    access_org_id = reader.GetString(reader.GetOrdinal("access_org_id")),
                    user_id = reader.GetString(reader.GetOrdinal("user_id")),
                    location_id = reader.GetString(reader.GetOrdinal("location_id")),
                    application_id = reader.GetString(reader.GetOrdinal("application_id")),

                    application_role_id = reader.IsDBNull(reader.GetOrdinal("application_role_id"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("application_role_id")),

                    crnt_owsh_typ_ds = reader.GetString(reader.GetOrdinal("crnt_owsh_typ_ds")),
                    default_authoritative_role = reader.GetString(reader.GetOrdinal("default_authoritative_role")),
                    record_source = reader.GetString(reader.GetOrdinal("record_source")),

                    char_attrib_1 = reader.IsDBNull(reader.GetOrdinal("char_attrib_1")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_1")),
                    char_attrib_2 = reader.IsDBNull(reader.GetOrdinal("char_attrib_2")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_2")),
                    char_attrib_3 = reader.IsDBNull(reader.GetOrdinal("char_attrib_3")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_3")),
                    char_attrib_4 = reader.IsDBNull(reader.GetOrdinal("char_attrib_4")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_4")),
                    char_attrib_5 = reader.IsDBNull(reader.GetOrdinal("char_attrib_5")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_5")),

                    role_prov_by_user_id = reader.GetString(reader.GetOrdinal("role_prov_by_user_id")),
                    created_in_trans_batch_id = reader.GetDecimal(reader.GetOrdinal("created_in_trans_batch_id")),

                    ack_in_eai_batch_id = reader.IsDBNull(reader.GetOrdinal("ack_in_eai_batch_id"))
                        ? (decimal?)null
                        : reader.GetDecimal(reader.GetOrdinal("ack_in_eai_batch_id")),

                    created_by_user_id = reader.GetString(reader.GetOrdinal("created_by_user_id")),
                    creation_date = reader.GetDateTime(reader.GetOrdinal("creation_date")),

                    updated_in_trans_batch_id = reader.IsDBNull(reader.GetOrdinal("updated_in_trans_batch_id"))
                        ? (decimal?)null
                        : reader.GetDecimal(reader.GetOrdinal("updated_in_trans_batch_id")),

                    updated_by_user_id = reader.IsDBNull(reader.GetOrdinal("updated_by_user_id"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("updated_by_user_id")),

                    update_date = reader.IsDBNull(reader.GetOrdinal("update_date"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("update_date")),

                    local_account = reader.IsDBNull(reader.GetOrdinal("local_account"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("local_account")),

                    Status = reader.IsDBNull(reader.GetOrdinal("Status"))
                        ? (byte?)null
                        : reader.GetByte(reader.GetOrdinal("Status")),

                    BoolFlag = reader.IsDBNull(reader.GetOrdinal("BoolFlag"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("BoolFlag")),

                    char_attrib_6 = reader.IsDBNull(reader.GetOrdinal("char_attrib_6")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_6")),
                    char_attrib_7 = reader.IsDBNull(reader.GetOrdinal("char_attrib_7")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_7")),
                    char_attrib_8 = reader.IsDBNull(reader.GetOrdinal("char_attrib_8")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_8")),
                    char_attrib_9 = reader.IsDBNull(reader.GetOrdinal("char_attrib_9")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_9")),
                    char_attrib_10 = reader.IsDBNull(reader.GetOrdinal("char_attrib_10")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_10")),
                    char_attrib_11 = reader.IsDBNull(reader.GetOrdinal("char_attrib_11")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_11")),
                    char_attrib_12 = reader.IsDBNull(reader.GetOrdinal("char_attrib_12")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_12")),
                    char_attrib_13 = reader.IsDBNull(reader.GetOrdinal("char_attrib_13")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_13")),
                    char_attrib_14 = reader.IsDBNull(reader.GetOrdinal("char_attrib_14")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_14")),
                    char_attrib_15 = reader.IsDBNull(reader.GetOrdinal("char_attrib_15")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_15")),
                    char_attrib_16 = reader.IsDBNull(reader.GetOrdinal("char_attrib_16")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_16")),
                    char_attrib_17 = reader.IsDBNull(reader.GetOrdinal("char_attrib_17")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_17")),
                    char_attrib_18 = reader.IsDBNull(reader.GetOrdinal("char_attrib_18")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_18")),
                    char_attrib_19 = reader.IsDBNull(reader.GetOrdinal("char_attrib_19")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_19")),
                    char_attrib_20 = reader.IsDBNull(reader.GetOrdinal("char_attrib_20")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_20")),

                    IsOverrideAccess = reader.IsDBNull(reader.GetOrdinal("IsOverrideAccess"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("IsOverrideAccess")),

                    //IsUpdated = reader.IsDBNull(reader.GetOrdinal("IsUpdated"))
                    //    ? (bool?)null
                    //    : reader.GetBoolean(reader.GetOrdinal("IsUpdated")),

                    char_attrib_21 = reader.IsDBNull(reader.GetOrdinal("char_attrib_21"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("char_attrib_21"))
                });
            }
            return list;
        }

        public async Task<List<UserAppAuthority>> GetByApplicationIdAsync(string Application_ID)
        {
            var list = new List<UserAppAuthority>();
            using var conn = new SqlConnection(_config["SqlConnection"]);
            using var cmd = new SqlCommand(
                @"select *from userAppAuthority 
                where application_id=@application_id", conn);

            cmd.Parameters.AddWithValue("@application_id", Application_ID);

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new UserAppAuthority
                {
                    UserAppAuthId = reader.GetDecimal(reader.GetOrdinal("user_app_auth_id")),
                    access_org_id = reader.GetString(reader.GetOrdinal("access_org_id")),
                    user_id = reader.GetString(reader.GetOrdinal("user_id")),
                    location_id = reader.GetString(reader.GetOrdinal("location_id")),
                    application_id = reader.GetString(reader.GetOrdinal("application_id")),

                    application_role_id = reader.IsDBNull(reader.GetOrdinal("application_role_id"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("application_role_id")),

                    crnt_owsh_typ_ds = reader.GetString(reader.GetOrdinal("crnt_owsh_typ_ds")),
                    default_authoritative_role = reader.GetString(reader.GetOrdinal("default_authoritative_role")),
                    record_source = reader.GetString(reader.GetOrdinal("record_source")),

                    char_attrib_1 = reader.IsDBNull(reader.GetOrdinal("char_attrib_1")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_1")),
                    char_attrib_2 = reader.IsDBNull(reader.GetOrdinal("char_attrib_2")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_2")),
                    char_attrib_3 = reader.IsDBNull(reader.GetOrdinal("char_attrib_3")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_3")),
                    char_attrib_4 = reader.IsDBNull(reader.GetOrdinal("char_attrib_4")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_4")),
                    char_attrib_5 = reader.IsDBNull(reader.GetOrdinal("char_attrib_5")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_5")),

                    role_prov_by_user_id = reader.GetString(reader.GetOrdinal("role_prov_by_user_id")),
                    created_in_trans_batch_id = reader.GetDecimal(reader.GetOrdinal("created_in_trans_batch_id")),

                    ack_in_eai_batch_id = reader.IsDBNull(reader.GetOrdinal("ack_in_eai_batch_id"))
                        ? (decimal?)null
                        : reader.GetDecimal(reader.GetOrdinal("ack_in_eai_batch_id")),

                    created_by_user_id = reader.GetString(reader.GetOrdinal("created_by_user_id")),
                    creation_date = reader.GetDateTime(reader.GetOrdinal("creation_date")),

                    updated_in_trans_batch_id = reader.IsDBNull(reader.GetOrdinal("updated_in_trans_batch_id"))
                        ? (decimal?)null
                        : reader.GetDecimal(reader.GetOrdinal("updated_in_trans_batch_id")),

                    updated_by_user_id = reader.IsDBNull(reader.GetOrdinal("updated_by_user_id"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("updated_by_user_id")),

                    update_date = reader.IsDBNull(reader.GetOrdinal("update_date"))
                        ? (DateTime?)null
                        : reader.GetDateTime(reader.GetOrdinal("update_date")),

                    local_account = reader.IsDBNull(reader.GetOrdinal("local_account"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("local_account")),

                    Status = reader.IsDBNull(reader.GetOrdinal("Status"))
                        ? (byte?)null
                        : reader.GetByte(reader.GetOrdinal("Status")),

                    BoolFlag = reader.IsDBNull(reader.GetOrdinal("BoolFlag"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("BoolFlag")),

                    char_attrib_6 = reader.IsDBNull(reader.GetOrdinal("char_attrib_6")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_6")),
                    char_attrib_7 = reader.IsDBNull(reader.GetOrdinal("char_attrib_7")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_7")),
                    char_attrib_8 = reader.IsDBNull(reader.GetOrdinal("char_attrib_8")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_8")),
                    char_attrib_9 = reader.IsDBNull(reader.GetOrdinal("char_attrib_9")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_9")),
                    char_attrib_10 = reader.IsDBNull(reader.GetOrdinal("char_attrib_10")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_10")),
                    char_attrib_11 = reader.IsDBNull(reader.GetOrdinal("char_attrib_11")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_11")),
                    char_attrib_12 = reader.IsDBNull(reader.GetOrdinal("char_attrib_12")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_12")),
                    char_attrib_13 = reader.IsDBNull(reader.GetOrdinal("char_attrib_13")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_13")),
                    char_attrib_14 = reader.IsDBNull(reader.GetOrdinal("char_attrib_14")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_14")),
                    char_attrib_15 = reader.IsDBNull(reader.GetOrdinal("char_attrib_15")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_15")),
                    char_attrib_16 = reader.IsDBNull(reader.GetOrdinal("char_attrib_16")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_16")),
                    char_attrib_17 = reader.IsDBNull(reader.GetOrdinal("char_attrib_17")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_17")),
                    char_attrib_18 = reader.IsDBNull(reader.GetOrdinal("char_attrib_18")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_18")),
                    char_attrib_19 = reader.IsDBNull(reader.GetOrdinal("char_attrib_19")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_19")),
                    char_attrib_20 = reader.IsDBNull(reader.GetOrdinal("char_attrib_20")) ? null : reader.GetString(reader.GetOrdinal("char_attrib_20")),

                    IsOverrideAccess = reader.IsDBNull(reader.GetOrdinal("IsOverrideAccess"))
                        ? (bool?)null
                        : reader.GetBoolean(reader.GetOrdinal("IsOverrideAccess")),

                    //IsUpdated = reader.IsDBNull(reader.GetOrdinal("IsUpdated"))
                    //    ? (bool?)null
                    //    : reader.GetBoolean(reader.GetOrdinal("IsUpdated")),

                    char_attrib_21 = reader.IsDBNull(reader.GetOrdinal("char_attrib_21"))
                        ? null
                        : reader.GetString(reader.GetOrdinal("char_attrib_21"))
                });
            }
            return list;
        }
    }
}
