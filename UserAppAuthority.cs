
using System.Text.Json.Serialization;

public class UserAppAuthority
{
    [JsonPropertyName("user_app_auth_id")]
    public decimal? UserAppAuthId { get; set; }

    [JsonPropertyName("coo_user_app_auth_id")]
    public decimal? CooUserAppAuthId { get; set; }

    public string access_org_id { get; set; }

    public string user_id { get; set; }

    public string location_id { get; set; }

    public string application_id { get; set; }

    public string application_role_id { get; set; }

    public string crnt_owsh_typ_ds { get; set; }

    public string default_authoritative_role { get; set; }

    public string record_source { get; set; }

    public DateTime? effectivity_date { get; set; }

    public string role_prov_by_user_id { get; set; }

    public decimal? created_in_trans_batch_id { get; set; }

    public decimal? updated_in_trans_batch_id { get; set; }

    public string created_by_user_id { get; set; }

    public string updated_by_user_id { get; set; }

    public DateTime? creation_date { get; set; }

    public DateTime? update_date { get; set; }

    public DateTime? delete_date { get; set; }

    public DateTime? deletion_date { get; set; }

    public string role_deleted_by_user_id { get; set; }

    public string deleted_by_user_id { get; set; }

    public decimal? deleted_in_trans_batch_id { get; set; }

    public decimal? deleted_in_eai_batch_id { get; set; }

    public decimal? ack_in_eai_batch_id { get; set; }

    public bool? local_account { get; set; }

    public bool? BoolFlag { get; set; }

    public bool? IsOverrideAccess { get; set; }

    public byte? Status { get; set; }

    // Char attributes
    public string char_attrib_1 { get; set; }
    public string char_attrib_2 { get; set; }
    public string char_attrib_3 { get; set; }
    public string char_attrib_4 { get; set; }
    public string char_attrib_5 { get; set; }
    public string char_attrib_6 { get; set; }
    public string char_attrib_7 { get; set; }
    public string char_attrib_8 { get; set; }
    public string char_attrib_9 { get; set; }
    public string char_attrib_10 { get; set; }
    public string char_attrib_11 { get; set; }
    public string char_attrib_12 { get; set; }
    public string char_attrib_13 { get; set; }
    public string char_attrib_14 { get; set; }
    public string char_attrib_15 { get; set; }
    public string char_attrib_16 { get; set; }
    public string char_attrib_17 { get; set; }
    public string char_attrib_18 { get; set; }
    public string char_attrib_19 { get; set; }
    public string char_attrib_20 { get; set; }
    public string char_attrib_21 { get; set; }

    // Computed properties
    [JsonIgnore]
    public decimal? EffectiveAuthorityId =>
        UserAppAuthId ?? CooUserAppAuthId;

    [JsonIgnore]
    public bool FutureStore =>
        CooUserAppAuthId.HasValue;
}
